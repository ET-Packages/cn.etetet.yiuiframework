using ET;
using System;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using Sirenix.Serialization;
#endif

namespace YIUIFramework
{
    public static class YIUIConstHelper
    {
        public const string YIUIConstAssetName = "YIUIConstAsset";
        public const string YIUIConstAssetPath = "Assets/GameRes/YIUI/YIUIConstAsset.txt";

        private static YIUIConstAsset m_YIUIConstAsset;

        #if !UNITY_EDITOR
        public static YIUIConstAsset Const => m_YIUIConstAsset;
        #endif

        public static async ETTask<bool> LoadAsset()
        {
            m_YIUIConstAsset = await OdinSerializationHelper.RuntimeLoad<YIUIConstAsset>(YIUIConstAssetName);
            return m_YIUIConstAsset != null;
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
            var constAsset = OdinSerializationHelper.EditorLoad<YIUIConstAsset>(YIUIConstAssetPath);

            if (constAsset == null)
            {
                constAsset = new YIUIConstAsset();
                var bytes = Sirenix.Serialization.SerializationUtility.SerializeValue(constAsset, DataFormat.JSON);
                WriteAllBytes($"{Application.dataPath}/../{YIUIConstAssetPath}", bytes);
            }

            return constAsset;
        }

        private static void WriteAllBytes(string path, byte[] bytes)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.LogError("写入文件失败: path =" + path + ", err=" + e);
            }
        }

        #endif
    }
}