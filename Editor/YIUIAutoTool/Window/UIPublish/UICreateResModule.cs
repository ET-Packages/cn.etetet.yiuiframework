﻿#if UNITY_EDITOR

using Sirenix.OdinInspector;

namespace YIUIFramework.Editor
{
    [HideReferenceObjectPicker]
    [HideLabel]
    public class UICreateResModule : BaseCreateModule
    {
        [InfoBox("UI所在的ET分包中的包名[cn.etetet.{0}]，可以没有 没有时生成在Unity/Assets/下")]
        [LabelText("新增模块指定分包名")]
        public string PackageName;

        [InfoBox("UI所对应的模块名 不是ET分包名称 全分包建议名称唯一")]
        [LabelText("新增模块名称")]
        public string Name;

        [GUIColor(0, 1, 0)]
        [Button("创建", 30)]
        private void Create()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            Create(Name, PackageName);
        }

        public static void Create(string createName, string packageName = "")
        {
            if (string.IsNullOrEmpty(createName))
            {
                UnityTipsHelper.ShowError("请设定 名称");
                return;
            }

            createName = NameUtility.ToFirstUpper(createName);

            string basePath = "";
            if (string.IsNullOrEmpty(packageName))
            {
                basePath = $"{YIUIConst.UIProjectResPath}/{createName}";
            }
            else
            {
                basePath = $"{string.Format(YIUIConst.UIProjectPackageResPath, packageName)}/{createName}";
            }

            var prefabsPath       = $"{basePath}/{YIUIConst.UIPrefabs}";
            var spritesPath       = $"{basePath}/{YIUIConst.UISprites}";
            var spritesAtlas1Path = $"{basePath}/{YIUIConst.UISprites}/{YIUIConst.UISpritesAtlas1}";
            var atlasIgnorePath   = $"{basePath}/{YIUIConst.UISprites}/{YIUIConst.UIAtlasIgnore}";
            var atlasPath         = $"{basePath}/{YIUIConst.UIAtlas}";
            var sourcePath        = $"{basePath}/{YIUIConst.UISource}";

            EditorHelper.CreateExistsDirectory(prefabsPath);
            EditorHelper.CreateExistsDirectory(spritesPath);
            EditorHelper.CreateExistsDirectory(spritesAtlas1Path);
            EditorHelper.CreateExistsDirectory(atlasIgnorePath);
            EditorHelper.CreateExistsDirectory(atlasPath);
            EditorHelper.CreateExistsDirectory(sourcePath);

            MenuItemYIUIPanel.CreateYIUIPanelByPath(sourcePath, createName);

            YIUIAutoTool.CloseWindowRefresh();
        }
    }
}
#endif