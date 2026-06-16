#if UNITY_EDITOR
namespace YIUIFramework
{
    public partial class UITaskEventBindClickString
    {
        public bool EditorConfigureTaskClickString(UIBindEventTable eventTable, UIEventBase uiEvent, string extraParam, bool skipWhenDrag, bool banLayerOption, out string error)
        {
            m_ExtraParam = extraParam ?? string.Empty;
            return EditorConfigureTaskClick(eventTable, uiEvent, skipWhenDrag, banLayerOption, out error);
        }
    }
}
#endif
