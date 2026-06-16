#if UNITY_EDITOR
namespace YIUIFramework
{
    public abstract partial class UIDataBindTextBase
    {
        public void EditorConfigureText(string format, bool changeEnabled, bool numberPrecision, string numberPrecisionStr)
        {
            m_Format = format ?? string.Empty;
            m_ChangeEnabled = changeEnabled;
            m_NumberPrecision = numberPrecision;
            m_NumberPrecisionStr = string.IsNullOrEmpty(numberPrecisionStr) ? "F1" : numberPrecisionStr;
            base.OnValidate();
        }
    }
}
#endif
