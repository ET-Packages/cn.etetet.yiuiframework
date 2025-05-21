namespace ET.Client
{
    // 分发UI打开之前监听
    [Event(SceneType.All)]
    public class YIUIEventPanelOpenBeforeHandler : AEvent<Scene, YIUIEventPanelOpenBefore>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelOpenBefore arg)
        {
            if (YIUIEventComponent.Inst == null) return;
            await YIUIEventComponent.Inst.Run(arg.UIComponentName, arg);
            await scene.DynamicEvent(arg);
        }
    }

    // 分发UI打开之后监听
    [Event(SceneType.All)]
    public class YIUIEventPanelOpenAfterHandler : AEvent<Scene, YIUIEventPanelOpenAfter>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelOpenAfter arg)
        {
            if (YIUIEventComponent.Inst == null) return;
            await YIUIEventComponent.Inst.Run(arg.UIComponentName, arg);
            await scene.DynamicEvent(arg);
        }
    }
}