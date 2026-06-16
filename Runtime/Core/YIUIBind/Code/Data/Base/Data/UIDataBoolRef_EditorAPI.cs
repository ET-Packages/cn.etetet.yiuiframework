#if UNITY_EDITOR
namespace YIUIFramework
{
    internal partial class UIDataBoolRef
    {
        internal bool EditorConfigure(UICompareModeEnum compareMode, UIDataValue referenceData, bool reverse, out string error)
        {
            error = string.Empty;

            if (m_Data == null || m_Data.DataValue == null)
            {
                error = "BoolRef 当前 Data 为空";
                return false;
            }

            var dataType = m_Data.DataValue.UIBindDataType;
            if ((dataType == EUIBindDataType.Bool || dataType == EUIBindDataType.String) && compareMode != UICompareModeEnum.Equal)
            {
                error = $"{m_Data.Name} 的类型 {dataType} 只支持 Equal，非等于请使用 reverse";
                return false;
            }

            m_CompareMode = compareMode;
            m_Reverse = reverse;

            if (m_ReferenceData == null || m_ReferenceData.UIBindDataType != dataType)
            {
                m_ReferenceData = UIDataHelper.GetNewDataValue(dataType);
            }

            if (referenceData != null)
            {
                if (referenceData.UIBindDataType != dataType)
                {
                    error = $"{m_Data.Name} 参考值类型不一致: {referenceData.UIBindDataType} != {dataType}";
                    return false;
                }

                if (!m_ReferenceData.SetValueFrom(referenceData))
                {
                    error = $"{m_Data.Name} 参考值写入失败";
                    return false;
                }
            }

            Refresh(m_Data);
            return true;
        }
    }
}
#endif
