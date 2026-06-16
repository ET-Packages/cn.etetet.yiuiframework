#if UNITY_EDITOR

namespace YIUIFramework
{
    public sealed partial class UIDataBindActiveComponent
    {
        public override string GetYIUIVisionText()
        {
            var extraParams = "target=" + YIUIVisionTextHelper.FormatUnityObject(m_Target) +
                              "，transition=" + m_TransitionMode +
                              "，transitionTime=" + YIUIVisionTextHelper.FormatFloat(m_TransitionTime);
            return GetYIUIDataBoolVisionText(nameof(UIDataBindActiveComponent), extraParams, "控制目标 Behaviour.enabled");
        }
    }

    public sealed partial class UIDataBindActiveComponents
    {
        public override string GetYIUIVisionText()
        {
            var extraParams = "targets=" + YIUIVisionTextHelper.FormatUnityObjectList(m_Targets) +
                              "，transition=" + m_TransitionMode +
                              "，transitionTime=" + YIUIVisionTextHelper.FormatFloat(m_TransitionTime);
            return GetYIUIDataBoolVisionText(nameof(UIDataBindActiveComponents), extraParams, "控制多个 Behaviour.enabled");
        }
    }

    public sealed partial class UIDataBindActiveGameObjects
    {
        public override string GetYIUIVisionText()
        {
            var extraParams = "targets=" + YIUIVisionTextHelper.FormatUnityObjectList(m_Targets) +
                              "，transition=" + m_TransitionMode +
                              "，transitionTime=" + YIUIVisionTextHelper.FormatFloat(m_TransitionTime);
            return GetYIUIDataBoolVisionText(nameof(UIDataBindActiveGameObjects), extraParams, "控制多个 GameObject active");
        }
    }
}

#endif
