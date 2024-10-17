﻿//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// UI窗口组件
    /// </summary>
    [FriendOf(typeof(YIUIWindowComponent))]
    [EntitySystemOf(typeof(YIUIWindowComponent))]
    public static partial class YIUIWindowComponentSystem
    {
        [EntitySystem]
        private static void Awake(this YIUIWindowComponent self)
        {
            self.UIBindVo = self.UIBase.UIBindVo;
        }

        [EntitySystem]
        private static void Destroy(this YIUIWindowComponent self)
        {
        }

        internal static async ETTask InternalOnWindowOpenTween(this YIUIWindowComponent self, bool tween = true)
        {
            self.UIBase.SetActive(true);

            if (tween && (!self.WindowBanRepetitionOpenTween || !self.m_FirstOpenTween))
            {
                self.m_FirstOpenTween = true;

                if (self.WindowBanAwaitOpenTween)
                {
                    self.SealedOnWindowOpenTween().NoContext();
                }
                else
                {
                    await self.SealedOnWindowOpenTween();
                }
            }
            else
            {
                self.OnOpenTweenEnd();
            }
        }

        internal static async ETTask InternalOnWindowCloseTween(this YIUIWindowComponent self, bool tween = true)
        {
            if (tween && (!self.WindowBanRepetitionCloseTween || !self.m_FirstCloseTween))
            {
                self.m_FirstCloseTween = true;

                if (self.WindowBanAwaitCloseTween)
                {
                    self.SealedOnWindowCloseTween().NoContext();
                }
                else
                {
                    await self.SealedOnWindowCloseTween();
                }
            }
            else
            {
                self.OnCloseTweenEnd();
            }
        }

        //有可能没有动画 也有可能动画被跳过 反正无论如何都会有动画结束回调
        private static void OnOpenTweenEnd(this YIUIWindowComponent self)
        {
            YIUIEventSystem.OpenTweenEnd(self.UIBase.OwnerUIEntity);
        }

        private static void OnCloseTweenEnd(this YIUIWindowComponent self)
        {
            YIUIEventSystem.CloseTweenEnd(self.UIBase.OwnerUIEntity);
            self.UIBase.SetActive(false);
        }

        private static async ETTask SealedOnWindowOpenTween(this YIUIWindowComponent self)
        {
            if (YIUIConstHelper.Const.IsLowQuality || self.WindowBanOpenTween)
            {
                self.OnOpenTweenEnd();
                return;
            }

            var foreverCode = self.WindowAllowOptionByTween ?
                    0 : EventSystem.Instance?.YIUIInvokeSync<YIUIInvokeBanLayerOptionForever, long>(new YIUIInvokeBanLayerOptionForever()) ?? 0;

            try
            {
                await self.OnOpenTween();
            }
            catch (Exception e)
            {
                Debug.LogError($"{self.UIBase.UIResName} 打开动画执行报错 {e}");
            }
            finally
            {
                if (!self.WindowAllowOptionByTween)
                {
                    EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeRecoverLayerOptionForever
                                                         {
                                                             ForeverCode = foreverCode
                                                         });
                }

                self.OnOpenTweenEnd();
            }
        }

        private static async ETTask SealedOnWindowCloseTween(this YIUIWindowComponent self)
        {
            if (!self.UIBase.ActiveSelf || YIUIConstHelper.Const.IsLowQuality || self.WindowBanCloseTween)
            {
                self.OnCloseTweenEnd();
                return;
            }

            var foreverCode = self.WindowAllowOptionByTween ?
                    0 : EventSystem.Instance?.YIUIInvokeSync<YIUIInvokeBanLayerOptionForever, long>(new YIUIInvokeBanLayerOptionForever()) ?? 0;
            try
            {
                await self.OnCloseTween();
            }
            catch (Exception e)
            {
                Debug.LogError($"{self.UIBase.UIResName} 关闭动画执行报错 {e}");
            }
            finally
            {
                if (!self.WindowAllowOptionByTween)
                {
                    EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeRecoverLayerOptionForever
                                                         {
                                                             ForeverCode = foreverCode
                                                         });
                }

                self.OnCloseTweenEnd();
            }
        }

        private static async ETTask OnOpenTween(this YIUIWindowComponent self)
        {
            if (self._LastCloseETTask != null)
            {
                await self._LastCloseETTask;
            }

            if (self._LastOpenETTask != null)
            {
                await self._LastOpenETTask;
                return;
            }

            self._LastOpenETTask = ETTask.Create(true);
            var tweent = await YIUIEventSystem.OpenTween(self.UIBase.OwnerUIEntity);
            if (!tweent)
            {
                //panel会有默认动画
                //不要动画请在界面参数上调整 WindowBanTween
                //需要其他动画请实现动画事件
                if (self.UIBase.UIBindVo.CodeType == EUICodeType.Panel)
                    await WindowFadeAnim.In(self.UIBase?.OwnerGameObject);
            }

            if (self.IsDisposed) return;
            self._LastOpenETTask?.SetResult();
            self._LastOpenETTask = null;
        }

        private static async ETTask OnCloseTween(this YIUIWindowComponent self)
        {
            if (self._LastOpenETTask != null)
            {
                await self._LastOpenETTask;
            }

            if (self._LastCloseETTask != null)
            {
                await self._LastCloseETTask;
                return;
            }

            self._LastCloseETTask = ETTask.Create(true);
            var tweent = await YIUIEventSystem.CloseTween(self.UIBase.OwnerUIEntity);
            if (!tweent)
            {
                if (self.UIBase.UIBindVo.CodeType == EUICodeType.Panel)
                    await WindowFadeAnim.Out(self.UIBase?.OwnerGameObject);
            }

            if (self.IsDisposed) return;
            self._LastCloseETTask?.SetResult();
            self._LastCloseETTask = null;
        }
    }
}
