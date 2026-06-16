using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    public static class MenuItemYIUIPanel
    {
        [MenuItem("Assets/YIUI/Create UIPanel", false, 1)]
        static void CreateYIUIPanelByFolder()
        {
            var activeObject = Selection.activeObject as DefaultAsset;
            if (activeObject == null)
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UIPrefabs} 下右键创建");
                return;
            }

            var path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!path.Contains(YIUIConstHelper.Const.UIProjectResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UIPrefabs} 下右键创建");
                return;
            }

            CreateYIUIPanelByPath(path);
        }

        public static bool CreateYIUIPanelByPath(string path, string uiName = null)
        {
            if (!path.Contains(YIUIConstHelper.Const.UIProjectResPath))
            {
                UnityTipsHelper.ShowError($"请在路径 {YIUIConstHelper.Const.UIProjectResPath}/xxx/{YIUIConstHelper.Const.UIPrefabs} 下右键创建");
                return false;
            }

            var saveName = string.IsNullOrEmpty(uiName)
                ? $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UIPanelName}"
                : uiName;
            var savePath = $"{path}/{saveName}.prefab";

            if (AssetDatabase.LoadAssetAtPath(savePath, typeof(Object)) != null)
            {
                UnityTipsHelper.ShowError($"已存在 请先重命名 {saveName}");
                return false;
            }

            var createPanel = CreateYIUIPanel(null, saveName);
            PrefabUtility.SaveAsPrefabAsset(createPanel, savePath);
            Object.DestroyImmediate(createPanel);
            UIMenuItemHelper.SelectAssetAtPath(savePath);
            return true;
        }

        private static GameObject CreateYIUIPanel(GameObject activeObject = null, string uiName = null)
        {
            //panel
            var panelObject = new GameObject();
            var panelRect   = panelObject.GetOrAddComponent<RectTransform>();
            panelObject.GetOrAddComponent<CanvasRenderer>();
            var cdeTable = panelObject.GetOrAddComponent<UIBindCDETable>();
            cdeTable.UICodeType  =  EUICodeType.Panel;
            cdeTable.IsSplitData =  false;
            cdeTable.PanelOption |= EPanelOption.TimeCache;

            var panelEditorData = cdeTable.PanelSplitData;
            panelEditorData.Panel = panelObject;
            panelObject.name      = string.IsNullOrEmpty(uiName)
                ? $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UIPanelName}"
                : uiName;
            if (activeObject != null)
            {
                panelRect.SetParent(activeObject.transform, false);
                EditorUtility.SetDirty(activeObject);
            }
            panelRect.ResetToFullScreen();

            //阻挡图
            var backgroundObject = new GameObject();
            var backgroundRect   = backgroundObject.GetOrAddComponent<RectTransform>();
            backgroundObject.GetOrAddComponent<CanvasRenderer>();
            backgroundObject.GetOrAddComponent<UIBlock>();
            backgroundObject.name = "UIBlockBG";
            backgroundRect.SetParent(panelRect, false);
            backgroundRect.ResetToFullScreen();

            panelObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            return panelObject;
        }
    }
}
