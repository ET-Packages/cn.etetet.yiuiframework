#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [HideReferenceObjectPicker]
    public abstract class BaseYIUIToolModule : BaseCreateModule
    {
        [HideInInspector]
        public OdinMenuEditorWindow AutoTool { get; internal set; }

        [HideInInspector]
        public OdinMenuTree Tree { get; internal set; }

        [HideInInspector]
        public string ModuleName { get; internal set; }

        [HideInInspector]
        public object UserData { get; internal set; }

        public virtual void SelectionMenu()
        {
        }
    }
}
#endif
