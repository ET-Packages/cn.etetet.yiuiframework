#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework
{
    public sealed partial class UIPanelSplitData
    {
        [OdinSerialize]
        [LabelText("生成通用界面枚举")]
        [ShowIf("ShowCreatePanelViewEnum")]
        internal bool CreatePanelViewEnum = true;

        internal bool ShowCreatePanelViewEnum()
        {
            return (AllCommonView.Count + AllCreateView.Count + AllPopupView.Count) >= 1;
        }

        [GUIColor(0, 1, 1)]
        [Button("检查拆分数据", 30)]
        private void AutoCheckBtn()
        {
            AutoCheck();
        }

        internal bool AutoCheck()
        {
            if (Panel == null)
            {
                Debug.LogError($"没有找到 Panel");
                return false;
            }

            if (!CheckPanelName()) return false;

            if (AllViewParent == null || AllViewParent.name != YIUIConstHelper.Const.UIAllViewParentName)
            {
                AllViewParent = Panel.transform.FindChildByName(YIUIConstHelper.Const.UIAllViewParentName)
                        .GetComponent<RectTransform>();
            }

            if (AllPopupViewParent == null || AllPopupViewParent.name != YIUIConstHelper.Const.UIAllPopupViewParentName)
            {
                AllPopupViewParent = Panel.transform.FindChildByName(YIUIConstHelper.Const.UIAllPopupViewParentName)
                        .GetComponent<RectTransform>();
            }

            if (AllViewParent != null)
            {
                CheckViewParent(AllCommonView, AllViewParent);
                CheckViewParent(AllCreateView, AllViewParent);
            }
            else
            {
                AllCommonView.Clear();
                AllCreateView.Clear();
            }

            if (AllPopupViewParent == null)
            {
                CheckViewParent(AllPopupView, AllPopupViewParent);
            }
            else
            {
                AllPopupView.Clear();
            }

            if (AllViewParent != null)
            {
                CheckViewName(AllCommonView);
                CheckViewName(AllCreateView);
            }

            if (AllPopupViewParent == null)
            {
                CheckViewName(AllPopupView);
            }

            var isSource = Panel.name.EndsWith($"{YIUIConstHelper.Const.UIPanelSourceName}");
            if (AllViewParent != null)
            {
                CheckChild(AllCommonView, isSource, true);
                CheckChild(AllCreateView, isSource, false);
            }

            if (AllPopupViewParent == null)
            {
                CheckChild(AllPopupView, isSource, false);
            }

            var hashList = new HashSet<RectTransform>();
            CheckRepetition(ref hashList, AllCommonView);
            CheckRepetition(ref hashList, AllCreateView);
            CheckRepetition(ref hashList, AllPopupView);

            return true;
        }

        private bool CheckPanelName()
        {
            var qualifiedName = NameUtility.ToFirstUpper(Panel.name);
            if (Panel.name != qualifiedName)
            {
                Panel.name = qualifiedName;
            }

            var sourceName = Panel.name == YIUIConstHelper.Const.UIYIUIPanelSourceName;
            var panelName = Panel.name == $"{YIUIConstHelper.Const.UIProjectName}{YIUIConstHelper.Const.UIPanelName}";
            if (sourceName || panelName)
            {
                Debug.LogError(
                    $"当前是默认名称 请手动修改名称 {(sourceName ? $"Xxx{YIUIConstHelper.Const.UIPanelSourceName}" : "")} {(panelName ? $"Xxx{YIUIConstHelper.Const.UIPanelName}" : "")}");
                return false;
            }

            var sourceEndsWith = Panel.name.EndsWith($"{YIUIConstHelper.Const.UIPanelSourceName}");
            var panelEndsWith = Panel.name.EndsWith($"{YIUIConstHelper.Const.UIPanelName}");

            if (sourceEndsWith || panelEndsWith)
            {
                return true;
            }

            Debug.LogError($"{Panel.name} 命名必须以指定规则结尾: [{YIUIConstHelper.Const.UIPanelName} / {YIUIConstHelper.Const.UIPanelSourceName}] 请勿随意修改");
            return false;
        }

        private void CheckChild(List<RectTransform> list, bool isSource, bool needView)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (current == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (isSource || needView)
                {
                    if (isSource)
                    {
                        if (current.childCount != 1)
                        {
                            list.RemoveAt(i);
                            Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 需要View 有且只能有1个",
                                current);
                            continue;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < current.childCount; j++)
                        {
                            Object.DestroyImmediate(current.GetChild(0).gameObject);
                        }

                        var viewName = current.name.Replace(YIUIConstHelper.Const.UIParentName, "");
                        var viewPrefab = AssetDatabase.FindAssets($"{viewName} t:Prefab", null);
                        string viewPath;
                        if (viewPrefab is not { Length: > 0 })
                        {
                            viewPath = ViewSaveAsPrefabAsset(current);
                        }
                        else
                        {
                            viewPath = AssetDatabase.GUIDToAssetPath(viewPrefab[0]);
                        }

                        if (string.IsNullOrEmpty(viewPath))
                        {
                            list.RemoveAt(i);
                            continue;
                        }

                        var viewAsset = (GameObject)AssetDatabase.LoadAssetAtPath(viewPath, typeof(Object));
                        PrefabUtility.InstantiatePrefab(viewAsset, current);
                    }
                }
                else
                {
                    for (int j = 0; j < current.childCount; j++)
                    {
                        Object.DestroyImmediate(current.GetChild(0).gameObject);
                    }

                    if (string.IsNullOrEmpty(ViewSaveAsPrefabAsset(current)))
                    {
                        list.RemoveAt(i);
                        continue;
                    }
                }
            }
        }

        private string ViewSaveAsPrefabAsset(RectTransform current)
        {
            var viewName = current.name.Replace(YIUIConstHelper.Const.UIParentName, "");
            var view = CreateYIUIView(viewName);
            var source = Panel.GetComponent<UIBindCDETable>();
            var pkgName = source?.PkgName;
            if (string.IsNullOrEmpty(pkgName))
            {
                Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] Panel 不是CDE 且没有找到PakName",
                    current);
                return "";
            }

            var path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(source);
            var loadSource = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            if (loadSource == null)
            {
                Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 没有找到资源 {path}",
                    current);
                return "";
            }

            string savePath = "";
            if (UIOperationHelper.CheckUIIsPackages(loadSource, false))
            {
                var etPkgName = UIOperationHelper.GetETPackagesName(loadSource, false);
                savePath =
                        $"{string.Format(YIUIConstHelper.Const.UIProjectPackageResPath, etPkgName)}/{pkgName}/{YIUIConstHelper.Const.UIPrefabs}";
            }
            else
            {
                savePath = $"{YIUIConstHelper.Const.UIProjectResPath}/{pkgName}/{YIUIConstHelper.Const.UIPrefabs}";
            }

            var newPath = $"{savePath}/{viewName}.prefab";
            PrefabUtility.SaveAsPrefabAsset(view, newPath);
            Object.DestroyImmediate(view);
            AssetDatabase.SaveAssets();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
            return newPath.Replace("Assets/../", "");
        }

        private GameObject CreateYIUIView(string name)
        {
            var viewObject = new GameObject();
            viewObject.name = name;
            var viewRect = viewObject.GetOrAddComponent<RectTransform>();
            viewObject.GetOrAddComponent<CanvasRenderer>();
            var cdeTable = viewObject.GetOrAddComponent<UIBindCDETable>();
            cdeTable.UICodeType = EUICodeType.View;
            viewRect.ResetToFullScreen();
            viewObject.SetLayerRecursively(LayerMask.NameToLayer("UI"));
            return viewObject;
        }

        //命名检查
        private void CheckViewName(List<RectTransform> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (current == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                var qualifiedName = NameUtility.ToFirstUpper(current.name);
                if (current.name != qualifiedName)
                {
                    current.name = qualifiedName;
                }

                if (current.name == YIUIConstHelper.Const.UIYIUIViewParentName)
                {
                    list.RemoveAt(i);
                    Debug.LogError(
                        $"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent 当前是默认名称 请手动修改名称 Xxx{YIUIConstHelper.Const.UIViewParentName}",
                        current);
                    continue;
                }

                if (!current.name.EndsWith(YIUIConstHelper.Const.UIViewParentName))
                {
                    list.RemoveAt(i);
                    Debug.LogError(
                        $"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 命名必须以 {YIUIConstHelper.Const.UIViewParentName} 结尾 请勿随意修改",
                        current);
                    continue;
                }

                if (current.childCount >= 2)
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{current.parent?.parent?.name}的{current.parent?.name}下的ViewParent: [{current.name}] 只能有<=1个子物体 你应该通过其他方式实现层级结构",
                        current);
                    continue;
                }

                if (current.transform.childCount == 1)
                {
                    var firstChild = current.transform.GetChild(0);
                    var viewCde = firstChild.GetOrAddComponent<UIBindCDETable>();
                    viewCde.gameObject.name = current.name.Replace(YIUIConstHelper.Const.UIParentName, "");
                }
            }
        }

        //检查null / 父级
        private void CheckViewParent(List<RectTransform> list, RectTransform parent)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                var current = list[i];

                var parentP = current.parent;
                if (parentP == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (parentP != parent)
                {
                    list.RemoveAt(i);

                    //因为只有2个父级 所以如果不是这个就会自动帮你移动到另外一个上面
                    //如果多了还是不要自动了
                    var currentParentName = parentP.name;
                    if (currentParentName == YIUIConstHelper.Const.UIAllViewParentName)
                    {
                        AllCreateView.Add(current);
                    }
                    else if (currentParentName == YIUIConstHelper.Const.UIAllPopupViewParentName)
                    {
                        AllPopupView.Add(current);
                    }
                }
            }
        }

        //检查重复
        private void CheckRepetition(ref HashSet<RectTransform> hashList, List<RectTransform> list)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var current = list[i];
                if (hashList.Contains(current))
                {
                    list.RemoveAt(i);
                    Debug.LogError($"{Panel.name} / {current.name} 重复存在 已移除 请检查");
                }
                else
                {
                    hashList.Add(current);
                }
            }
        }
    }
}
#endif