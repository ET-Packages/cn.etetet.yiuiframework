﻿namespace ET.Client
{
    public static partial class YIUIViewComponentSystem
    {
        //关闭自己 同步
        //view 传了才有消息不传没有关闭消息根据需求处理
        public static void Close(this YIUIViewComponent self, Entity view, bool tween = true)
        {
            self.CloseAsync(view, tween).NoContext();
        }

        //关闭自己 同步
        //view 传了才有消息不传没有关闭消息根据需求处理
        public static async ETTask<bool> CloseAsync(this YIUIViewComponent self, Entity view, bool tween = true)
        {
            if (view != null)
            {
                var success = await YIUIEventSystem.Close(view);
                if (!success)
                {
                    Log.Info($"<color=yellow> 关闭事件返回不允许关闭View UI: {self.UIBase.OwnerGameObject.name} </color>");
                    return false;
                }
            }

            await self.UIWindow.InternalOnWindowCloseTween(tween);
            self.UIBase.SetActive(false);
            return true;
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