#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// UIBase 模块
    /// </summary>
    [HideReferenceObjectPicker]
    public class CreateUIBaseModule : BaseCreateModule
    {
        [LabelText("YIUI项目命名空间")]
        [ShowInInspector]
        [ReadOnly]
        private const string UINamespace = YIUIConst.UINamespace;

        [LabelText("YIUI项目资源路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        private const string UIProjectResPath = YIUIConst.UIProjectResPath;

        [LabelText("YIUI项目脚本路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        private const string UIGenerationPath = YIUIConst.UIETComponentGenPath;

        [LabelText("YIUI项目自定义脚本路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        private const string UICodeScriptsPath = YIUIConst.UIETSystemGenPath;

        [HideLabel]
        [ShowInInspector]
        private CreateUIBaseEditorData UIBaseData = new CreateUIBaseEditorData();

        public override void Initialize()
        {
        }

        public override void OnDestroy()
        {
        }

        private const string m_CommonPkg = "Common";

        [Button("初始化项目")]
        private void CreateProject()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            EditorHelper.CreateExistsDirectory(UIGenerationPath);
            EditorHelper.CreateExistsDirectory(UIProjectResPath);
            EditorHelper.CreateExistsDirectory(UICodeScriptsPath);
            UICreateResModule.Create(m_CommonPkg); //默认初始化一个common模块
            CopyUIRoot();
            YIUIAutoTool.CloseWindowRefresh();
        }

        private void CopyUIRoot()
        {
            var loadRoot = (GameObject)AssetDatabase.LoadAssetAtPath(YIUIConst.UIRootPrefabPath, typeof(Object));
            if (loadRoot == null)
            {
                Debug.LogError($"没有找到原始UIRoot {YIUIConst.UIRootPrefabPath}");
                return;
            }

            var newGameObj = Object.Instantiate(loadRoot);
            var commonPath =
                    $"{UIProjectResPath}/{m_CommonPkg}/{YIUIConst.UIPrefabs}/{YIUIConst.UIRootName}.prefab";
            PrefabUtility.SaveAsPrefabAsset(newGameObj, commonPath);
            Object.DestroyImmediate(newGameObj);
        }
    }
}
#endif
