namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeDestroyPanelHandler : AInvokeHandler<YIUIInvokeDestroyPanel>
    {
        public override void Handle(YIUIInvokeDestroyPanel args)
        {
            YIUIMgrComponent.Inst?.DestroyPanel(args.PanelName);
        }
    }
}
