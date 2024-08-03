//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace YIUIFramework
{
    /// 一个项目不可能随时换项目路径 这里就是强制设置的只可读
    /// 初始化项目的时候手动改这个一次就可以了
    /// 建议什么都不要改 改了自己查问题
    /// <summary>
    /// YIUI常量
    /// </summary>
    public static partial class YIUIConst
    {
        [LabelText("YIUI根目录名称")]
        public const string UIProjectName = "YIUI"; //不要修改

        [LabelText("YIUI项目命名空间")]
        public const string UINamespace = "ET.Client"; //所有生成文件的命名空间

        [LabelText("YIUI项目编辑器资源")]
        public const string UIProjectEditorPath = "Assets/Editor/" + UIProjectName; //编辑器才会用到的资源

        [LabelText("YIUI项目指定包编辑器资源")]
        public const string UIProjectPackageEditorPath = "Assets/../Packages/cn.etetet.{0}/" + UIProjectEditorPath; //指定包的编辑器才会用到的资源

        [LabelText("YIUI项目资源")]
        public const string UIProjectResPath = "Assets/GameRes/" + UIProjectName; //玩家的预设/图片等资源存放的地方

        [LabelText("YIUI项目指定包资源")]
        public const string UIProjectPackageResPath = "Assets/../Packages/cn.etetet.{0}/" + UIProjectResPath; //指定包的资源存放的地方

        [LabelText("YIUI项目生成包名")]
        public const string UIETCreatePackageName = "yiui"; //对应指定代码生成到指定的包中

        [LabelText("YIUI项目生成默认包")]
        public const string UIETCreatePackagePath = "Assets/../Packages/cn.etetet.yiui";

        [LabelText("YIUI项目脚本")]
        public const string UIETComponentGenPath = "Assets/../Packages/cn.etetet.{0}/Scripts/ModelView/Client/YIUIGen"; //自动生成的代码会覆盖不可修改

        [LabelText("YIUI项目ET组件")]
        public const string UIETComponentPath = "Assets/../Packages/cn.etetet.{0}/Scripts/ModelView/Client/YIUIComponent"; //玩家可编写的核心代码部分 ET系统

        [LabelText("YIUI项目自定义脚本")]
        public const string UIETSystemGenPath = "Assets/../Packages/cn.etetet.{0}/Scripts/HotfixView/Client/YIUIGen"; //自动生成的代码会覆盖不可修改

        [LabelText("YIUI项目ET系统")]
        public const string UIETSystemPath = "Assets/../Packages/cn.etetet.{0}/Scripts/HotfixView/Client/YIUISystem"; //玩家可编写的核心代码部分 ET系统

        [LabelText("YIUI框架所处位置")]
        public const string UIFrameworkPath = "Assets/../Packages/cn.etetet.yiuiframework";

        [LabelText("YIUI项目代码模板")]
        public const string UITemplatePath = "Assets/../Packages/cn.etetet.yiuiframework/Editor/YIUIAutoTool/Template";

        [LabelText("YIUI项目Root预制")]
        public const string UIRootPrefabPath = "Packages/cn.etetet.yiuiframework/Editor/UIRootPrefab/" + UIRootName + ".prefab";

        public const string UIPackages               = "Packages";
        public const string UIETPackagesFormat       = "cn.etetet.";
        public const string UIRootName               = "YIUIRoot";
        public const string UILayerRootName          = "YIUILayerRoot";
        public const string UIRootPkgName            = "Common";
        public const string UIPanelName              = "Panel";
        public const string UIViewName               = "View";
        public const string UIParentName             = "Parent";
        public const string UIPrefabs                = "Prefabs";
        public const string UIPrefabsCN              = "预制";
        public const string UISprites                = "Sprites";
        public const string UISpritesCN              = "精灵";
        public const string UIAtlas                  = "Atlas";
        public const string UIAtlasCN                = "图集";
        public const string UISource                 = "Source";
        public const string UISourceCN               = "源文件";
        public const string UIAtlasIgnore            = "AtlasIgnore"; //图集忽略文件夹名称
        public const string UISpritesAtlas1          = "Atlas1";      //图集1 不需要华丽的取名 每个包内的自定义图集就按顺序就好 当然你也可以自定义其他
        public const string UIAllViewParentName      = "AllViewParent";
        public const string UIAllPopupViewParentName = "AllPopupViewParent";
        public const string UIYIUIPanelSourceName    = UIProjectName + UIPanelName + UISource;
        public const string UIPanelSourceName        = UIPanelName + UISource;
        public const string UIYIUIViewName           = UIProjectName + UIViewName;
        public const string UIViewParentName         = UIViewName + UIParentName;
        public const string UIYIUIViewParentName     = UIProjectName + UIViewName + UIParentName;
    }
}