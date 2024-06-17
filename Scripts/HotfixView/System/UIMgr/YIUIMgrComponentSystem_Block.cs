using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        #region 永久屏蔽 forever

        //永久屏蔽
        //适用于 不知道要屏蔽多久 但是能保证可以成对调用的
        //这个没有放到API类中
        //因为如果你不能保证请不要用
        //至少过程中要try起来finally 保证不会出错 否则请不要使用这个功能
        public static long BanLayerOptionForever(this YIUIMgrComponent self)
        {
            self.SetLayerBlockOption(false);
            var foreverBlockCode = IdGenerater.Instance.GenerateId();
            self.m_AllForeverBlockCode.Add(foreverBlockCode);
            return foreverBlockCode;
        }

        //恢复永久屏蔽
        public static void RecoverLayerOptionForever(this YIUIMgrComponent self, long code)
        {
            if (!self.m_AllForeverBlockCode.Contains(code))
            {
                return;
            }

            self.m_AllForeverBlockCode.Remove(code);

            if (!self.IsForeverBlock)
            {
                //如果当前有其他倒计时 就等待倒计时恢复
                //否则可直接恢复
                if (self.m_LastCountDownGuid == 0)
                {
                    self.SetLayerBlockOption(true);
                }
            }
        }

        #endregion

        //如果当前被屏蔽了操作 可以拿到还有多久操作会恢复
        public static float GetLastRecoverOptionResidueTime(this YIUIMgrComponent self)
        {
            if (self.CanLayerBlockOption)
            {
                return 0;
            }

            return self.LastRecoverOptionTime - Time.unscaledTime;
        }

        /// <summary>
        /// 禁止层级操作
        /// </summary>
        /// <param name="time">需要禁止的时间</param>
        public static void BanLayerOption(this YIUIMgrComponent self, float time = 1f)
        {
            self.BanLayerOptionCountDown(time);
        }

        /// <summary>
        /// 强制恢复层级到可操作状态
        /// 此方法会强制打断倒计时 
        /// 清除所有永久屏蔽
        /// 根据需求调用
        /// </summary>
        public static void RecoverLayerOption(this YIUIMgrComponent self)
        {
            self.RecoverLayerOptionAll();
        }

        internal static void OnBlockDispose(this YIUIMgrComponent self)
        {
            self.RemoveLastCountDown();
        }

        private static void RemoveLastCountDown(this YIUIMgrComponent self)
        {
            CountDownMgr.Inst?.Remove(ref self.m_LastCountDownGuid);
        }

        //初始化添加屏蔽模块
        private static void InitAddUIBlock(this YIUIMgrComponent self)
        {
            self.m_LayerBlock = new GameObject("LayerBlock");
            var rect = self.m_LayerBlock.AddComponent<RectTransform>();
            self.m_LayerBlock.AddComponent<CanvasRenderer>();
            self.m_LayerBlock.AddComponent<UIBlock>();
            rect.SetParent(self.UILayerRoot);
            rect.SetAsLastSibling();
            rect.ResetToFullScreen();
            self.SetLayerBlockOption(true);
        }

        /// <summary>
        /// 设置UI是否可以操作
        /// 不能提供此API对外操作
        /// 因为有人设置过后就会忘记恢复
        /// 如果你确实需要你可以设置 禁止无限时间
        /// 之后调用恢复操作也可以做到
        /// </summary>
        /// <param name="value">true = 可以操作 = 屏蔽层会被隐藏</param>
        internal static void SetLayerBlockOption(this YIUIMgrComponent self, bool value)
        {
            self.m_LayerBlock.SetActive(!value);
        }

        /// <summary>
        /// 强制恢复层级到可操作状态
        /// 此方法会强制打断倒计时 根据需求调用
        /// </summary>
        internal static void RecoverLayerOptionAll(this YIUIMgrComponent self)
        {
            self.SetLayerBlockOption(true);
            self.m_LastRecoverOptionTime = 0;
            self.m_AllForeverBlockCode.Clear();
            self.RemoveLastCountDown();
        }

        #region 倒计时屏蔽

        /// <summary>
        /// 禁止层级操作
        /// 适合于知道想屏蔽多久 且可托管的操作
        /// </summary>
        /// <param name="time">需要禁止的时间</param>
        internal static void BanLayerOptionCountDown(this YIUIMgrComponent self, float time)
        {
            self.SetLayerBlockOption(false);

            var currentTime              = Time.realtimeSinceStartup; //当前的时间不受暂停影响
            var currentRecoverOptionTime = currentTime + time;

            //假设A 先禁止100秒
            //B 又想禁止10秒  显然 B这个就会被屏蔽最少需要等到A禁止的时间结束
            if (currentRecoverOptionTime > self.m_LastRecoverOptionTime)
            {
                self.m_LastRecoverOptionTime = currentRecoverOptionTime;
                self.RemoveLastCountDown();
                CountDownMgr.Inst?.Add(ref self.m_LastCountDownGuid, time, self.OnCountDownLayerOption);
            }
        }

        private static void OnCountDownLayerOption(this YIUIMgrComponent self, double residuetime, double elapsetime, double totaltime)
        {
            if (residuetime <= 0)
            {
                self.m_LastCountDownGuid = 0;

                if (self.IsForeverBlock)
                {
                    //如果当前被其他永久屏蔽 则等待永久屏蔽执行恢复
                    return;
                }

                self.SetLayerBlockOption(true);
            }
        }

        #endregion
    }
}
