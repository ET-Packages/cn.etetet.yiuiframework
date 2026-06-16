#if UNITY_EDITOR

namespace YIUIFramework
{
    public sealed partial class UIDataBindText
    {
        public override string GetYIUIVisionText()
        {
            return GetYIUIDataTextVisionText(nameof(UIDataBindText),
                "目标：Text=" + YIUIVisionTextHelper.FormatUnityObject(m_Text));
        }
    }
}

#endif
