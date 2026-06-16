using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitFrameAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_WaitFrameAsync, ETTask>
    {
        public override async ETTask Handle(Entity entity, YIUIInvokeEntity_WaitFrameAsync args)
        {
            #if ET9
            await entity.Root().GetComponent<TimerComponent>().WaitFrameAsync();
            #else
            await entity.Root().TimerComponent.WaitFrameAsync();
            #endif
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_WaitAsync, ETTask>
    {
        public override async ETTask Handle(Entity entity, YIUIInvokeEntity_WaitAsync args)
        {
            if (args.CancellationToken == null)
            {
                #if ET9
                await entity.Root().GetComponent<TimerComponent>().WaitAsync(args.Time);
                #else
                await entity.Root().TimerComponent.WaitAsync(args.Time);
                #endif
            }
            else
            {
                #if ET9
                await entity.Root().GetComponent<TimerComponent>().WaitAsync(args.Time).NewContext(args.CancellationToken);
                #else
                await entity.Root().TimerComponent.WaitAsync(args.Time).NewContext(args.CancellationToken);
                #endif
            }
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeTimerComponentWaitSecondAsyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_WaitSecondAsync, ETTask>
    {
        public override async ETTask Handle(Entity entity, YIUIInvokeEntity_WaitSecondAsync args)
        {
            if (args.CancellationToken == null)
            {
                #if ET9
                await entity.Root().GetComponent<TimerComponent>().WaitAsync((long)(args.Time * 1000));
                #else
                await entity.Root().TimerComponent.WaitAsync((long)(args.Time * 1000));
                #endif
            }
            else
            {
                #if ET9
                await entity.Root().GetComponent<TimerComponent>().WaitAsync((long)(args.Time * 1000)).NewContext(args.CancellationToken);
                #else
                await entity.Root().TimerComponent.WaitAsync((long)(args.Time * 1000)).NewContext(args.CancellationToken);
                #endif
            }
        }
    }
}
