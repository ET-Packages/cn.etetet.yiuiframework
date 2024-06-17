namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeHomePanelHandler : AInvokeHandler<YIUIInvokeHomePanel>
    {
        public override void Handle(YIUIInvokeHomePanel args)
        {
            YIUIMgrComponent.Inst?.HomePanel(args.PanelName, args.Tween, args.ForceHome).NoContext();
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeHomePanelWaitHandler : AInvokeHandler<YIUIInvokeHomePanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(YIUIInvokeHomePanel args)
        {
            return await YIUIMgrComponent.Inst?.HomePanel(args.PanelName, args.Tween, args.ForceHome);
        }
    }
}
