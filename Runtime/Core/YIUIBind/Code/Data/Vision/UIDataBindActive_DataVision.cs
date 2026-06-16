#if UNITY_EDITOR

namespace YIUIFramework
{
    public sealed partial class UIDataBindActive
    {
        public override string GetYIUIVisionText()
        {
            var extraParams = "transition=" + m_TransitionMode +
                              "，transitionTime=" + YIUIVisionTextHelper.FormatFloat(m_TransitionTime);
            return GetYIUIDataBoolVisionText(nameof(UIDataBindActive), extraParams, "控制当前 GameObject active");
        }
    }
}

#endif
