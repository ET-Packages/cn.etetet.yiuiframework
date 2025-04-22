using System;

namespace ET.Client
{
    /// <summary>
    /// 关闭动画消息
    /// </summary>
    public static partial class YIUIEventSystem
    {
        public static async ETTask CloseTween(Entity component)
        {
            if (component == null || component.IsDisposed)
            {
                return;
            }

            var iYIUICloseTweenSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUICloseTweenSystem));
            if (iYIUICloseTweenSystems == null)
            {
                return;
            }

            foreach (IYIUICloseTweenSystem aYIUICloseTweenSystem in iYIUICloseTweenSystems)
            {
                if (aYIUICloseTweenSystem == null)
                {
                    continue;
                }

                try
                {
                    await aYIUICloseTweenSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}