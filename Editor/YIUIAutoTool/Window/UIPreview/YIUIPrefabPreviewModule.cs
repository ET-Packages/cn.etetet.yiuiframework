#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [YIUIAutoMenu("Prefab视觉预览", 200050)]
    public sealed class YIUIPrefabPreviewModule : BaseYIUIToolModule
    {
        [AssetsOnly]
        [LabelText("UI Prefab")]
        public GameObject Prefab;

        [ReadOnly]
        [LabelText("最近导出")]
        public string LastOutputPath;

        [Button("打开预览窗口", 30, Icon = SdfIconType.Eye, IconAlignment = IconAlignment.LeftOfText)]
        public void OpenPreviewWindow()
        {
            if (!TryGetPrefabPath(out _))
            {
                return;
            }

            try
            {
                YIUIPrefabPreviewWindow.OpenPrefab(Prefab);
            }
            catch (Exception exception)
            {
                Debug.LogError("打开Prefab视觉预览失败：Prefab=" + FormatPrefab() + "，原因=" + exception.Message);
            }
        }

        [Button("导出 1920x1080 PNG", 30, Icon = SdfIconType.Download, IconAlignment = IconAlignment.LeftOfText)]
        public void CapturePreview()
        {
            if (!TryGetPrefabPath(out var prefabPath))
            {
                return;
            }

            try
            {
                var result = YIUIPrefabPreviewWindow.CapturePrefabToPng(prefabPath);
                LastOutputPath = result.OutputPath;
                Debug.Log("YIUI预览完成：Prefab=" + result.PrefabPath + "，PNG=" + result.OutputPath + "，分辨率=" + result.Width + "x" + result.Height + "，PlayMode=false");
            }
            catch (Exception exception)
            {
                Debug.LogError("导出Prefab视觉预览失败：Prefab=" + FormatPrefab() + "，原因=" + exception.Message);
            }
        }

        [Button("在文件管理器中定位", 24, Icon = SdfIconType.Folder2Open, IconAlignment = IconAlignment.LeftOfText)]
        [EnableIf(nameof(HasOutputPath))]
        public void RevealOutput()
        {
            EditorUtility.RevealInFinder(LastOutputPath);
        }

        public override void Initialize()
        {
            var selected = Selection.activeObject as GameObject;
            var selectedPath = selected == null ? string.Empty : AssetDatabase.GetAssetPath(selected);
            if (selectedPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                Prefab = selected;
            }
        }

        private bool HasOutputPath()
        {
            return !string.IsNullOrWhiteSpace(LastOutputPath);
        }

        private bool TryGetPrefabPath(out string prefabPath)
        {
            prefabPath = Prefab == null ? string.Empty : AssetDatabase.GetAssetPath(Prefab);
            if (Prefab != null && prefabPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            Debug.LogError("Prefab视觉预览失败：请选择有效的.prefab资源，Prefab=" + FormatPrefab() + "，PrefabPath=" + (string.IsNullOrEmpty(prefabPath) ? "<空>" : prefabPath));
            return false;
        }

        private string FormatPrefab()
        {
            return Prefab == null ? "<null>" : Prefab.name;
        }
    }
}

#endif
