namespace ET.Client
{
    // 分发UI关闭之前监听
    [Event(SceneType.All)]
    public class YIUIEventPanelCloseBeforeHandler : AEvent<Scene, YIUIEventPanelCloseBefore>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelCloseBefore arg)
        {
            var yiuiEventComponent = scene.YIUIEvent();
            if (yiuiEventComponent == null) return;
            EntityRef<YIUIEventComponent> yiuiEventComponentRef = yiuiEventComponent;
            await yiuiEventComponent.Run(arg.UIComponentName, arg);
            yiuiEventComponent = yiuiEventComponentRef;
            await yiuiEventComponent.DynamicEvent(arg);
        }
    }

    // 分发UI关闭之后监听
    [Event(SceneType.All)]
    public class YIUIEventPanelCloseAfterHandler : AEvent<Scene, YIUIEventPanelCloseAfter>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelCloseAfter arg)
        {
            var yiuiEventComponent = scene.YIUIEvent();
            if (yiuiEventComponent == null) return;
            EntityRef<YIUIEventComponent> yiuiEventComponentRef = yiuiEventComponent;
            await yiuiEventComponent.Run(arg.UIComponentName, arg);
            yiuiEventComponent = yiuiEventComponentRef;
            await yiuiEventComponent.DynamicEvent(arg);
        }
    }

    // 分发UI被摧毁
    [Event(SceneType.All)]
    public class YIUIEventPanelDestroyHandler : AEvent<Scene, YIUIEventPanelDestroy>
    {
        protected override async ETTask Run(Scene scene, YIUIEventPanelDestroy arg)
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