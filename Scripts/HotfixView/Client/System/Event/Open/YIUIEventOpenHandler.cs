namespace ET.Client
{
    // 分发UI打开之前监听
    [Event(SceneType.All)]
    public class YIUIEventPanelOpenBeforeHandler : AEvent<Scene, YIUIEventPanelOpenBefore>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelOpenBefore arg)
        {
            var yiuiEventComponent = scene.YIUIEvent();
            if (yiuiEventComponent == null) return;
            EntityRef<YIUIEventComponent> yiuiEventComponentRef = yiuiEventComponent;
            await yiuiEventComponent.Run(arg.UIComponentName, arg);
            yiuiEventComponent = yiuiEventComponentRef;
            await yiuiEventComponent.DynamicEvent(arg);
        }
    }

    // 分发UI打开之后监听
    [Event(SceneType.All)]
    public class YIUIEventPanelOpenAfterHandler : AEvent<Scene, YIUIEventPanelOpenAfter>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelOpenAfter arg)
        {
            var yiuiEventComponent = scene.YIUIEvent();
            if (yiuiEventComponent == null) return;
            EntityRef<YIUIEventComponent> yiuiEventComponentRef = yiuiEventComponent;
            await yiuiEventComponent.Run(arg.UIComponentName, arg);
            yiuiEventComponent = yiuiEventComponentRef;
            await yiuiEventComponent.DynamicEvent(arg);
        }
    }
}