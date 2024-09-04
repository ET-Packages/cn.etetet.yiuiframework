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
    public class UICheckPrefabModule : BaseYIUIToolModule
    {
        [GUIColor(0.4f, 0.8f, 1)]
        [Button("检查", 50)]
        [PropertyOrder(-99)]
        public void CheckAll()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;
        }

        public override void Initialize()
        {
            InitGetAll();
        }

        public override void OnDestroy()
        {
        }

        private void InitGetAll()
        {
        }
    }
}