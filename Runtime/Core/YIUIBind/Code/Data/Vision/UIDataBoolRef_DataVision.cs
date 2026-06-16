#if UNITY_EDITOR

namespace YIUIFramework
{
    internal partial class UIDataBoolRef
    {
        internal string GetYIUIVisionText()
        {
            var dataText = YIUIVisionTextHelper.FormatDataInline(m_Data, false);
            var conditionText = GetConditionText();

            return "- " + dataText +
                   "，条件：" + conditionText +
                   "，取反=" + YIUIVisionTextHelper.FormatBool(m_Reverse);
        }

        private string GetConditionText()
        {
            if (m_Data == null || m_Data.DataValue == null)
            {
                return "当前值无法比较";
            }

            var referenceText = YIUIVisionTextHelper.FormatDataValue(m_ReferenceData);
            return "当前值 " + YIUIVisionTextHelper.FormatCompareMode(m_CompareMode) + " " + referenceText;
        }
    }
}

#endif
