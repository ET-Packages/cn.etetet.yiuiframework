#if UNITY_EDITOR && TextMeshPro

namespace YIUIFramework
{
    public sealed partial class UIDataBindTextTMP
    {
        public override string GetYIUIVisionText()
        {
            return GetYIUIDataTextVisionText(nameof(UIDataBindTextTMP),
                "目标：TextMeshProUGUI=" + YIUIVisionTextHelper.FormatUnityObject(m_Text));
        }
    }
}

#endif
