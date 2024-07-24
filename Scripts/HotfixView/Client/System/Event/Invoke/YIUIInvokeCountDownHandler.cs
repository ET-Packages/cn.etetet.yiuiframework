using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeCountDownAddSyncHandler : AInvokeHandler<YIUIInvokeCountDownAdd>
    {
        public override void Handle(YIUIInvokeCountDownAdd args)
        {
            if (args.TotalTime <= 0)
            {
                Log.Error($"总时长必须大于 0");
                return;
            }

            if (args.Interval <= 0)
            {
                //没有间隔则默认使用一次性回调
                args.Interval = args.TotalTime;
            }

            CountDownMgr.Inst?.Add(args.TimerCallback, args.TotalTime, args.Interval, args.Forever, args.StartCallback);
        }
    }

    [Invoke(EYIUIInvokeType.SyncHandler_1)]
    public class YIUIInvokeCountDownAddSyncHandler_1 : AInvokeHandler<YIUIInvokeCountDownAdd, bool>
    {
        public override bool Handle(YIUIInvokeCountDownAdd args)
        {
            if (args.TotalTime <= 0)
            {
                Log.Error($"总时长必须大于 0");
                return false;
            }

            if (args.Interval <= 0)
            {
                //没有间隔则默认使用一次性回调
                args.Interval = args.TotalTime;
            }

            return CountDownMgr.Inst?.Add(args.TimerCallback, args.TotalTime, args.Interval, args.Forever, args.StartCallback) ?? false;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeCountDownRemoveSyncHandler : AInvokeHandler<YIUIInvokeCountDownRemove>
    {
        public override void Handle(YIUIInvokeCountDownRemove args)
        {
            CountDownMgr.Inst?.Remove(args.TimerCallback);
        }
    }

    [Invoke(EYIUIInvokeType.SyncHandler_1)]
    public class YIUIInvokeCountDownRemoveSyncHandler_1 : AInvokeHandler<YIUIInvokeCountDownRemove, bool>
    {
        public override bool Handle(YIUIInvokeCountDownRemove args)
        {
            return CountDownMgr.Inst?.Remove(args.TimerCallback) ?? false;
        }
    }
}
