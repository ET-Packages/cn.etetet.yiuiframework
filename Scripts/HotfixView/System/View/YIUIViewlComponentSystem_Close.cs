namespace ET.Client
{
    public static partial class YIUIViewComponentSystem
    {
        //view 关闭自己 同步
        public static void Close(this YIUIViewComponent self, bool tween = true)
        {
            self.CloseAsync(tween).NoContext();
        }

        //view 关闭自己 异步
        public static async ETTask CloseAsync(this YIUIViewComponent self, bool tween = true)
        {
            await self.UIWindow.InternalOnWindowCloseTween(tween);
            self.UIBase.SetActive(false);
        }

        //标准view 可快捷关闭panel 需要满足panel的结构 同步
        public static void ClosePanel(this YIUIViewComponent self, bool tween = true, bool ignoreElse = false)
        {
            EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeViewClosePanel
                                                 {
                                                     ViewComponent = self,
                                                     Tween         = tween,
                                                     IgnoreElse    = ignoreElse
                                                 });
        }

        //标准view 可快捷关闭panel 需要满足panel的结构 异步
        public static async ETTask<bool> ClosePanelAsync(this YIUIViewComponent self, bool tween = true, bool ignoreElse = false)
        {
            return await EventSystem.Instance?.YIUIInvokeAsync<YIUIInvokeViewClosePanel, ETTask<bool>>(new YIUIInvokeViewClosePanel
                    {
                        ViewComponent = self,
                        Tween         = tween,
                        IgnoreElse    = ignoreElse
                    });
        }
    }
}
