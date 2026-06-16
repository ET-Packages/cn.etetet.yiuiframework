#if UNITY_EDITOR
namespace YIUIFramework
{
    public abstract partial class UIEventBind
    {
        public void EditorPrepareRemove()
        {
            UnbindEvent();
            m_Binded = false;
        }
    }
}
#endif
