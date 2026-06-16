using System;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// 不使用泛型 使用type加载的方式
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        public static async ETTask<UnityObject> LoadAssetAsync(this YIUILoadComponent self, string pkgName, string resName, Type assetType)
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            if (load.Object != null)
            {
                return load.Object;
            }

            #if ET9
            using var _ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUILoad, load.NameCode);
            #else
            using var _ = await self.Root().CoroutineLockComponent.Wait(CoroutineLockType.YIUILoad, load.NameCode);
            #endif

            if (load.Object != null)
            {
                return load.Object;
            }

            var (obj, hashCode) = await YIUILoadDI.LoadAssetAsyncFunc(pkgName, resName, assetType);

            if (obj == null)
            {
                load.RemoveRefCount();
                return null;
            }

            if (!LoadHelper.AddLoadHandle(obj, load))
            {
                load.RemoveRefCount();
                return null;
            }

            load.ResetHandle(obj, hashCode);
            return obj;
        }
    }
}