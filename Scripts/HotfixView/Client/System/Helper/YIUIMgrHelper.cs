using ET.Client;

namespace ET
{
    public static class YIUIMgrHelper
    {
        /// <summary>
        /// 从任意位置获取到UI管理器组件
        /// </summary>
        public static YIUIMgrComponent YIUIMgr(this Entity entity)
        {
            return entity?.Root()?.GetComponent<YIUIMgrComponent>(true);
        }

        /// <summary>
        /// 从任意位置获取到UI根组件
        /// </summary>
        public static YIUIRootComponent YIUIMgrRoot(this Entity entity)
        {
            return entity?.Root()?.GetComponent<YIUIRootComponent>(true);
        }

        /// <summary>
        /// 获取指定场景的UI根组件
        /// </summary>
        public static YIUIRootComponent YIUIRoot(this Entity entity)
        {
            return entity.Scene()?.GetComponent<YIUIRootComponent>(true);
        }

        /// <summary>
        /// 获取指定场景的UI根组件
        /// </summary>
        public static YIUIRootComponent YIUIRoot(this Scene scene)
        {
            return scene?.GetComponent<YIUIRootComponent>(true);
        }
    }
}