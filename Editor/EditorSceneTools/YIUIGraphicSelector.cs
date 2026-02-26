using System;
using System.Collections.Generic;
using System.Linq;
using ET;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace YIUIFramework.Editor
{
    public class YIUIGraphicSelector : UnityEditor.Editor
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            YIUIGraphicSelectorEnableReset(false);
            SceneView.duringSceneGui += DuringSceneGUI;
        }

        [InitializeOnEnterPlayMode]
        private static void Start()
        {
            YIUIGraphicSelectorEnableReset();
        }

        private static readonly Vector3[] FourCorners = new Vector3[4];
        private static readonly Vector3 LabelOffset = new(-5, 10, 0);
        private static readonly List<(GameObject gameObject, Vector3 position)> HoveredObjects = new List<(GameObject, Vector3)>();
        private static GUIStyle _labelStyle;

        private static GUIStyle LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle()
                    {
                        fontSize = 13,
                        alignment = TextAnchor.MiddleLeft,
                        normal = { textColor = Color.white },
                        padding = new RectOffset(6, 6, 2, 2),
                        border = new RectOffset(2, 2, 2, 2)
                    };

                    var backgroundTexture = new Texture2D(1, 1);
                    backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.8f));
                    backgroundTexture.Apply();

                    _labelStyle.normal.background = backgroundTexture;
                }

                return _labelStyle;
            }
        }

        private const float Size = 5;
        private static readonly Vector3 Offset = new(Size, -Size, 0);
        private static bool m_Enable;

        public static bool Enable
        {
            get => m_Enable;
            set
            {
                m_Enable = value;
                EditorPrefs.SetBool(YIUIConstAsset.YIUIGraphicSelectorEnableKey, value);
                YIUIGraphicSelectorEnableLog();
            }
        }

        private static void YIUIGraphicSelectorEnableLog()
        {
            Debug.Log($"<color=yellow>YIUI图层快选工具:</color> {(m_Enable ? "<color=green>启用</color>" : "<color=red>禁用</color>")}");
        }

        private static void YIUIGraphicSelectorEnableReset(bool log = true)
        {
            m_Enable = EditorPrefs.GetBool(YIUIConstAsset.YIUIGraphicSelectorEnableKey, true);
            if (log)
            {
                YIUIGraphicSelectorEnableLog();
            }
        }

        /// <summary>
        /// 安全获取鼠标射线，鼠标不在窗口内或计算失败时返回 null
        /// </summary>
        private static Ray? TryGetMouseRay(Camera cam)
        {
            // 非鼠标/绘制事件时相机投影矩阵可能未就绪，直接跳过
            var evtType = Event.current.type;
            if (evtType != EventType.MouseMove && evtType != EventType.MouseDown &&
                evtType != EventType.MouseUp && evtType != EventType.MouseDrag &&
                evtType != EventType.Repaint)
            {
                return null;
            }

            var mousePosition = Event.current.mousePosition;
            var screenPos = HandleUtility.GUIPointToScreenPixelCoordinate(mousePosition);

            // 检查屏幕坐标是否在相机视口范围内
            var pixelRect = cam.pixelRect;
            if (screenPos.x < pixelRect.xMin || screenPos.x > pixelRect.xMax ||
                screenPos.y < pixelRect.yMin || screenPos.y > pixelRect.yMax)
            {
                return null;
            }

            // 使用ViewportPointToRay避免屏幕坐标超出视锥体的问题
            var viewportPoint = cam.ScreenToViewportPoint(screenPos);
            const float epsilon = 0.001f;
            if (viewportPoint.x < epsilon || viewportPoint.x > 1f - epsilon ||
                viewportPoint.y < epsilon || viewportPoint.y > 1f - epsilon)
            {
                return null;
            }

            try
            {
                return cam.ViewportPointToRay(viewportPoint);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static void DuringSceneGUI(SceneView sceneView)
        {
            if (!m_Enable)
            {
                return;
            }

            HoveredObjects.Clear();

            var cam = sceneView.camera ?? Camera.current;
            if (cam == null)
            {
                return;
            }

            // 尝试获取鼠标射线，鼠标不在窗口内时为 null（不影响画框和点击功能）
            Ray? ray = TryGetMouseRay(cam);

            var currentPrefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            var allGraphics = currentPrefabStage != null ? currentPrefabStage.prefabContentsRoot.GetComponentsInChildren<Graphic>() : FindObjectsByType<Graphic>(FindObjectsSortMode.None);

            foreach (var g in allGraphics)
            {
                if (g.canvas == null)
                {
                    continue;
                }

                var canvasGroup = g.gameObject.GetComponentInParent<CanvasGroup>();
                if (canvasGroup && canvasGroup.alpha == 0)
                {
                    continue;
                }

                var scale = g.canvas.rootCanvas.transform.localScale.x;

                if (g.gameObject.activeInHierarchy)
                {
                    // 画框 + 点击跳转 — 始终执行，不依赖鼠标位置
                    Handles.color = Color.green;
                    g.rectTransform.GetWorldCorners(FourCorners);

                    var buttonPosition = FourCorners[1] + Offset * scale;

                    if (Handles.Button(buttonPosition, Quaternion.identity, Size * scale, 0, Handles.RectangleHandleCap))
                    {
                        var current = Event.current;
                        if (current.control)
                        {
                            Unity.CodeEditor.CodeEditor.Editor.CurrentCodeEditor.OpenProject();
                        }
                        else if (current.shift)
                        {
                            Selection.objects = Selection.objects.Append(g.gameObject).ToArray();
                        }
                        else
                        {
                            Selection.activeObject = g.gameObject;
                        }
                    }

                    // Hover 标签 — 仅当鼠标在窗口内（ray 有效）时才检测
                    if (ray.HasValue)
                    {
                        var rayOrigin = ray.Value.origin;
                        if (rayOrigin.x < buttonPosition.x + Size * scale / 2
                         && rayOrigin.x > buttonPosition.x - Size * scale / 2
                         && rayOrigin.y < buttonPosition.y + Size * scale / 2
                         && rayOrigin.y > buttonPosition.y - Size * scale / 2)
                        {
                            HoveredObjects.Add((g.gameObject, rayOrigin + LabelOffset * scale));
                        }
                    }
                }
            }

            DisplayHoveredObjectLabels();
        }

        private static void DisplayHoveredObjectLabels()
        {
            if (HoveredObjects.Count == 0) return;

            if (HoveredObjects.Count == 1)
            {
                var (gameObject, position) = HoveredObjects[0];
                Handles.Label(position, gameObject.name, LabelStyle);
                return;
            }

            var labelBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < HoveredObjects.Count; i++)
            {
                var (gameObject, _) = HoveredObjects[i];
                labelBuilder.AppendLine($"[{i + 1}] {gameObject.name}");
            }

            var combinedLabel = labelBuilder.ToString().TrimEnd('\r', '\n');
            Handles.Label(HoveredObjects[0].position, combinedLabel, LabelStyle);
        }
    }
}