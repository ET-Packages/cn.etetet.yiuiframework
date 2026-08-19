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
        private const float MinOutputScale = 0.01f; // 输出缩放比例下限，避免生成零像素图片。
        private const float MaxOutputScale = 1f; // 输出缩放比例上限，1表示保留设计分辨率。
        private const float DefaultOutputScale = 0.5f; // 编辑器工具默认以半尺寸快速查看整体布局。

        [AssetsOnly]
        [LabelText("UI Prefab")]
        public GameObject Prefab;

        [ReadOnly]
        [LabelText("最近导出")]
        public string LastOutputPath;

        [LabelText("输出缩放比例")]
        [PropertyRange(MinOutputScale, MaxOutputScale)]
        public float OutputScale = DefaultOutputScale;

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

        [Button("按设计分辨率导出 PNG", 30, Icon = SdfIconType.Download, IconAlignment = IconAlignment.LeftOfText)]
        public void CapturePreview()
        {
            if (!TryGetPrefabPath(out var prefabPath))
            {
                return;
            }

            try
            {
                var result = YIUIPrefabPreviewWindow.CapturePrefabToPng(
                    prefabPath, null, OutputScale);
                LastOutputPath = result.OutputPath;
                Debug.Log("YIUI预览完成：Prefab=" + result.PrefabPath +
                    "，PNG=" + result.OutputPath + "，设计分辨率=" +
                    result.DesignWidth + "x" + result.DesignHeight +
                    "，输出分辨率=" + result.Width + "x" + result.Height +
                    "，Scale=" + result.Scale + "，PlayMode=false");
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
