using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    [Serializable]
    [HideReferenceObjectPicker]
    [HideLabel]
    public class YIUIConstAsset : ScriptableObject
    {
        [LabelText("YIUI根目录名称")]
        public string UIProjectName = "YIUI"; //不要修改

        [LabelText("YIUI项目命名空间")]
        public string UINamespace = "ET.Client"; //所有生成文件的命名空间

        [LabelText("YIUI项目编辑器资源")]
        public string UIProjectEditorPath = "Assets/Editor/YIUI"; //编辑器才会用到的资源

        [LabelText("YIUI项目指定包编辑器资源")]
        public string UIProjectPackageEditorPath = "Assets/../Packages/cn.etetet.{0}/Assets/Editor/YIUI"; //指定包的编辑器才会用到的资源

        [LabelText("YIUI项目资源")]
        public string UIProjectResPath = "Assets/GameRes/YIUI"; //玩家的预设/图片等资源存放的地方

        [LabelText("YIUI项目指定包资源")]
        public string UIProjectPackageResPath = "Assets/../Packages/cn.etetet.{0}/Assets/GameRes/YIUI"; //指定包的资源存放的地方

        [LabelText("YIUI项目生成包名")]
        public string UIETCreatePackageName = "yiui"; //对应指定代码生成到指定的包中

        [LabelText("YIUI项目生成默认包")]
        public string UIETCreatePackagePath = "Assets/../Packages/cn.etetet.yiui";

        [LabelText("YIUI项目脚本")]
        public string UIETComponentGenPath = "Assets/../Packages/cn.etetet.{0}/Scripts/ModelView/Client/YIUIGen"; //自动生成的代码会覆盖不可修改

        [LabelText("YIUI项目ET组件")]
        public string UIETComponentPath = "Assets/../Packages/cn.etetet.{0}/Scripts/ModelView/Client/YIUIComponent"; //玩家可编写的核心代码部分 ET系统

        [LabelText("YIUI项目自定义脚本")]
        public string UIETSystemGenPath = "Assets/../Packages/cn.etetet.{0}/Scripts/HotfixView/Client/YIUIGen"; //自动生成的代码会覆盖不可修改

        [LabelText("YIUI项目ET系统")]
        public string UIETSystemPath = "Assets/../Packages/cn.etetet.{0}/Scripts/HotfixView/Client/YIUISystem"; //玩家可编写的核心代码部分 ET系统

        [LabelText("YIUI框架所处位置")]
        public string UIFrameworkPath = "Assets/../Packages/cn.etetet.yiuiframework";

        [LabelText("YIUI项目代码模板")]
        public string UITemplatePath = "Assets/../Packages/cn.etetet.yiuiframework/Editor/YIUIAutoTool/Template";

        [LabelText("YIUI项目Root预制")]
        public string UIRootPrefabPath = "Packages/cn.etetet.yiuiframework/Editor/UIRootPrefab/YIUIRoot.prefab";

        public string UIPackages = "Packages";

        public string UIETPackagesFormat = "cn.etetet.";

        public string UIRootName = "YIUIRoot";

        public string UILayerRootName = "YIUILayerRoot";

        public string UIRootPkgName = "Common";

        public string UIPanelName = "Panel";

        public string UIViewName = "View";

        public string UIParentName = "Parent";

        public string UIPrefabs = "Prefabs";

        public string UIPrefabsCN = "预制";

        public string UISprites = "Sprites";

        public string UISpritesCN = "精灵";

        public string UIAtlas = "Atlas";

        public string UIAtlasCN = "图集";

        public string UISource = "Source";

        public string UISourceCN = "源文件";

        public string UIAtlasIgnore = "AtlasIgnore"; //图集忽略文件夹名称

        public string UISpritesAtlas1 = "Atlas1"; //图集1 不需要华丽的取名 每个包内的自定义图集就按顺序就好 当然你也可以自定义其他

        public string UIAllViewParentName = "AllViewParent";

        public string UIAllPopupViewParentName = "AllPopupViewParent";

        public string UIYIUIPanelSourceName = "YIUIPanelSource";

        public string UIPanelSourceName = "PanelSource";

        public string UIYIUIViewName = "YIUIView";

        public string UIViewParentName = "ViewParent";

        public string UIYIUIViewParentName = "YIUIViewParent";

        #region Root

        [LabelText("UI宽度")]
        public int DesignScreenWidth = 1920;

        [LabelText("UI高度")]
        public int DesignScreenHeight = 1080;

        [LabelText("UI宽度(float)")]
        public float DesignScreenWidth_F = 1920f;

        [LabelText("UI高度(float)")]
        public float DesignScreenHeight_F = 1080f;

        [LabelText("根节点偏移")]
        public int RootPosOffset = 1000;

        [LabelText("每个层级的距离")]
        public int LayerDistance = 1000;

        #endregion
    }
}