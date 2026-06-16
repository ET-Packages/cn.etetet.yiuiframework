#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine.UI;

namespace YIUIFramework
{
    public sealed class UIDataBindChangeEditorValue
    {
        public UIData Data;
        public UIDataValue ChangeData;
    }

    public partial class UIDataBindChange
    {
        public bool EditorConfigureChanges(IReadOnlyList<UIDataBindChangeEditorValue> changes, bool invokeClick, bool skipWhenDrag, out string error)
        {
            error = string.Empty;

            if (changes == null || changes.Count <= 0)
            {
                error = $"{name} Change 配置不能为空";
                return false;
            }

            var datas = new List<UIData>(changes.Count);
            foreach (var change in changes)
            {
                if (change == null || change.Data == null)
                {
                    error = $"{name} Change 配置存在空 Data";
                    return false;
                }

                datas.Add(change.Data);
            }

            if (!EditorSetDataSelects(datas, out error))
            {
                return false;
            }

            m_InvokeClick = invokeClick;
            m_SkipWhenDrag = skipWhenDrag;
            if (m_Selectable == null)
            {
                m_Selectable = GetComponent<Selectable>();
            }

            foreach (var change in changes)
            {
                var changeRef = FindEditorChangeRef(change.Data);
                if (changeRef == null)
                {
                    error = $"{name} 未找到 ChangeRef: {change.Data.Name}";
                    return false;
                }

                if (!changeRef.EditorConfigure(change.ChangeData, out error))
                {
                    return false;
                }
            }

            base.OnValidate();
            return true;
        }

        private UIDataChangeRef FindEditorChangeRef(UIData data)
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
