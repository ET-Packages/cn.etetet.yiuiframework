using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeCoroutineLockHandler : AInvokeHandler<YIUIInvokeCoroutineLock, ETTask<Entity>>
    {
        public override async ETTask<Entity> Handle(YIUIInvokeCoroutineLock args)
        {
            var lockType = args.LockType;
            if (lockType <= 0)
            {
                lockType = CoroutineLockType.YIUIFramework;
            }
            var coroutineLock = await YIUIMgrComponent.Inst?.Root().GetComponent<CoroutineLockComponent>().Wait(lockType, args.Lock);
            return coroutineLock;
        }
    }
}