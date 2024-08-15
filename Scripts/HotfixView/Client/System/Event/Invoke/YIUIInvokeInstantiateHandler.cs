using UnityEngine;
using YIUIFramework;
using UnityGameObject = UnityEngine.GameObject;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadInstantiateAsyncHandler : AInvokeHandler<YIUIInvokeLoadInstantiate, ETTask<Entity>>
    {
        public override async ETTask<Entity> Handle(YIUIInvokeLoadInstantiate args)
        {
            return await YIUIFactory.InstantiateAsync(args.LoadType, args.ParentEntity, args.ParentTransform);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadInstantiateSyncHandler : AInvokeHandler<YIUIInvokeLoadInstantiate, Entity>
    {
        public override Entity Handle(YIUIInvokeLoadInstantiate args)
        {
            return YIUIFactory.Instantiate(args.LoadType, args.ParentEntity, args.ParentTransform);
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeLoadInstantiateByVoAsyncHandler : AInvokeHandler<YIUIInvokeLoadInstantiateByVo, ETTask<Entity>>
    {
        public override async ETTask<Entity> Handle(YIUIInvokeLoadInstantiateByVo args)
        {
            return await YIUIFactory.InstantiateAsync(args.BindVo, args.ParentEntity, args.ParentTransform);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeLoadInstantiateByVoSyncHandler : AInvokeHandler<YIUIInvokeLoadInstantiateByVo, Entity>
    {
        public override Entity Handle(YIUIInvokeLoadInstantiateByVo args)
        {
            return YIUIFactory.Instantiate(args.BindVo, args.ParentEntity, args.ParentTransform);
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeInstantiateGameObjectSyncHandler : AInvokeHandler<YIUIInvokeInstantiateGameObject, UnityGameObject>
    {
        public override UnityGameObject Handle(YIUIInvokeInstantiateGameObject args)
        {
            return YIUIFactory.InstantiateGameObject(args.PkgName, args.ResName);
        }
    }

    [Invoke(EYIUIInvokeType.Async)]
    public class YIUIInvokeInstantiateGameObjectAsyncHandler : AInvokeHandler<YIUIInvokeInstantiateGameObject, ETTask<UnityGameObject>>
    {
        public override async ETTask<UnityGameObject> Handle(YIUIInvokeInstantiateGameObject args)
        {
            return await YIUIFactory.InstantiateGameObjectAsync(args.PkgName, args.ResName);
        }
    }
}
