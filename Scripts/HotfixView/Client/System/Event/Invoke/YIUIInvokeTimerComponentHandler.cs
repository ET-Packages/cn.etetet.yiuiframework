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
            if (args.CancellationToken == null)
            {
                await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitAsync(args.Time);
            }
            else
            {
                await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitAsync(args.Time).NewContext(args.CancellationToken);
            }
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitSecondAsyncHandler : AInvokeHandler<YIUIInvokeWaitSecondAsync, ETTask>
    {
        public override async ETTask Handle(YIUIInvokeWaitSecondAsync args)
        {
            if (args.CancellationToken == null)
            {
                await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitAsync((long)(args.Time * 1000));
            }
            else
            {
                await YIUIMgrComponent.Inst?.Root().GetComponent<TimerComponent>().WaitAsync((long)(args.Time * 1000)).NewContext(args.CancellationToken);
            }
        }
    }
}