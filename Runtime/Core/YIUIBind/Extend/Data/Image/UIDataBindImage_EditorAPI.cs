#if UNITY_EDITOR
using UnityEngine.UI;

namespace YIUIFramework
{
    public sealed partial class UIDataBindImage
    {
        public void EditorConfigureImage(bool setNativeSize, bool changeEnabled)
        {
            m_SetNativeSize = setNativeSize;
            m_ChangeEnabled = changeEnabled;
            if (m_Image == null)
            {
                m_Image = GetComponent<Image>();
            }

            base.OnValidate();
        }
    }
}
#endif
