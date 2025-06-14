using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeReleaseSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_Release>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_Release args)
        {
            if (entity == null || entity.IsDisposed) return;
            entity.YIUILoad()?.Release(args.obj);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeReleaseInstantiateSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_ReleaseInstantiate>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_ReleaseInstantiate args)
        {
            if (entity == null || entity.IsDisposed) return;
            entity.YIUILoad()?.ReleaseInstantiate(args.obj);
        }
    }
}