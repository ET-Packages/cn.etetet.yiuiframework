#if UNITY_EDITOR
using UnityEngine.UI;

namespace YIUIFramework
{
    public sealed partial class UIDataBindImageFormat
    {
        public void EditorConfigureImageFormat(string format, bool setNativeSize, bool changeEnabled)
        {
            m_Format = format ?? string.Empty;
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
