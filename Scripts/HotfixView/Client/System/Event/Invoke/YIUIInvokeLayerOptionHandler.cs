using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeBanLayerOptionForeverHandler : AInvokeHandler<YIUIInvokeBanLayerOptionForever, long>
    {
        public override long Handle(YIUIInvokeBanLayerOptionForever args)
        {
            return YIUIMgrComponent.Inst?.BanLayerOptionForever() ?? 0;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeRecoverLayerOptionForeverHandler : AInvokeHandler<YIUIInvokeRecoverLayerOptionForever>
    {
        public override void Handle(YIUIInvokeRecoverLayerOptionForever args)
        {
            YIUIMgrComponent.Inst?.RecoverLayerOptionForever(args.ForeverCode);
        }
    }
}
