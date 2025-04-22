using System;

namespace ET.Client
{
    /// <summary>
    /// 打开动画消息
    /// </summary>
    public static partial class YIUIEventSystem
    {
        public static async ETTask OpenTween(Entity component)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var iYIUIOpenTweenSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIOpenTweenSystem));
            if (iYIUIOpenTweenSystems == null)
            {
                return;
            }

            foreach (IYIUIOpenTweenSystem aYIUIOpenTweenSystem in iYIUIOpenTweenSystems)
            {
                if (aYIUIOpenTweenSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUIOpenTweenSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}