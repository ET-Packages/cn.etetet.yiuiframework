using System;
using UnityEngine;
using YIUIFramework;
using UnityObject = UnityEngine.Object;

//文档 https://lib9kmxvq7k.feishu.cn/wiki/GQb8wtMiYibrxaklR9ic6RHUnUe
namespace ET.Client
{
    //对应ROOT打开对应Panel
    public struct YIUIInvokeRootOpenPanel
    {
        public YIUIRootComponent Root;
        public string            PanelName;
    }

    /// <summary>
    /// 关闭一个窗口 (被直接摧毁的)
    /// 注意被直接摧毁的Panel 无法触发动画 回退等异步操作
    /// </summary>
    public struct YIUIInvokeDestroyPanel
    {
        public string PanelName;
    }

    //移除注册中的UI
    public struct YIUIInvokeRemoveUIReset
    {
        public string PanelName;
    }

    //home跳转
    public struct YIUIInvokeHomePanel
    {
        public string            PanelName;
        public bool              Tween;
        public YIUIRootComponent ForceHome;
    }

    /// <summary>
    /// 关闭指定Panel
    /// </summary>
    public struct YIUIInvokeClosePanel
    {
        public string PanelName;
        public bool   Tween;
        public bool   IgnoreElse;
    }

    //由 当前panel的子集 任意view发起的关闭
    //需要符合特定结构要求 只要是标准的panel中的view 都行
    public struct YIUIInvokeViewClosePanel
    {
        public YIUIViewComponent ViewComponent;
        public bool              Tween;
        public bool              IgnoreElse;
    }

    // 加载任意实例化 to Type
    public struct YIUIInvokeLoadInstantiate
    {
        public Type      LoadType;
        public string    PkgName;
        public string    ResName;
        public Entity    ParentEntity;
        public Transform ParentTransform;
    }

    //YIUI加载组件初始化 由具体的资源管理器实现
    public struct YIUIInvokeLoadInitialize
    {
        public EntityRef<YIUILoadComponent> LoadComponent;
    }
}