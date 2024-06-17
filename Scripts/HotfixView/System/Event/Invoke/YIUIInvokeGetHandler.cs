using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUIRootHandler : AInvokeHandler<YIUIInvokeGetUIRoot, GameObject>
    {
        public override GameObject Handle(YIUIInvokeGetUIRoot args)
        {
            return YIUIMgrComponent.Inst?.UIRoot;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUICanvasRootHandler : AInvokeHandler<YIUIInvokeGetUICanvasRoot, GameObject>
    {
        public override GameObject Handle(YIUIInvokeGetUICanvasRoot args)
        {
            return YIUIMgrComponent.Inst?.UICanvasRoot;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUILayerRootHandler : AInvokeHandler<YIUIInvokeGetUILayerRoot, RectTransform>
    {
        public override RectTransform Handle(YIUIInvokeGetUILayerRoot args)
        {
            return YIUIMgrComponent.Inst?.UILayerRoot;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUICameraHandler : AInvokeHandler<YIUIInvokeGetUICamera, Camera>
    {
        public override Camera Handle(YIUIInvokeGetUICamera args)
        {
            return YIUIMgrComponent.Inst?.UICamera;
        }
    }

    [Invoke(EYIUIInvokeType.Sync)]
    public class YIUIInvokeGetUICanvasHandler : AInvokeHandler<YIUIInvokeGetUICanvas, Canvas>
    {
        public override Canvas Handle(YIUIInvokeGetUICanvas args)
        {
            return YIUIMgrComponent.Inst?.UICanvas;
        }
    }
}
