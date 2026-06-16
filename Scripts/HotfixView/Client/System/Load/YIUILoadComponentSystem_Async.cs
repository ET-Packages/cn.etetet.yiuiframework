using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// 异步加载
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        /// <summary>
        /// 异步加载资源对象
        /// 返回类型
        /// </summary>
        internal static async ETTask<T> LoadAssetAsync<T>(this YIUILoadComponent self, string pkgName, string resName) where T : UnityObject
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            if (load.Object != null)
            {
                return (T)load.Object;
            }

            #if ET9
            using var _ = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUILoad, load.NameCode);
            #else
            using var _ = await self.Root().CoroutineLockComponent.Wait(CoroutineLockType.YIUILoad, load.NameCode);
            #endif

            if (load.Object != null)
            {
                return (T)load.Object;
            }

            var (obj, hashCode) = await YIUILoadDI.LoadAssetAsyncFunc(pkgName, resName, typeof(T));

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
            return (T)obj;
        }
    }
}
