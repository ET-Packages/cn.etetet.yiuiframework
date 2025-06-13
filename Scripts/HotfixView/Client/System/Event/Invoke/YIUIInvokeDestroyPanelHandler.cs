namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeDestroyPanelHandler : AInvokeEntityHandler<YIUIInvokeEntity_DestroyPanel>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_DestroyPanel args)
        {
            if (entity.IsDisposed)
            {
                return;
            }

            entity.YIUIMgr().DestroyPanel(args.PanelName);
        }
    }
}