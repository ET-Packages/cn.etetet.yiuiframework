﻿using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class UIMenuItemHelper
    {
        /// <summary>
        /// 克隆对象 根据传入的路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject CloneGameObjectByPath(string path, Transform parent = null)
        {
            var loadSource = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (loadSource == null)
            {
                UnityTipsHelper.ShowError($"未知错误 没有加载到源数据 请检查 {path}");
                return null;
            }

            var newGameObj = Object.Instantiate(loadSource, parent, false);
            if (newGameObj.name.EndsWith("(Clone)"))
                newGameObj.name = newGameObj.name.Replace("(Clone)", "");

            return newGameObj;
        }

        public static GameObject CopyGameObject(GameObject obj)
        {
            var newGameObj = Object.Instantiate(obj);

            if (newGameObj.name.EndsWith("(Clone)"))
                newGameObj.name = newGameObj.name.Replace("(Clone)", "");

            return newGameObj;
        }

        internal static GameObject SelectAssetAtPath(string savePath)
        {
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
            var selectPath = savePath.Replace("Assets/../", "");
            var assetObj   = (GameObject)AssetDatabase.LoadAssetAtPath(selectPath, typeof(Object));
            var cde        = assetObj.GetComponent<UIBindCDETable>();
            cde.AutoCheck();
            EditorGUIUtility.PingObject(assetObj);
            Selection.activeObject = assetObj;
            return assetObj;
        }
    }
}