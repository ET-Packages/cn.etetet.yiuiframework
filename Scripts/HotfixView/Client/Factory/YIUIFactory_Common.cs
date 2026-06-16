//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIFactory
    {
        public static async ETTask<GameObject> InstantiateGameObjectAsync(Scene scene, string pkgName, string resName)
        {
            EntityRef<Scene> sceneRef = scene;
            var obj = await scene.YIUILoad()?.LoadAssetAsyncInstantiate(pkgName, resName);
            if (obj == null)
            {
                Debug.LogError($"没有加载到这个资源 pkgName:[{pkgName}]/resName:[{resName}]");
                return null;
            }

            obj.AddComponent<YIUIReleaseInstantiate>().m_EntityRef = sceneRef.Entity;

            return obj;
        }
    }
}