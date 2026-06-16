#if UNITY_EDITOR

namespace YIUIFramework
{
    internal partial class UIDataChangeRef
    {
        internal string GetYIUIVisionText()
        {
            return "- " + YIUIVisionTextHelper.FormatDataInline(m_Data, false) +
                   "，点击后改为：type=" + GetChangeDataTypeText() +
                   "，value=" + YIUIVisionTextHelper.FormatDataValue(m_ChangeData);
        }

        private string GetChangeDataTypeText()
        {
            return m_ChangeData != null ? m_ChangeData.UIBindDataType.ToString() : "null";
        }
    }
}

#endif
