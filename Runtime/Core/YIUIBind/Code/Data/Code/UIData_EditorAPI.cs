#if UNITY_EDITOR
namespace YIUIFramework
{
    public sealed partial class UIData
    {
        public void EditorCollectBindTargets(YIUIBindEditorRemoveResult result)
        {
            EditorRemoveDestroyedBinds();

            if (result == null || m_Binds == null)
            {
                return;
            }

            for (var i = 0; i < m_Binds.Count; i++)
            {
                var bind = m_Binds[i];
                if (bind == null)
                {
                    continue;
                }

                result.BindTargets.Add(YIUIBindEditorRemoveUtility.GetComponentPath(bind));
            }
        }
    }
}
#endif
