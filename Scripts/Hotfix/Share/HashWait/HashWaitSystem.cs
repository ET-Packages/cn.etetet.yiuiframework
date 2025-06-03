﻿using System;
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
                self.Notify(hashCode, HashWaitError.Destroy, false);
            }
        }

        public static async ETTask<HashWaitError> Wait(this HashWait self, long hashCode)
        {
            if (self.m_HashWaitTasks.ContainsKey(hashCode))
            {
                Log.Error($"已经有Wait在等待 {hashCode} 的结果 不能重复等待");
                return HashWaitError.Error;
            }

            EntityRef<HashWait> selfRef = self;
            var cancellationToken = await ETTaskHelper.GetContextAsync<ETCancellationToken>();
            self = selfRef.Entity;
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
                selfRef.Entity?.Notify(hashCode, HashWaitError.Cancel, false);
            }
        }

        public static void Notify(this HashWait self, long hashCode, HashWaitError error = HashWaitError.Success, bool waitFrame = true)
        {
            if (!self.m_HashWaitTasks.Remove(hashCode, out var task))
            {
                return;
            }

            if (waitFrame)
            {
                Notify(self.Root(), task, error).NoContext();
            }
            else
            {
                task.SetResult(error);
            }
        }

        private static async ETTask Notify(Scene scene, ETTask<HashWaitError> task, HashWaitError error)
        {
            await scene?.GetComponent<TimerComponent>().WaitFrameAsync();
            task?.SetResult(error);
        }
    }
}