namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeRootOpenPanelAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_SceneOpenPanel, ETTask<bool>>
    {
        public override async ETTask<bool> Handle(Entity entity, YIUIInvokeEntity_SceneOpenPanel args)
        {
            if (entity.IsDisposed)
            {
                return false;
            }

            return await entity.YIUISceneRoot().OpenPanelAsync(args.PanelName) != null;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeRootOpenPanelSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_SceneOpenPanel>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_SceneOpenPanel args)
        {
            if (entity.IsDisposed)
            {
                return;
            }

            entity.YIUISceneRoot().OpenPanelAsync(args.PanelName).NoContext();
        }
    }
}