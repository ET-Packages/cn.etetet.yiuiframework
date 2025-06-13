using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeCoroutineLockHandler : AInvokeEntityHandler<YIUIInvokeEntity_CoroutineLock, ETTask<Entity>>
    {
        public override async ETTask<Entity> Handle(Entity entity, YIUIInvokeEntity_CoroutineLock args)
        {
            if (entity == null) return null;
            var lockType = args.LockType;
            if (lockType <= 0)
            {
                lockType = CoroutineLockType.YIUIInvokeCoroutineLock;
            }

            return await entity.Root().GetComponent<CoroutineLockComponent>().Wait(lockType, args.Lock);
        }
    }
}