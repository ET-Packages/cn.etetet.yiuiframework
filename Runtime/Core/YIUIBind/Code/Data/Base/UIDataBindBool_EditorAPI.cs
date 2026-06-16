#if UNITY_EDITOR
using System.Collections.Generic;

namespace YIUIFramework
{
    public sealed class UIDataBindBoolEditorCondition
    {
        public UIData Data;
        public UICompareModeEnum CompareMode;
        public UIDataValue ReferenceData;
        public bool Reverse;
    }

    public abstract partial class UIDataBindBool
    {
        public bool EditorConfigureBoolConditions(IReadOnlyList<UIDataBindBoolEditorCondition> conditions, UIBooleanLogic logic, out string error)
        {
            error = string.Empty;

            if (conditions == null || conditions.Count <= 0)
            {
                error = $"{name} Active 条件不能为空";
                return false;
            }

            var datas = new List<UIData>(conditions.Count);
            foreach (var condition in conditions)
            {
                if (condition == null || condition.Data == null)
                {
                    error = $"{name} Active 条件存在空 Data";
                    return false;
                }

                datas.Add(condition.Data);
            }

            if (!EditorSetDataSelects(datas, out error))
            {
                return false;
            }

            m_BooleanLogic = logic;

            foreach (var condition in conditions)
            {
                var dataRef = FindEditorBoolRef(condition.Data);
                if (dataRef == null)
                {
                    error = $"{name} 未找到 BoolRef: {condition.Data.Name}";
                    return false;
                }

                if (!dataRef.EditorConfigure(condition.CompareMode, condition.ReferenceData, condition.Reverse, out error))
                {
                    return false;
                }
            }

            base.OnValidate();
            return true;
        }

        private UIDataBoolRef FindEditorBoolRef(UIData data)
        {
            if (data == null)
            {
                return null;
            }

            foreach (var dataRef in m_Datas)
            {
                if (dataRef != null && dataRef.Data != null && dataRef.Data.DataGuid == data.DataGuid)
                {
                    return dataRef;
                }
            }

            return null;
        }
    }
}
#endif
