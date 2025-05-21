namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeRootOpenPanelAsyncHandler : AInvokeHandler<YIUIInvokeRootOpenPanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(YIUIInvokeRootOpenPanel args)
        {
            if (args.Root == null)
            {
                return false;
            }

            return await args.Root.OpenPanelAsync(args.PanelName) != null;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeRootOpenPanelSyncHandler : AInvokeHandler<YIUIInvokeRootOpenPanel>
    {
        public override void Handle(YIUIInvokeRootOpenPanel args)
        {
            if (args.Root == null)
            {
                return;
            }

            args.Root.OpenPanelAsync(args.PanelName).NoContext();
        }
    }
}