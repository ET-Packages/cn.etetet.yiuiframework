#if UNITY_EDITOR
namespace YIUIFramework
{
    internal partial class UIDataChangeRef
    {
        internal bool EditorConfigure(UIDataValue changeData, out string error)
        {
            error = string.Empty;

            if (m_Data == null || m_Data.DataValue == null)
            {
                error = "ChangeRef 当前 Data 为空";
                return false;
            }

            var dataType = m_Data.DataValue.UIBindDataType;
            if (m_ChangeData == null || m_ChangeData.UIBindDataType != dataType)
            {
                m_ChangeData = UIDataHelper.GetNewDataValue(dataType);
            }

            if (changeData != null)
            {
                if (changeData.UIBindDataType != dataType)
                {
                    error = $"{m_Data.Name} 改值类型不一致: {changeData.UIBindDataType} != {dataType}";
                    return false;
                }

                if (!m_ChangeData.SetValueFrom(changeData))
                {
                    error = $"{m_Data.Name} 改值写入失败";
                    return false;
                }
            }

            Refresh(m_Data);
            return true;
        }
    }
}
#endif
