using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// UI用 加载器
    /// 扩展 GameObject快捷方法 需成对使用
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        /// <summary>
        /// 异步加载 并实例化
        /// </summary>
        public static async ETTask<GameObject> LoadAssetAsyncInstantiate(this YIUILoadComponent self, string pkgName, string resName)
        {
            var asset = await self.LoadAssetAsync<GameObject>(pkgName, resName);
            if (asset == null) return null;
            var obj = UnityObject.Instantiate(asset);
            YIUILoadHelperStatic.g_ObjectMap.Add(obj, asset);
            return obj;
        }

        /// <summary>
        /// 释放由 实例化出来的GameObject
        /// </summary>
        public static void ReleaseInstantiate(this YIUILoadComponent self, UnityObject gameObject)
        {
            if (!YIUILoadHelperStatic.g_ObjectMap.Remove(gameObject, out var asset)) return;
            self.Release(asset);
        }
    }
}