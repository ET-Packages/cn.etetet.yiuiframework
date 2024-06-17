using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitFrameAsyncHandler : AInvokeHandler<YIUIInvokeWaitFrameAsync, ETTask>
    {
        public override async ETTask Handle(YIUIInvokeWaitFrameAsync args)
        {
            await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitFrameAsync();
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitAsyncHandler : AInvokeHandler<YIUIInvokeWaitAsync, ETTask>
    {
        public override async ETTask Handle(YIUIInvokeWaitAsync args)
        {
            await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitAsync(args.Time);
        }
    }
}
