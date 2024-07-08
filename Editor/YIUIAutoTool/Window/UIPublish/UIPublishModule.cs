#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using ET;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace YIUIFramework.Editor
{
    public class UIPublishModule : BaseYIUIToolModule
    {
        internal const string m_PublishName        = "发布";
        internal const string m_PublishCommonName  = "通用";
        internal const string m_PublishPackageName = "ET包";

        [FolderPath]
        [LabelText("所有模块资源路径")]
        [ReadOnly]
        [ShowInInspector]
        private string m_AllPkgPath = YIUIConst.UIProjectResPath;

        //所有ET模块资源路径
        private const string m_AllETPkgPath = "Packages/";

        [BoxGroup("创建模块", centerLabel: true)]
        [ShowInInspector]
        internal UICreateResModule CreateResModule = new UICreateResModule();

        //所有模块
        private List<UIPublishPackageModule> m_AllUIPublishPackageModule = new List<UIPublishPackageModule>();

        private void AddAllPkg()
        {
            EditorHelper.CreateExistsDirectory(m_AllPkgPath);
            var allInfo = new Dictionary<string, List<string>>();
            try
            {
                foreach (var yiuiPkg in Directory.GetDirectories(EditorHelper.GetProjPath(m_AllPkgPath)))
                {
                    if (!allInfo.ContainsKey(m_AllPkgPath))
                    {
                        allInfo.Add(m_AllPkgPath, new List<string>());
                    }

                    var list = allInfo[m_AllPkgPath];
                    list.Add(yiuiPkg);
                }

                foreach (var packagePath in Directory.GetDirectories(EditorHelper.GetProjPath(m_AllETPkgPath)))
                {
                    var packageRes = $"{packagePath}/{m_AllPkgPath}";
                    if (Directory.Exists(packageRes))
                    {
                        var etPackagesName = UIOperationHelper.GetETPackagesName(packageRes, false);
                        if (string.IsNullOrEmpty(etPackagesName))
                        {
                            Debug.LogError($"错误这里没有找到ET包名 请检查 {packageRes}");
                            continue;
                        }

                        var resPath = $"{m_AllETPkgPath}{YIUIConst.UIETPackagesFormat}{etPackagesName}/{m_AllPkgPath}";
                        if (!allInfo.ContainsKey(resPath))
                        {
                            allInfo.Add(resPath, new List<string>());
                        }

                        var list = allInfo[resPath];
                        foreach (var yiuiPkg in Directory.GetDirectories(packageRes))
                        {
                            list.Add(yiuiPkg);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"获取所有模块错误 请检查 err={e.Message}{e.StackTrace}");
                return;
            }

            var commonPublish = $"{m_PublishName}/{m_PublishCommonName}";
            Tree.AddMenuItemAtPath(m_PublishName, new OdinMenuItem(Tree, m_PublishCommonName, null)).AddIcon(EditorIcons.Folder);
            var packagePublish = $"{m_PublishName}/{m_PublishPackageName}";
            Tree.AddMenuItemAtPath(m_PublishName, new OdinMenuItem(Tree, m_PublishPackageName, null)).AddIcon(EditorIcons.Folder);

            foreach ((var resPath, var listInfo) in allInfo)
            {
                var etPackagesName = UIOperationHelper.GetETPackagesName(resPath, false);

                foreach (var folder in listInfo)
                {
                    var pkgName   = Path.GetFileName(folder);
                    var upperName = NameUtility.ToFirstUpper(pkgName);
                    if (upperName != pkgName)
                    {
                        Debug.LogError($"这是一个非法的模块[ {pkgName} ]请使用统一方法创建模块 或者满足指定要求");
                        continue;
                    }

                    var newUIPublishPackageModule = new UIPublishPackageModule(this, resPath, pkgName);

                    var showPkgName = string.IsNullOrEmpty(etPackagesName) ? $"{pkgName}" : $"[{etPackagesName}]{pkgName}";

                    var publishName = string.IsNullOrEmpty(etPackagesName) ? $"{commonPublish}" : $"{packagePublish}";

                    //0 模块
                    Tree.AddMenuItemAtPath(publishName,
                                           new OdinMenuItem(Tree, showPkgName, newUIPublishPackageModule)).AddIcon(EditorIcons.Folder);

                    //1 图集
                    Tree.AddAllAssetsAtPath($"{publishName}/{showPkgName}/{YIUIConst.UIAtlasCN}",
                                            $"{resPath}/{pkgName}/{YIUIConst.UIAtlas}", typeof(SpriteAtlas), true, false);

                    //2 预制体
                    Tree.AddAllAssetsAtPath($"{publishName}/{showPkgName}/{YIUIConst.UIPrefabsCN}",
                                            $"{resPath}/{pkgName}/{YIUIConst.UIPrefabs}", typeof(UIBindCDETable), true, false);

                    //3 源文件
                    Tree.AddAllAssetsAtPath($"{publishName}/{showPkgName}/{YIUIConst.UISourceCN}",
                                            $"{resPath}/{pkgName}/{YIUIConst.UISource}", typeof(UIBindCDETable), true, false);

                    //4 精灵
                    Tree.AddAllAssetImporterAtPath($"{publishName}/{showPkgName}/{YIUIConst.UISpritesCN}",
                                                   $"{resPath}/{pkgName}/{YIUIConst.UISprites}", typeof(TextureImporter), true, false);

                    m_AllUIPublishPackageModule.Add(newUIPublishPackageModule);
                }
            }
        }

        [GUIColor(0f, 1f, 1f)]
        [Button("UI自动生成绑定替代反射代码", 50)]
        [PropertyOrder(-9999)]
        public static void CreateUIBindProvider()
        {
            new CreateUIBindProviderModule().Create();
        }

        [GUIColor(0.4f, 0.8f, 1)]
        [Button("全部发布", 50)]
        [PropertyOrder(-99)]
        public void PublishAll()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            foreach (var module in m_AllUIPublishPackageModule)
            {
                module.PublishCurrent(false); //不要默认重置所有图集设置 有的图集真的会有独立设置
            }

            UnityTipsHelper.CallBackOk("YIUI全部 发布完毕", YIUIAutoTool.CloseWindowRefresh);
        }

        public override void Initialize()
        {
            AddAllPkg();
            CreateResModule?.Initialize();
        }

        public override void OnDestroy()
        {
            CreateResModule?.OnDestroy();
        }
    }
}
#endif
