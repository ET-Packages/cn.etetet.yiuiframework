namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeRemoveUIResetHandler : AInvokeHandler<YIUIInvokeRemoveUIReset>
    {
        public override void Handle(YIUIInvokeRemoveUIReset args)
        {
            YIUIMgrComponent.Inst?.RemoveUIReset(args.PanelName);
        }
    }
}
