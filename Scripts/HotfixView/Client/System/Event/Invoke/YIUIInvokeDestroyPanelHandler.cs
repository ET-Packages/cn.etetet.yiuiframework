namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeDestroyPanelHandler : AInvokeHandler<YIUIInvokeDestroyPanel>
    {
        public override void Handle(YIUIInvokeDestroyPanel args)
        {
            if (YIUIMgrComponent.Inst == null || YIUIMgrComponent.Inst.IsDisposed)
            {
                return;
            }

            YIUIMgrComponent.Inst.DestroyPanel(args.PanelName);
        }
    }
}