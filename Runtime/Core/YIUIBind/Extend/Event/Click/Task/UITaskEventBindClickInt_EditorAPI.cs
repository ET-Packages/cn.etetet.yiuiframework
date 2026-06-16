#if UNITY_EDITOR
namespace YIUIFramework
{
    public partial class UITaskEventBindClickInt
    {
        public bool EditorConfigureTaskClickInt(UIBindEventTable eventTable, UIEventBase uiEvent, int extraParam, bool skipWhenDrag, bool banLayerOption, out string error)
        {
            m_ExtraParam = extraParam;
            return EditorConfigureTaskClick(eventTable, uiEvent, skipWhenDrag, banLayerOption, out error);
        }
    }
}
#endif
