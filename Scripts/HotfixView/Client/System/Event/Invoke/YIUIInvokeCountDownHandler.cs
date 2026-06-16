using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeCountDownAddSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_CountDownAdd>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_CountDownAdd args)
        {
            if (args.TotalTime < 0)
            {
                Log.Error($"总时长必须>= 0  (0=无限)");
                return;
            }

            if (args.Interval <= 0)
            {
                if (args.TotalTime <= 0)
                {
                    Log.Error($"没有间隔,总时长不能<=0");
                    return;
                }

                //没有间隔则默认使用一次性回调
                args.Interval = args.TotalTime;
            }

            CountDownMgr countDownMgr = entity?.YIUICountDown();
            if (countDownMgr != null)
            {
                countDownMgr.Add(args);
            }
        }
    }

    [Invoke(EYIUIInvokeType.SyncHandler_1)]
    public class YIUIInvokeCountDownAddSyncHandler_1 : AInvokeEntityHandler<YIUIInvokeEntity_CountDownAdd, bool>
    {
        public override bool Handle(Entity entity, YIUIInvokeEntity_CountDownAdd args)
        {
            if (args.TotalTime < 0)
            {
                Log.Error($"总时长必须>= 0  (0=无限)");
                return false;
            }

            if (args.Interval <= 0)
            {
                if (args.TotalTime <= 0)
                {
                    Log.Error($"没有间隔,总时长不能<=0");
                    return false;
                }

                //没有间隔则默认使用一次性回调
                args.Interval = args.TotalTime;
            }

            return entity.YIUICountDown().Add(args);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeCountDownRemoveSyncHandler : AInvokeEntityHandler<YIUIInvokeEntity_CountDownRemove>
    {
        public override void Handle(Entity entity, YIUIInvokeEntity_CountDownRemove args)
        {
            CountDownMgr countDownMgr = entity?.YIUICountDown();
            if (countDownMgr != null)
            {
                countDownMgr.Remove(args);
            }
        }
    }

    [Invoke(EYIUIInvokeType.SyncHandler_1)]
    public class YIUIInvokeCountDownRemoveSyncHandler_1 : AInvokeEntityHandler<YIUIInvokeEntity_CountDownRemove, bool>
    {
        public override bool Handle(Entity entity, YIUIInvokeEntity_CountDownRemove args)
        {
            CountDownMgr countDownMgr = entity?.YIUICountDown();
            return countDownMgr != null && countDownMgr.Remove(args);
        }
    }
}