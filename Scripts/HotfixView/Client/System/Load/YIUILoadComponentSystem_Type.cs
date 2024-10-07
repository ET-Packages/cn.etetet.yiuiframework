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
        public static UnityObject LoadAsset(this YIUILoadComponent self, string pkgName, string resName, Type assetType)
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            var loadObj = load.Object;
            if (loadObj != null)
            {
                return loadObj;
            }

            var (obj, hashCode) = YIUILoadDI.LoadAssetFunc(pkgName, resName, assetType);
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

        public static async ETTask<UnityObject> LoadAssetAsync(this YIUILoadComponent self, string pkgName, string resName, Type assetType)
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            var loadObj = load.Object;
            if (loadObj != null)
            {
                return loadObj;
            }

            if (load.WaitAsync)
            {
                await self.Root().GetComponent<TimerComponent>().WaitUntil(() => !load.WaitAsync);

                loadObj = load.Object;
                if (loadObj != null)
                {
                    return loadObj;
                }
                else
                {
                    load.RemoveRefCount();
                    return null;
                }
            }

            load.SetWaitAsync(true);

            var (obj, hashCode) = await YIUILoadDI.LoadAssetAsyncFunc(pkgName, resName, assetType);

            if (obj == null)
            {
                load.SetWaitAsync(false);
                load.RemoveRefCount();
                return null;
            }

            if (!LoadHelper.AddLoadHandle(obj, load))
            {
                load.SetWaitAsync(false);
                load.RemoveRefCount();
                return null;
            }

            load.ResetHandle(obj, hashCode);
            load.SetWaitAsync(false);
            return obj;
        }

        private static void LoadAssetAsync(this YIUILoadComponent self, string pkgName, string resName, Type assetType, Action<UnityObject> action)
        {
            self.LoadAssetAsyncAction(pkgName, resName, assetType, action).NoContext();
        }

        private static async ETTask LoadAssetAsyncAction(this YIUILoadComponent self,
                                                         string                 pkgName,
                                                         string                 resName,
                                                         Type                   assetType,
                                                         Action<UnityObject>    action)
        {
            var asset = await self.LoadAssetAsync(pkgName, resName, assetType);
            if (asset == null)
            {
                Log.Error($"异步加载对象失败 {pkgName} {resName}");
                return;
            }

            action?.Invoke(asset);
        }
    }
}
