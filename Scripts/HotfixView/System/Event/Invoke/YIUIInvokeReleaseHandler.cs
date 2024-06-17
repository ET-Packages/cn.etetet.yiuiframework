using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeReleaseSyncHandler : AInvokeHandler<YIUIInvokeRelease>
    {
        public override void Handle(YIUIInvokeRelease args)
        {
            YIUILoadComponent.Inst?.Release(args.obj);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeReleaseInstantiateSyncHandler : AInvokeHandler<YIUIInvokeReleaseInstantiate>
    {
        public override void Handle(YIUIInvokeReleaseInstantiate args)
        {
            YIUILoadComponent.Inst?.ReleaseInstantiate(args.obj);
        }
    }
}
