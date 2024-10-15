using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(YIUICloseCommonComponent))]
    public static partial class YIUICloseCommonComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this YIUICloseCommonComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this YIUICloseCommonComponent self)
        {
        }

        #region YIUIEvent开始
        
        [YIUIInvoke]
        private static async ETTask OnEventCloseInvoke(this YIUICloseCommonComponent self)
        {
            
            await ETTask.CompletedTask;
        }
        #endregion YIUIEvent结束
    }
}
