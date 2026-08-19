#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace YIUIFramework.Editor
{
    public sealed class YIUIPrefabPreviewCaptureResult
    {
        public string PrefabPath { get; set; }

        public string OutputPath { get; set; }

        public int DesignWidth { get; set; }

        public int DesignHeight { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public float Scale { get; set; }

        public bool IsScaled { get; set; }

        public float MatchWidthOrHeight { get; set; }

        public string ResolutionSource { get; set; }

        public bool Exists { get; set; }
    }

    /// <summary>
    /// 在不进入 PlayMode 的情况下，按 YIUI 配置的设计分辨率渲染选中的 UGUI Prefab。
    /// </summary>
    public sealed class YIUIPrefabPreviewWindow : EditorWindow
    {
        private const float CameraDistance = 100f; // 预览相机与 ScreenSpaceCamera Canvas 的距离。
        private const float OrthographicHalfHeightScale = 0.5f; // 将设计高度换算为正交相机半高。
        private const float MinOutputScale = 0.01f; // 输出缩放比例下限，避免生成零像素图片。
        private const float MaxOutputScale = 1f; // 输出缩放比例上限，1表示保留设计分辨率。

        private static readonly Color PreviewBackground = new(0.075f, 0.085f, 0.1f, 1f);

        private readonly struct PreviewDesignSettings
        {
            public PreviewDesignSettings(int width, int height, float matchWidthOrHeight)
            {
                Width = width;
                Height = height;
                MatchWidthOrHeight = matchWidthOrHeight;
            }

            public int Width { get; }

            public int Height { get; }

            public float MatchWidthOrHeight { get; }
        }

        private PreviewRenderUtility previewUtility;
        private GameObject previewRoot;
        private Canvas previewCanvas;
        private GameObject previewPrefabInstance;
        private GameObject previewEventSystem;
        private GameObject selectedPrefab;
        private string selectedPrefabPath;
        private PreviewDesignSettings designSettings;
        private string designSettingsError;
        private bool autoFollowSelection = true;
        private bool showGuides = true;

        [MenuItem("ET/YIUI/Prefab视觉预览")]
        public static void Open()
        {
            var window = GetWindow<YIUIPrefabPreviewWindow>("YIUI Prefab预览");
            window.minSize = new Vector2(620f, 420f);
            window.Show();
            window.RefreshFromSelection();
        }

        public static void OpenPrefab(GameObject prefab)
        {
            if (prefab == null)
            {
                throw new ArgumentNullException(nameof(prefab), "打开Prefab视觉预览失败：Prefab=<null>");
            }

            var prefabPath = AssetDatabase.GetAssetPath(prefab);
            if (!prefabPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("打开Prefab视觉预览失败：目标不是Prefab，PrefabPath=" + prefabPath, nameof(prefab));
            }

            var window = GetWindow<YIUIPrefabPreviewWindow>("YIUI Prefab预览");
            window.minSize = new Vector2(620f, 420f);
            window.Show();
            window.SetPrefab(prefab);
        }

        [MenuItem("ET/YIUI/Prefab视觉预览/导出当前Prefab PNG")]
        private static void CaptureSelectedPrefab()
        {
            var prefab = Selection.activeObject as GameObject;
            var prefabPath = prefab == null ? string.Empty : AssetDatabase.GetAssetPath(prefab);
            if (prefab == null || !prefabPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                Debug.LogError("YIUI预览失败：当前选择不是Prefab，无法执行离屏渲染。实际选择=" + (Selection.activeObject == null ? "<null>" : Selection.activeObject.name));
                return;
            }

            try
            {
                var result = CapturePrefabToPng(prefabPath);
                Debug.Log("YIUI预览完成：Prefab=" + result.PrefabPath + "，PNG=" + result.OutputPath + "，分辨率=" + result.Width + "x" + result.Height + "，PlayMode=false");
            }
            catch (Exception exception)
            {
                Debug.LogError("YIUI预览失败：离屏渲染异常，Prefab=" + prefabPath + "，原因=" + exception.Message);
            }
        }

        public static YIUIPrefabPreviewCaptureResult CapturePrefabToPng(
            string prefabPath, string outputDirectory = null,
            float scale = MaxOutputScale)
        {
            if (string.IsNullOrWhiteSpace(prefabPath))
            {
                throw new ArgumentException("YIUI预览失败：PrefabPath=<空>", nameof(prefabPath));
            }

            var normalizedPrefabPath = prefabPath.Trim().Replace('\\', '/');
            if (!normalizedPrefabPath.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("YIUI预览失败：目标路径不是.prefab，PrefabPath=" + normalizedPrefabPath, nameof(prefabPath));
            }

            if (scale < MinOutputScale || scale > MaxOutputScale)
            {
                throw new ArgumentOutOfRangeException(nameof(scale),
                    "YIUI预览失败：Scale必须在" + MinOutputScale + "到" + MaxOutputScale +
                    "之间，实际Scale=" + scale);
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(normalizedPrefabPath);
            if (prefab == null)
            {
                throw new FileNotFoundException("YIUI预览失败：找不到Prefab资源，PrefabPath=" + normalizedPrefabPath, normalizedPrefabPath);
            }

            var settings = ResolveDesignSettings();
            var outputWidth = Mathf.Max(1,
                Mathf.RoundToInt(settings.Width * scale));
            var outputHeight = Mathf.Max(1,
                Mathf.RoundToInt(settings.Height * scale));
            var isScaled = outputWidth != settings.Width ||
                outputHeight != settings.Height;

            var resolvedOutputDirectory = string.IsNullOrWhiteSpace(outputDirectory)
                ? Path.Combine("Library", "YIUI", "PrefabPreviews")
                : outputDirectory.Trim();
            var absoluteOutputDirectory = Path.GetFullPath(resolvedOutputDirectory);
            Directory.CreateDirectory(absoluteOutputDirectory);
            var absoluteOutputPath = Path.Combine(absoluteOutputDirectory,
                prefab.name + "_" + outputWidth + "x" + outputHeight + ".png");

            Texture2D designTexture = null;
            Texture2D outputTexture = null;
            try
            {
                designTexture = RenderPrefabToTexture(prefab, settings);
                outputTexture = isScaled
                    ? ResizeTexture(designTexture, outputWidth, outputHeight)
                    : designTexture;
                File.WriteAllBytes(absoluteOutputPath,
                    outputTexture.EncodeToPNG());
            }
            finally
            {
                if (outputTexture != null && outputTexture != designTexture)
                {
                    DestroyImmediate(outputTexture);
                }

                if (designTexture != null)
                {
                    DestroyImmediate(designTexture);
                }
            }

            return new YIUIPrefabPreviewCaptureResult
            {
                PrefabPath = normalizedPrefabPath,
                OutputPath = absoluteOutputPath,
                DesignWidth = settings.Width,
                DesignHeight = settings.Height,
                Width = outputWidth,
                Height = outputHeight,
                Scale = scale,
                IsScaled = isScaled,
                MatchWidthOrHeight = settings.MatchWidthOrHeight,
                ResolutionSource = YIUIConstHelper.YIUIConstAssetPath,
                Exists = File.Exists(absoluteOutputPath),
            };
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.projectChanged += OnProjectChanged;
            RefreshDesignSettings();
            RefreshFromSelection();
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            EditorApplication.projectChanged -= OnProjectChanged;
            CleanupPreview();
        }

        private void OnSelectionChanged()
        {
            if (autoFollowSelection)
            {
                RefreshFromSelection();
            }
        }

        private void OnProjectChanged()
        {
            RefreshDesignSettings();
            if (selectedPrefab != null)
            {
                RebuildPreview();
            }
        }

        private void OnGUI()
        {
            DrawToolbar();

            if (!string.IsNullOrEmpty(designSettingsError))
            {
                EditorGUILayout.HelpBox(designSettingsError, MessageType.Error);
                return;
            }

            if (selectedPrefab == null)
            {
                EditorGUILayout.HelpBox("请在 Project 窗口选中一个 UGUI Prefab，或使用上方对象框指定 Prefab。", MessageType.Info);
                return;
            }

            if (previewUtility == null || previewPrefabInstance == null)
            {
                RebuildPreview();
            }

            var previewArea = GUILayoutUtility.GetRect(10f, 10000f, 10f, 10000f, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            DrawPreview(previewArea);
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                var nextPrefab = (GameObject)EditorGUILayout.ObjectField(selectedPrefab, typeof(GameObject), false, GUILayout.MinWidth(180f));
                if (nextPrefab != selectedPrefab)
                {
                    SetPrefab(nextPrefab);
                }

                autoFollowSelection = GUILayout.Toggle(autoFollowSelection, "跟随选择", EditorStyles.toolbarButton, GUILayout.Width(70f));
                showGuides = GUILayout.Toggle(showGuides, "设计分辨率参考线", EditorStyles.toolbarButton, GUILayout.Width(112f));

                if (GUILayout.Button("刷新", EditorStyles.toolbarButton, GUILayout.Width(48f)))
                {
                    RebuildPreview();
                }

                if (GUILayout.Button("清空", EditorStyles.toolbarButton, GUILayout.Width(48f)))
                {
                    SetPrefab(null);
                }
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(string.IsNullOrEmpty(selectedPrefabPath) ? "未选择 Prefab" : selectedPrefabPath, EditorStyles.miniLabel);
                GUILayout.FlexibleSpace();
                var resolutionText = string.IsNullOrEmpty(designSettingsError)
                    ? "设计分辨率 " + designSettings.Width + " x " + designSettings.Height
                    : "设计分辨率配置无效";
                EditorGUILayout.LabelField(resolutionText, EditorStyles.miniLabel, GUILayout.Width(150f));
            }
        }

        private void DrawPreview(Rect area)
        {
            if (area.width < 20f || area.height < 20f || previewUtility == null)
            {
                return;
            }

            var aspect = designSettings.Width / (float)designSettings.Height;
            var drawRect = area;
            if (drawRect.width / drawRect.height > aspect)
            {
                var width = drawRect.height * aspect;
                drawRect.x += (drawRect.width - width) * 0.5f;
                drawRect.width = width;
            }
            else
            {
                var height = drawRect.width / aspect;
                drawRect.y += (drawRect.height - height) * 0.5f;
                drawRect.height = height;
            }

            if (Event.current.type == EventType.Repaint)
            {
                previewUtility.BeginPreview(drawRect, GUIStyle.none);
                ConfigurePreviewCamera();
                Canvas.ForceUpdateCanvases();
                previewUtility.Render(true, true);
                previewUtility.EndAndDrawPreview(drawRect);
            }

            if (showGuides)
            {
                Handles.BeginGUI();
                var oldColor = Handles.color;
                Handles.color = new Color(0.25f, 0.75f, 1f, 0.55f);
                Handles.DrawAAPolyLine(1f,
                    new Vector3(drawRect.x, drawRect.y),
                    new Vector3(drawRect.xMax, drawRect.y),
                    new Vector3(drawRect.xMax, drawRect.yMax),
                    new Vector3(drawRect.x, drawRect.yMax),
                    new Vector3(drawRect.x, drawRect.y));
                Handles.color = oldColor;
                Handles.EndGUI();
            }

            if (Event.current.type == EventType.MouseDown && drawRect.Contains(Event.current.mousePosition))
            {
                Repaint();
            }
        }

        private void RefreshFromSelection()
        {
            var candidate = Selection.activeObject as GameObject;
            if (candidate == null)
            {
                return;
            }

            var path = AssetDatabase.GetAssetPath(candidate);
            if (!path.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            SetPrefab(candidate);
        }

        private void SetPrefab(GameObject prefab)
        {
            if (prefab != null && !AssetDatabase.GetAssetPath(prefab).EndsWith(".prefab", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (selectedPrefab == prefab)
            {
                return;
            }

            selectedPrefab = prefab;
            selectedPrefabPath = prefab == null ? string.Empty : AssetDatabase.GetAssetPath(prefab);
            RebuildPreview();
        }

        private void RebuildPreview()
        {
            CleanupPreview();
            if (!RefreshDesignSettings())
            {
                Repaint();
                return;
            }

            if (selectedPrefab == null)
            {
                Repaint();
                return;
            }

            previewUtility = new PreviewRenderUtility();
            previewUtility.camera.orthographic = true;
            previewUtility.camera.orthographicSize = designSettings.Height * OrthographicHalfHeightScale;
            previewUtility.camera.nearClipPlane = 0.01f;
            previewUtility.camera.farClipPlane = CameraDistance + 10f;
            previewUtility.camera.clearFlags = CameraClearFlags.SolidColor;
            previewUtility.camera.backgroundColor = PreviewBackground;
            previewUtility.camera.allowHDR = false;
            previewUtility.camera.allowMSAA = false;
            previewUtility.ambientColor = Color.white;

            previewRoot = new GameObject("YIUI_PrefabPreviewRoot", typeof(RectTransform));
            previewRoot.hideFlags = HideFlags.HideAndDontSave;
            previewUtility.AddSingleGO(previewRoot);

            previewCanvas = previewRoot.AddComponent<Canvas>();
            previewCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            previewCanvas.worldCamera = previewUtility.camera;
            previewCanvas.planeDistance = CameraDistance;
            previewCanvas.pixelPerfect = false;

            var scaler = previewRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(designSettings.Width, designSettings.Height);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = designSettings.MatchWidthOrHeight;

            previewEventSystem = new GameObject("YIUI_PrefabPreviewEventSystem", typeof(EventSystem));
            previewEventSystem.hideFlags = HideFlags.HideAndDontSave;
            previewUtility.AddSingleGO(previewEventSystem);

            previewPrefabInstance = InstantiatePrefabForPreview(selectedPrefab, previewRoot.transform);
            if (previewPrefabInstance == null)
            {
                CleanupPreview();
                return;
            }

            Repaint();
        }

        private static GameObject InstantiatePrefabForPreview(GameObject prefab, Transform parent)
        {
            var instance = Instantiate(prefab, parent);
            instance.name = prefab.name + "_PreviewInstance";
            instance.hideFlags = HideFlags.HideAndDontSave;
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = Vector3.one;

            DisableNonUIBehaviours(instance);

            return instance;
        }

        private static Texture2D RenderPrefabToTexture(GameObject prefab, PreviewDesignSettings settings)
        {
            var previewScene = EditorSceneManager.NewPreviewScene();
            var previousRenderTexture = RenderTexture.active;
            RenderTexture renderTexture = null;
            try
            {
                var cameraObject = new GameObject("YIUI_OfflinePreviewCamera", typeof(Camera));
                cameraObject.hideFlags = HideFlags.HideAndDontSave;
                SceneManager.MoveGameObjectToScene(cameraObject, previewScene);
                var camera = cameraObject.GetComponent<Camera>();
                camera.scene = previewScene;
                camera.transform.position = new Vector3(0f, 0f, -CameraDistance);
                camera.transform.rotation = Quaternion.identity;
                camera.orthographic = true;
                camera.orthographicSize = settings.Height * OrthographicHalfHeightScale;
                camera.nearClipPlane = 0.01f;
                camera.farClipPlane = CameraDistance + 10f;
                camera.aspect = settings.Width / (float)settings.Height;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = PreviewBackground;
                camera.allowHDR = false;
                camera.allowMSAA = false;

                renderTexture = new RenderTexture(settings.Width, settings.Height, 24, RenderTextureFormat.ARGB32)
                {
                    hideFlags = HideFlags.HideAndDontSave,
                    name = "YIUI_OfflinePreviewRenderTexture"
                };
                renderTexture.Create();
                camera.targetTexture = renderTexture;
                camera.pixelRect = new Rect(0f, 0f, settings.Width, settings.Height);

                var canvasObject = new GameObject("YIUI_OfflinePreviewCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
                canvasObject.hideFlags = HideFlags.HideAndDontSave;
                SceneManager.MoveGameObjectToScene(canvasObject, previewScene);
                var canvas = canvasObject.GetComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = camera;
                canvas.planeDistance = CameraDistance;
                canvas.pixelPerfect = false;

                var scaler = canvasObject.GetComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(settings.Width, settings.Height);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = settings.MatchWidthOrHeight;

                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, previewScene);
                instance.hideFlags = HideFlags.HideAndDontSave;
                instance.transform.SetParent(canvasObject.transform, false);
                instance.transform.localPosition = Vector3.zero;
                instance.transform.localRotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one;
                DisableNonUIBehaviours(instance);

                Canvas.ForceUpdateCanvases();
                camera.Render();

                RenderTexture.active = renderTexture;
                var texture = new Texture2D(settings.Width, settings.Height, TextureFormat.RGBA32, false, false);
                texture.ReadPixels(new Rect(0f, 0f, settings.Width, settings.Height), 0, 0);
                texture.Apply(false, false);
                return texture;
            }
            finally
            {
                RenderTexture.active = previousRenderTexture;
                if (renderTexture != null)
                {
                    var sceneRoots = previewScene.GetRootGameObjects();
                    foreach (var root in sceneRoots)
                    {
                        var previewCamera = root.GetComponent<Camera>();
                        if (previewCamera != null && previewCamera.targetTexture == renderTexture)
                        {
                            previewCamera.targetTexture = null;
                        }
                    }

                    renderTexture.Release();
                    DestroyImmediate(renderTexture);
                }

                EditorSceneManager.ClosePreviewScene(previewScene);
            }
        }

        private static void DisableNonUIBehaviours(GameObject instance)
        {

            var behaviours = instance.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var behaviour in behaviours)
            {
                if (behaviour == null || behaviour is UIBehaviour)
                {
                    continue;
                }

                behaviour.enabled = false;
            }
        }

        private void ConfigurePreviewCamera()
        {
            if (previewUtility == null)
            {
                return;
            }

            var camera = previewUtility.camera;
            camera.transform.position = new Vector3(0f, 0f, -CameraDistance);
            camera.transform.rotation = Quaternion.identity;
            camera.orthographic = true;
            camera.orthographicSize = designSettings.Height * OrthographicHalfHeightScale;
            camera.aspect = designSettings.Width / (float)designSettings.Height;
        }

        private bool RefreshDesignSettings()
        {
            try
            {
                designSettings = ResolveDesignSettings();
                designSettingsError = string.Empty;
                return true;
            }
            catch (Exception exception)
            {
                designSettingsError = exception.Message;
                return false;
            }
        }

        private static PreviewDesignSettings ResolveDesignSettings()
        {
            var settings = YIUIConstHelper.Const;
            if (settings == null)
            {
                throw new InvalidOperationException("YIUI预览失败：无法加载YIUI配置，配置路径=" + YIUIConstHelper.YIUIConstAssetPath);
            }

            var width = settings.DesignScreenWidth;
            var height = settings.DesignScreenHeight;
            var matchWidthOrHeight = settings.MatchWidthOrHeight;
            if (width <= 0 || height <= 0)
            {
                throw new InvalidOperationException(
                    "YIUI预览失败：设计分辨率必须大于0，配置路径=" + YIUIConstHelper.YIUIConstAssetPath +
                    "，DesignScreenWidth=" + width + "，DesignScreenHeight=" + height);
            }

            var maxTextureSize = SystemInfo.maxTextureSize;
            if (width > maxTextureSize || height > maxTextureSize)
            {
                throw new InvalidOperationException(
                    "YIUI预览失败：设计分辨率超过当前设备纹理上限，配置路径=" + YIUIConstHelper.YIUIConstAssetPath +
                    "，DesignScreenWidth=" + width + "，DesignScreenHeight=" + height + "，MaxTextureSize=" + maxTextureSize);
            }

            if (matchWidthOrHeight is < 0f or > 1f)
            {
                throw new InvalidOperationException(
                    "YIUI预览失败：MatchWidthOrHeight必须在0到1之间，配置路径=" + YIUIConstHelper.YIUIConstAssetPath +
                    "，MatchWidthOrHeight=" + matchWidthOrHeight);
            }

            return new PreviewDesignSettings(width, height, matchWidthOrHeight);
        }

        private static Texture2D ResizeTexture(Texture2D source,
            int outputWidth, int outputHeight)
        {
            var previousActive = RenderTexture.active;
            RenderTexture outputRenderTexture = null;
            Texture2D outputTexture = null;
            try
            {
                outputRenderTexture = new RenderTexture(outputWidth,
                    outputHeight, 0, RenderTextureFormat.ARGB32,
                    RenderTextureReadWrite.sRGB)
                {
                    hideFlags = HideFlags.HideAndDontSave,
                    name = "YIUI_PrefabPreviewScaledRenderTexture"
                };
                outputRenderTexture.Create();
                Graphics.Blit(source, outputRenderTexture);

                RenderTexture.active = outputRenderTexture;
                outputTexture = new Texture2D(outputWidth, outputHeight,
                    TextureFormat.RGBA32, false, false);
                outputTexture.ReadPixels(new Rect(0f, 0f, outputWidth,
                    outputHeight), 0, 0, false);
                outputTexture.Apply(false, false);
                return outputTexture;
            }
            catch
            {
                if (outputTexture != null)
                {
                    DestroyImmediate(outputTexture);
                }

                throw;
            }
            finally
            {
                RenderTexture.active = previousActive;
                if (outputRenderTexture != null)
                {
                    outputRenderTexture.Release();
                    DestroyImmediate(outputRenderTexture);
                }
            }
        }

        private void CleanupPreview()
        {
            if (previewUtility != null)
            {
                previewUtility.Cleanup();
                previewUtility = null;
            }

            previewRoot = null;
            previewCanvas = null;
            previewPrefabInstance = null;
            previewEventSystem = null;
        }
    }
}

#endif
