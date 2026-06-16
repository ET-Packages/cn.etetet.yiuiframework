#if UNITY_EDITOR
using System;
using UnityEngine.UI;

namespace YIUIFramework
{
    public partial class UIDataBindSlider
    {
        public bool EditorConfigureSlider(float tweenSpeed, string tweenType, out string error)
        {
            error = string.Empty;
            m_TweenSpeed = tweenSpeed;

            if (!string.IsNullOrEmpty(tweenType))
            {
                if (!Enum.TryParse(tweenType, true, out ETweenType parsedTweenType))
                {
                    error = $"{name} 不支持的 TweenType: {tweenType}";
                    return false;
                }

                m_TweenType = parsedTweenType;
            }

            if (m_Slider == null)
            {
                m_Slider = GetComponent<Slider>();
            }

            base.OnValidate();
            return true;
        }
    }
}
#endif
