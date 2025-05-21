﻿namespace ET
{
    /// <summary>
    /// 对于已知task可能会null的情况，提供一个安全的await方法
    /// 减少自己判断null的麻烦
    /// </summary>
    public static class ETTaskSafely
    {
        public static ETTask Await(ETTask task, bool logError = false)
        {
            if (task == null)
            {
                if (logError)
                {
                    Log.Error($"ETTaskSafely.Await: task is null");
                }

                return ETTask.CompletedTask;
            }

            return task;
        }

        public static async ETTask<T> Await<T>(ETTask<T> task, T defaultValue = default, bool logError = false)
        {
            if (task == null)
            {
                if (logError)
                {
                    Log.Error($"ETTaskSafely.Await: task is null");
                }

                return defaultValue;
            }

            return await task;
        }
    }
}