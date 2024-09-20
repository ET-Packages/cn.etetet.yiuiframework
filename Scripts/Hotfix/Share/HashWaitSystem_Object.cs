﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public static partial class HashWaitSystem
    {
        public static async ETTask<HashWaitError> WaitObject(this HashWait self, object target)
        {
            var hash = target.GetHashCode();
            if (self.m_HashWaitTasks.ContainsKey(hash))
            {
                Log.Error($"已经有Wait在等待 {target.GetType().Name} 的结果 不能重复等待");
                return HashWaitError.Error;
            }

            return await self.Wait(hash);
        }

        public static void NotifyObject(this HashWait self, object target, HashWaitError error = HashWaitError.Success, bool waitFrame = true)
        {
            self.Notify(target.GetHashCode(), error, waitFrame);
        }
    }
}
