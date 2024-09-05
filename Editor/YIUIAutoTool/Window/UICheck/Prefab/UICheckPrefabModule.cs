using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            InitGetAll();
        }

        [GUIColor(1, 0, 0)]
        [Button("删除所有", 50, Icon = SdfIconType.X)]
        [PropertyOrder(-88)]
        [ShowIf("ShowIfDeleteAll")]
        public void DeleteAll()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            List<YIUICheckPrefabData> deleteVo = new();
            foreach (var bind in m_CheckPrefabs)
            {
                var result = bind.ShowIfDelete();
                if (result)
                {
                    deleteVo.Add(bind);
                }
            }

            var count = deleteVo.Count;
            for (int index = 0; index < deleteVo.Count; index++)
            {
                var bind = deleteVo[index];
                bind.Delete(false, false);
                EditorUtility.DisplayProgressBar("同步信息", $"删除资源信息 {bind.ResName}", index * 1.0f / count);
            }

            EditorUtility.ClearProgressBar();

            UnityTipsHelper.Show($"删除成功: {count}个相关文件");

            YIUIAutoTool.CloseWindowRefresh();
        }

        private bool ShowIfDeleteAll()
        {
            if (m_CheckPrefabs is not { Count: > 0 }) return false;

            foreach (var bind in m_CheckPrefabs)
            {
                var result = bind.ShowIfDelete();
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
        [HideReferenceObjectPicker]
        private List<YIUICheckPrefabData> m_CheckPrefabs = new();

        public override void Initialize()
        {
        }

        public override void OnDestroy()
        {
        }

        private readonly string m_Pattern = @"YIUI\\[^\\]*\\Prefabs";

        private void InitGetAll()
        {
            m_CheckPrefabs.Clear();
            var allPrefabs = AssetDatabase.FindAssets("t:Prefab", null);
            var allCount   = allPrefabs.Length;
            for (int index = 0; index < allPrefabs.Length; index++)
            {
                var guid = allPrefabs[index];
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var normalizedPath = Path.GetFullPath(path);

                var match = Regex.Match(normalizedPath, m_Pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    var pkgName     = match.Value.Split('\\')[1];
                    var fileNameAll = Path.GetFileName(path);
                    var fileName    = fileNameAll.Split('.')[0];
                    m_CheckPrefabs.Add(new YIUICheckPrefabData(path, pkgName, fileName));
                    EditorUtility.DisplayProgressBar("同步信息", $"初始化 {pkgName},{fileName}", index * 1.0f / allCount);
                }
            }

            EditorUtility.ClearProgressBar();
        }
    }
}