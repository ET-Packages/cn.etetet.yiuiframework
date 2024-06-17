namespace ET.Client
{
    public static partial class YIUIPanelComponentSystem
    {
        public static void Close(this YIUIPanelComponent self, bool tween = true, bool ignoreElse = false)
        {
            self.CloseAsync(tween, ignoreElse).NoContext();
        }

        public static async ETTask<bool> CloseAsync(this YIUIPanelComponent self, bool tween = true, bool ignoreElse = false)
        {
            return await EventSystem.Instance?.YIUIInvokeAsync<YIUIInvokeClosePanel, ETTask<bool>>(new YIUIInvokeClosePanel
                                                                                                   {
                                                                                                       PanelName  = self.UIBase.UIName,
                                                                                                       Tween      = tween,
                                                                                                       IgnoreElse = ignoreElse
                                                                                                   });
        }

        public static void Home<T>(this YIUIPanelComponent self, bool tween = true) where T : Entity
        {
            EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeHomePanel
                                                 {
                                                     PanelName = typeof(T).Name,
                                                     Tween     = tween
                                                 });
        }

        public static void Home(this YIUIPanelComponent self, string homeName, bool tween = true)
        {
            EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeHomePanel
                                                 {
                                                     PanelName = homeName,
                                                     Tween     = tween
                                                 });
        }
    }
}
