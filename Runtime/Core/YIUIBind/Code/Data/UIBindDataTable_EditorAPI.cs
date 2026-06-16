#if UNITY_EDITOR
namespace YIUIFramework
{
    public sealed partial class UIBindDataTable
    {
        public static string EditorNormalizeDataName(string dataName)
        {
            if (string.IsNullOrEmpty(dataName))
            {
                return dataName;
            }

            var normalizedName = dataName;
            if (!normalizedName.CheckFirstName(NameUtility.DataName))
            {
                normalizedName = $"{NameUtility.FirstName}{NameUtility.DataName}{normalizedName}";
            }

            return normalizedName.ChangeToBigName(NameUtility.DataName);
        }

        public UIData EditorGetOrAddData(string dataName, UIDataValue dataValue, out bool created, out string error)
        {
            created = false;
            error = string.Empty;

            if (string.IsNullOrEmpty(dataName))
            {
                error = "Data 名称不能为空";
                return null;
            }

            if (dataValue == null)
            {
                error = $"Data 值不能为空: {dataName}";
                return null;
            }

            var normalizedName = EditorNormalizeDataName(dataName);
            if (!m_DataDic.TryGetValue(normalizedName, out var uiData))
            {
                m_DataDic.TryGetValue(dataName, out uiData);
            }

            if (uiData != null)
            {
                if (uiData.DataValue == null)
                {
                    error = $"已存在 Data 但值为空: {normalizedName}";
                    return null;
                }

                if (uiData.DataValue.UIBindDataType != dataValue.UIBindDataType)
                {
                    error = $"Data 已存在但类型不一致: {normalizedName}，已存在={uiData.DataValue.UIBindDataType}，请求={dataValue.UIBindDataType}";
                    return null;
                }

                uiData.SetValueFrom(dataValue);
                return uiData;
            }

            var data = new UIData(normalizedName, dataValue);
            m_DataDic.Add(data.Name, data);
            created = true;
            m_Initialized = false;
            OnValidate();
            return data;
        }

        public bool EditorTryRemoveData(string dataName, bool forceRemoveBinds, out YIUIBindEditorRemoveResult result, out string error)
        {
            result = new YIUIBindEditorRemoveResult();
            error = string.Empty;
            result.DeclarationKind = "Data";
            result.DeclarationName = EditorNormalizeDataName(dataName);

            if (string.IsNullOrEmpty(dataName))
            {
                error = "Data 名称不能为空";
                return false;
            }

            m_Initialized = false;
            OnValidate();

            if (m_DataDic == null)
            {
                error = "DataTable 未初始化";
                return false;
            }

            var normalizedName = EditorNormalizeDataName(dataName);
            if (!m_DataDic.TryGetValue(normalizedName, out var uiData))
            {
                if (!m_DataDic.TryGetValue(dataName, out uiData))
                {
                    error = $"Data 不存在: {normalizedName}";
                    return false;
                }

                normalizedName = dataName;
            }

            result.BindCount = uiData.GetBindCount();
            uiData.EditorCollectBindTargets(result);

            if (result.BindCount > 0 && !forceRemoveBinds)
            {
                result.RequiredForceRemoveBinds = true;
                error = $"Data 已绑定 {result.BindCount} 个目标，删除需要 forceRemoveBinds=true: {normalizedName}";
                return false;
            }

            result.RemovedBindCount = result.BindCount;
            uiData.OnDataRemoveCallBack();
            m_DataDic.Remove(normalizedName);
            result.Removed = true;

            m_Initialized = false;
            OnValidate();
            return true;
        }
    }
}
#endif
