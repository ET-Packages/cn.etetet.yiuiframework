using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [EntitySystemOf(typeof(HashWait))]
    public static partial class HashWaitSystem
    {
        [EntitySystem]
        private static void Awake(this HashWait self)
        {
            self.m_HashWaitTasks.Clear();
        }

        [EntitySystem]
        private static void Destroy(this HashWait self)
        {
            foreach (var hashCode in self.m_HashWaitTasks.Keys.ToArray())
            {
                self.Notify(hashCode, HashWaitError.Destroy);
            }
        }

        public static async ETTask<HashWaitError> Wait(this HashWait self, long hashCode)
        {
            if (self.m_HashWaitTasks.ContainsKey(hashCode))
            {
                Log.Error($"已经有Wait在等待 {hashCode} 的结果 不能重复等待");
                return HashWaitError.Error;
            }

            var cancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();

            var task = ETTask<HashWaitError>.Create(true);
            self.m_HashWaitTasks.Add(hashCode, task);

            try
            {
                cancellationToken?.Add(CancelAction);
                return await task;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            void CancelAction()
            {
                self.Notify(hashCode, HashWaitError.Cancel);
            }
        }

        public static async ETTask<HashWaitError> Wait(this HashWait self, long hashCode, long timeout)
        {
            if (timeout <= 0)
            {
                return await self.Wait(hashCode);
            }

            if (self.m_HashWaitTasks.ContainsKey(hashCode))
            {
                Log.Error($"已经有Wait在等待 {hashCode} 的结果 不能重复等待");
                return HashWaitError.Error;
            }

            var cancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();

            var task = ETTask<HashWaitError>.Create(true);

            self.m_HashWaitTasks.Add(hashCode, task);

            var timeoutToken = new ETCancellationToken();
            WaitTimeout().WithContext(timeoutToken);

            try
            {
                cancellationToken?.Add(CancelAction);
                var result = await task;
                timeoutToken.Cancel();
                return result;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }

            void CancelAction()
            {
                self.Notify(hashCode, HashWaitError.Cancel);
            }

            async ETTask WaitTimeout()
            {
                await self.Root().GetComponent<TimerComponent>().WaitAsync(timeout);
                if (timeoutToken.IsCancel())
                {
                    return;
                }

                if (task == null || task.IsCompleted)
                {
                    return;
                }

                self.Notify(hashCode, HashWaitError.Timeout);
            }
        }

        public static void Notify(this HashWait self, long hashCode, HashWaitError error = HashWaitError.Success)
        {
            if (!self.m_HashWaitTasks.Remove(hashCode, out var task))
            {
                return;
            }

            task.SetResult(error);
        }
    }
}
