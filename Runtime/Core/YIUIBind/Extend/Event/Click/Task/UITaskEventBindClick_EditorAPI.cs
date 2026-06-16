#if UNITY_EDITOR
using UnityEngine.UI;

namespace YIUIFramework
{
    public partial class UITaskEventBindClick
    {
        public bool EditorConfigureTaskClick(UIBindEventTable eventTable, UIEventBase uiEvent, bool skipWhenDrag, bool banLayerOption, out string error)
        {
            error = string.Empty;

            if (eventTable == null)
            {
                error = $"{name} EventTable 不能为空";
                return false;
            }

            if (uiEvent == null)
            {
                error = $"{name} UIEvent 不能为空";
                return false;
            }

            if (!EditorAddBind(eventTable, uiEvent))
            {
                error = $"{name} 绑定事件失败: {uiEvent.EventName}";
                return false;
            }

            m_SkipWhenDrag = skipWhenDrag;
            m_BanLayerOption = banLayerOption;
            if (m_Selectable == null)
            {
                m_Selectable = GetComponent<Selectable>();
            }

            OnValidate();
            return true;
        }
    }
}
#endif
