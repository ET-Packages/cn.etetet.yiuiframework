using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [YIUIAutoMenu("检查", int.MaxValue)]
    public class UICheckModule : BaseYIUIToolModule
    {
        [Button("文档", 30, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/F1cDwqKSliIiBIkWMwmcfsj6nUe");
        }

        [BoxGroup("检查类型", centerLabel: true)]
        [HideLabel]
        [EnumToggleButtons]
        [OnValueChanged("OnCheckViewTypeChanged")]
        public EYIUICheckViewType CheckViewType = EYIUICheckViewType.Script;

        [HideLabel]
        public BaseYIUIToolModule CurrentCheckViewModule;

        [HideLabel]
        private EnumPrefs<EYIUICheckViewType> YIUICheckViewPrefs = new("YIUIAutoTool_YIUICheckViewPrefs");

        private void OnCheckViewTypeChanged()
        {
            SelectCheckViewType(CheckViewType);
        }

        private void SelectCheckViewType(EYIUICheckViewType checkType)
        {
            switch (checkType)
            {
                case EYIUICheckViewType.Script:
                    CurrentCheckViewModule = new UICheckScriptModule();
                    break;
                case EYIUICheckViewType.Prefab:
                    CurrentCheckViewModule = new UICheckScriptModule();
                    break;
                default:
                    UnityTipsHelper.ShowError($"未知的类型:{checkType}");
                    CurrentCheckViewModule = null;
                    break;
            }

            if (CurrentCheckViewModule != null)
            {
                CurrentCheckViewModule.Initialize();
            }

            CheckViewType = checkType;
        }

        public override void Initialize()
        {
            SelectCheckViewType(YIUICheckViewPrefs.Value);
        }

        public override void OnDestroy()
        {
            YIUICheckViewPrefs.Value = CheckViewType;
        }
    }
}