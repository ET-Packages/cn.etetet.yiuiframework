using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        //添加component的地方 调用 方便异步等待
        //因为UI初始化需要动态加载UI根节点
        public static async ETTask Initialize(this YIUIMgrComponent self)
        {
            //初始化所有YIUI相关 单例
            await YIUISingletonHelper.InitializeAll();

            //初始化其他UI框架中的管理器
            self.AddComponent<CountDownMgr>();

            //初始化UIRoot
            await self.InitRoot();
            self.InitSafeArea();
        }
    }
}
