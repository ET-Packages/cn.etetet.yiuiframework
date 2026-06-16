#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework
{
    public sealed partial class UIDataBindImageFill
    {
        public bool EditorConfigureImageFill(float tweenSpeed, string tweenType, out string error)
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

            if (m_Image == null)
            {
                m_Image = GetComponent<Image>();
            }

            if (m_Image != null && m_TweenSpeed <= 0 && !Mathf.Approximately(m_Image.fillAmount, m_TargetValue))
            {
                m_Image.fillAmount = m_TargetValue;
            }

            base.OnValidate();
            return true;
        }
    }
}
#endif
