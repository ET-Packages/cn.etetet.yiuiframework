#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    ///  宏
    /// </summary>
    [YIUIAutoMenu("宏设置", 200100)]
    public class YIUIMacroModule : BaseYIUIToolModule
    {
        [BoxGroup("宏", centerLabel: true)]
        [HideLabel]
        [EnumToggleButtons]
        [OnValueChanged("OnMacroTypeChanged")]
        public YIUIMacroType MacroType = YIUIMacroType.ET;

        [HideLabel]
        public BaseYIUIToolModule CurrentMacroModule;

        private void OnMacroTypeChanged()
        {
            SelectMacroType(MacroType);
        }

        private void SelectMacroType(YIUIMacroType macroType)
        {
            switch (macroType)
            {
                case YIUIMacroType.ET:
                    CurrentMacroModule = null;
                    break;
                case YIUIMacroType.Unity:
                    CurrentMacroModule = new UnityMacroModule();
                    break;
                default:
                    UnityTipsHelper.ShowError($"未知的宏类型:{macroType}");
                    CurrentMacroModule = null;
                    break;
            }

            if (CurrentMacroModule != null)
            {
                CurrentMacroModule.Initialize();
            }

            MacroType = macroType;
        }

        public override void Initialize()
        {
            SelectMacroType(YIUIMacroType.ET);
        }

        public override void OnDestroy()
        {
        }
    }

    public enum YIUIMacroType
    {
        ET,
        Unity
    }
}
#endif