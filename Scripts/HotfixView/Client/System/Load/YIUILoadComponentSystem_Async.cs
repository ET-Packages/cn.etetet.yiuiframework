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
            var loadObj = load.Object;
            if (loadObj != null)
            {
                return (T)loadObj;
            }

            if (load.WaitAsync)
            {
                await self.Root().GetComponent<TimerComponent>().WaitUntil(() => !load.WaitAsync);

                loadObj = load.Object;

                if (loadObj != null)
                {
                    return (T)loadObj;
                }
                else
                {
                    load.RemoveRefCount();
                    return null;
                }
            }

            load.SetWaitAsync(true);

            var (obj, hashCode) = await YIUILoadDI.LoadAssetAsyncFunc(pkgName, resName, typeof(T));

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
            return (T)obj;
        }

        /// <summary>
        /// 异步加载资源对象
        /// 回调类型
        /// </summary>
        internal static void LoadAssetAsync<T>(this YIUILoadComponent self, string pkgName, string resName, Action<T> action) where T : UnityObject
        {
            self.LoadAssetAsyncAction(pkgName, resName, action).NoContext();
        }

        private static async ETTask LoadAssetAsyncAction<T>(this YIUILoadComponent self, string pkgName, string resName, Action<T> action)
                where T : UnityObject
        {
            var asset = await self.LoadAssetAsync<T>(pkgName, resName);
            if (asset == null)
            {
                Debug.LogError($"异步加载对象失败 {pkgName} {resName}");
                return;
            }

            action?.Invoke(asset);
        }
    }
}
