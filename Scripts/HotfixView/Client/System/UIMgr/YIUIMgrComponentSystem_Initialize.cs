using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        //添加component的地方 调用 方便异步等待
        //因为UI初始化需要动态加载UI根节点
        public static async ETTask<bool> Initialize(this YIUIMgrComponent self)
        {
            //YIUI资源管理
            var loadResult = await self.AddComponent<YIUILoadComponent>().Initialize();
            if (!loadResult) return false;
            
            var constResult = await YIUIConstHelper.LoadAsset();
            if (!constResult) return false;

            //初始化UI绑定
            YIUIBindHelper.InternalGameGetUIBindVoFunc = YIUICodeGenerated.YIUIBindProvider.Get;
            var buildResult = YIUIBindHelper.InitAllBind();
            if (!buildResult) return false;

            //初始化其他UI框架中的管理器
            self.AddComponent<CountDownMgr>();

            //初始化所有YIUI相关 单例
            await YIUISingletonHelper.InitializeAll();

            //初始化YIUIRoot
            var rootResult = await self.InitRoot();
            if (!rootResult) return false;
            self.InitSafeArea();

            //其他模块各自初始化
            await EventSystem.Instance.PublishAsync(self.Scene(), new YIUIEventInitializeAfter());

            return true;
        }
    }
}