namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeClosePanelSyncHandler : AInvokeHandler<YIUIInvokeClosePanel>
    {
        public override void Handle(YIUIInvokeClosePanel args)
        {
            YIUIMgrComponent.Inst?.ClosePanel(args.PanelName, args.Tween, args.IgnoreElse);
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeClosePanelAsyncHandler : AInvokeHandler<YIUIInvokeClosePanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(YIUIInvokeClosePanel args)
        {
            return await YIUIMgrComponent.Inst?.ClosePanelAsync(args.PanelName, args.Tween, args.IgnoreElse);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeViewClosePanelSyncHandler : AInvokeHandler<YIUIInvokeViewClosePanel>
    {
        public override void Handle(YIUIInvokeViewClosePanel args)
        {
            var panel = args.ViewComponent?.Parent?.Parent?.Parent?.GetComponent<YIUIPanelComponent>();
            if (panel == null)
            {
                Log.Error($"错误当前view的结构不满足标准 无法向上找到Panel 并且关闭 请检查 {args.ViewComponent?.UIBase?.UIName}");
                return;
            }

            panel.Close(args.Tween, args.IgnoreElse);
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeViewClosePanelAsyncHandler : AInvokeHandler<YIUIInvokeViewClosePanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(YIUIInvokeViewClosePanel args)
        {
            var panel = args.ViewComponent?.Parent?.Parent?.Parent?.GetComponent<YIUIPanelComponent>();
            if (panel == null)
            {
                Log.Error($"错误当前view的结构不满足标准 无法向上找到Panel 并且关闭 请检查 {args.ViewComponent?.UIBase?.UIName}");
                return false;
            }

            return await panel.CloseAsync(args.Tween, args.IgnoreElse);
        }
    }
}
