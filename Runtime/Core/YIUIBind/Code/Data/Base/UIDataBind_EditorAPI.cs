#if UNITY_EDITOR
namespace YIUIFramework
{
    public abstract partial class UIDataBind
    {
        public void EditorPrepareRemove()
        {
            UnBindData();
            m_Binded = false;
        }
    }
}
#endif
