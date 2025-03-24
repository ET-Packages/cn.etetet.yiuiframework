#if ENABLE_VIEW
using System;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static partial class ComponentViewHelper
    {
        public static void DrawScripButton(Entity entity)
        {
            string componentName = entity.GetType().Name;

            // 绘制两个并排的按钮，一个查看View代码，一个查看System代码
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space(20, false);
            if (GUILayout.Button("查看Component代码"))
            {
                ScriptHelper.OpenScript(componentName);
            }

            if (GUILayout.Button("查看System代码"))
            {
                ScriptHelper.OpenScript($"{componentName}System");
            }

            EditorGUILayout.Space(20, false);
            EditorGUILayout.EndHorizontal();
        }
    }

    public static partial class ComponentViewHelper
    {
        static partial void OverrideDraw()
        {
            drawAction = YIUIDrawAction;
        }

        private static void YIUIDrawAction(Entity entity)
        {
            try
            {
                DrawScripButton(entity);
                Draw(entity);
            }
            catch (Exception e)
            {
                Debug.Log($"组件视图绘制错误: {entity.GetType().FullName}, {e}");
            }
        }
    }
}
#endif