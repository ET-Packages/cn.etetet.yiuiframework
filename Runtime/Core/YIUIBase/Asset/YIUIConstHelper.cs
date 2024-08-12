using ET;
using UnityEngine;
using UnityObject = UnityEngine.Object;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

namespace YIUIFramework
{
    public static class YIUIConstHelper
    {
        public const string YIUIConstAssetName = "YIUIConstAsset";

        private static YIUIConstAsset m_YIUIConstAsset;

        #if !UNITY_EDITOR
        public static YIUIConstAsset Const => m_YIUIConstAsset;
        #endif

        public static async ETTask<bool> LoadAsset()
        {
            var loadResult = await EventSystem.Instance?.YIUIInvokeAsync<YIUIInvokeLoad, ETTask<UnityObject>>(new YIUIInvokeLoad
            {
                LoadType = typeof(YIUIConstAsset),
                ResName  = YIUIConstAssetName
            });

            if (loadResult == null)
            {
                Debug.LogError($"初始化失败 没有加载到目标数据 {YIUIConstAssetName} 请到YIUI工具中 全局设置 初始化项目");
                return false;
            }

            m_YIUIConstAsset = (YIUIConstAsset)loadResult;

            EventSystem.Instance?.YIUIInvokeSync(new YIUIInvokeRelease
            {
                obj = loadResult
            });
            return true;
        }

        #if UNITY_EDITOR

        public static YIUIConstAsset Const
        {
            get
            {
                if (m_YIUIConstAsset == null)
                {
                    m_YIUIConstAsset = LoadEditorAsset();
                }

                return m_YIUIConstAsset;
            }
        }

        private static YIUIConstAsset LoadEditorAsset()
        {
            var constAsset = Load<YIUIConstAsset>();

            if (constAsset == null)
            {
                constAsset = CreateAsset();
            }

            return constAsset;
        }

        private static YIUIConstAsset CreateAsset()
        {
            var constAsset = ScriptableObject.CreateInstance<YIUIConstAsset>();

            var assetFolder = constAsset.UIProjectResPath;
            if (!Directory.Exists(assetFolder))
                Directory.CreateDirectory(assetFolder);

            AssetDatabase.CreateAsset(constAsset, $"{assetFolder}/{YIUIConstAssetName}.asset");

            return constAsset;
        }

        private static T Load<T>(bool showLog = false)
                where T : ScriptableObject
        {
            var settingType = typeof(T);
            var guids       = AssetDatabase.FindAssets($"t:{settingType.Name}");
            if (guids.Length == 0)
            {
                if (showLog)
                {
                    Debug.LogError($"没有这个文件 : {settingType.Name}");
                }

                return null;
            }
            else
            {
                if (guids.Length != 1)
                {
                    foreach (var guid in guids)
                    {
                        string path = AssetDatabase.GUIDToAssetPath(guid);
                        Debug.LogError($"找到多个文件 : {path}");
                    }
                }

                string filePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                var    asset    = AssetDatabase.LoadAssetAtPath<T>(filePath);
                return asset;
            }
        }

        #endif
    }
}