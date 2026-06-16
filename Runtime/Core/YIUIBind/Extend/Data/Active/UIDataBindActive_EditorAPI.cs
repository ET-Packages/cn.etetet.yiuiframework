#if UNITY_EDITOR
using System;

namespace YIUIFramework
{
    public sealed partial class UIDataBindActive
    {
        public bool EditorConfigureActiveTransition(string transitionMode, float transitionTime, out string error)
        {
            error = string.Empty;

            if (!string.IsNullOrEmpty(transitionMode))
            {
                if (!Enum.TryParse(transitionMode, true, out UITransitionModeEnum parsedMode))
                {
                    error = $"{name} 不支持的 TransitionMode: {transitionMode}";
                    return false;
                }

                m_TransitionMode = parsedMode;
            }

            m_TransitionTime = transitionTime;
            base.OnValidate();
            return true;
        }
    }
}
#endif
