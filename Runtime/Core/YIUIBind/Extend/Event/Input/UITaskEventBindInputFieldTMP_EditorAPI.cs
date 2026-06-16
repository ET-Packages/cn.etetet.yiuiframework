#if UNITY_EDITOR && TextMeshPro
using TMPro;

namespace YIUIFramework
{
    public partial class UITaskEventBindInputFieldTMP
    {
        public bool EditorConfigureTaskInputFieldTMP(UIBindEventTable eventTable, UIEventBase uiEvent, out string error)
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

            if (m_InputField == null)
            {
                m_InputField = GetComponent<TMP_InputField>();
            }

            OnValidate();
            return true;
        }
    }
}
#endif
