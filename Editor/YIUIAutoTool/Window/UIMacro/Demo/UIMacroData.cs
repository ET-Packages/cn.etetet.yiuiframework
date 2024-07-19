#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;

namespace YIUIFramework.Editor
{
    [Flags]
    [LabelText("YIUI")]
    [YIUIEnumUnityMacro]
    public enum EYIUIMacroType : long
    {
        [LabelText("所有")]
        ALL = -1,

        [LabelText("无")]
        NONE = 0,

        [LabelText("模拟非编辑器状态")]
        YIUIMACRO_SIMULATE_NONEEDITOR = 1,

        [LabelText("UIBing 初始化")]
        YIUIMACRO_BIND_INITIALIZE = 1 << 1,

        [LabelText("界面开关")]
        YIUIMACRO_PANEL_OPENCLOSE = 1 << 2,

        [LabelText("运行时调试UI")]
        YIUIMACRO_BIND_RUNTIME_EDITOR = 1 << 3,

        [LabelText("红点堆栈收集")]
        YIUIMACRO_REDDOT_STACK = 1 << 4,

        [LabelText("绑定使用反射")]
        YIUIMACRO_BIND_REFLECTION = 1 << 5, //打包后你也想用反射加载绑定信息的 打开这个

        [LabelText("绑定使用反射 编辑器时使用ET DLL")]
        YIUIMACRO_BIND_BYETDLL = 1 << 6, //使用反射时 有2种方式 1.使用DLL(ET工程编译出来的) 2.使用Unity的程序集(Unity编译的)

        [LabelText("绑定使用反射 运行时使用AppDomain程序集 全部")]
        YIUIMACRO_BIND_BYUNITYDLL_ALL = 1 << 7, //默认只找ET.ModelView 开了这个会找全部的程序集
    }

    [Flags]
    [LabelText("ET")]
    [YIUIEnumUnityMacro]
    public enum EYIUIETMacroType : long
    {
        [LabelText("所有")]
        ALL = -1,

        [LabelText("无")]
        NONE = 0,

        [LabelText("已初始化")]
        INITED = 1,

        [LabelText("可视化")]
        ENABLE_VIEW = 1 << 1,
    }
}
#endif