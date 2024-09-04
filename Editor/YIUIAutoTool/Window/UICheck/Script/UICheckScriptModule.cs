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
    public class UICheckScriptModule : BaseYIUIToolModule
    {
        [GUIColor(0.4f, 0.8f, 1)]
        [Button("检查", 50)]
        [PropertyOrder(-99)]
        public void CheckAll()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            EditorUtility.DisplayProgressBar("同步信息", $"刷新资源信息", 0);

            var count = m_Binds.Count;
            for (int index = 0; index < m_Binds.Count; index++)
            {
                var bind = this.m_Binds[index];
                bind.UpdatCheck();
                EditorUtility.DisplayProgressBar("同步信息", $"刷新资源信息 {bind.ComponentType.Name}", index * 1.0f / count);
            }

            EditorUtility.ClearProgressBar();
        }

        [GUIColor(1, 0, 0)]
        [Button("删除所有", 50, Icon = SdfIconType.X)]
        [PropertyOrder(-88)]
        [ShowIf("ShowIfDeleteAll")]
        public void DeleteAll()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            List<YIUICheckVo> deleteVo = new();
            foreach (var bind in m_Binds)
            {
                var result = bind.ShowIfDeleteScript();
                if (result)
                {
                    deleteVo.Add(bind);
                }
            }

            var count = deleteVo.Count;
            for (int index = 0; index < deleteVo.Count; index++)
            {
                var bind = deleteVo[index];
                bind.DeleteScript(false, false);
                EditorUtility.DisplayProgressBar("同步信息", $"删除资源信息 {bind.ComponentType.Name}", index * 1.0f / count);
            }

            EditorUtility.ClearProgressBar();

            UnityTipsHelper.Show($"删除成功: {count}个相关文件");

            YIUIAutoTool.CloseWindowRefresh();
        }

        private bool ShowIfDeleteAll()
        {
            if (m_Binds is not { Count: > 0 }) return false;

            foreach (var bind in m_Binds)
            {
                var result = bind.ShowIfDeleteScript();
                if (result)
                {
                    return true;
                }
            }

            return false;
        }

        [TableList(DrawScrollView = true, IsReadOnly = true)]
        [BoxGroup("检查结果", centerLabel: true)]
        [HideLabel]
        [ShowInInspector]
        private List<YIUICheckVo> m_Binds;

        public override void Initialize()
        {
            InitGetAll();
        }

        public override void OnDestroy()
        {
        }

        private void InitGetAll()
        {
            var types = AssemblyHelper.GetAllTypes();

            m_Binds = new List<YIUICheckVo>();

            foreach (var type in types)
            {
                if (type.IsAbstract) continue;
                var attribute = type.GetCustomAttribute<YIUIAttribute>();
                if (attribute == null) continue;
                if (GetBindVo(out var bindVo, attribute, type))
                {
                    this.m_Binds.Add(bindVo);
                }
            }
        }

        private static bool GetBindVo(out YIUICheckVo bindVo,
                                      YIUIAttribute   attribute,
                                      Type            componentType)
        {
            bindVo = new YIUICheckVo();
            if (componentType == null ||
                !componentType.GetFieldValue("PkgName", out bindVo.PkgName) ||
                !componentType.GetFieldValue("ResName", out bindVo.ResName))
            {
                return false;
            }

            bindVo.ComponentType = componentType;
            bindVo.CodeType      = attribute.YIUICodeType;
            bindVo.PanelLayer    = attribute.YIUIPanelLayer;
            if (bindVo is { CodeType: EUICodeType.Panel, PanelLayer: EPanelLayer.Any })
            {
                Debug.LogError($"{componentType.Name} 错误的设定 既然是Panel 那必须设定所在层级 不能是Any 请检查重新导出");
            }

            return true;
        }
    }
}