using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    /// <summary>
    /// UI相对位置助手
    /// </summary>
    public static class YIUIRelativePositionHelper
    {
        /// <summary>
        /// 获取屏幕位置
        /// </summary>
        /// <param name="args">参数</param>
        /// <param name="autoSetWindowPosition">是否自动设置位置</param>
        /// <returns></returns>
        public static Vector2 GetWindowScreenPosition(YIUIRelativePositionParam args, bool autoSetWindowPosition = true)
        {
            if (args.WindowRectTransform == null)
            {
                return GetScreenCenter();
            }

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(args.WindowRectTransform);

            var menuRect = GetScreenRect(args.WindowRectTransform, default);
            var screenCenter = GetScreenCenter();
            if (menuRect.width <= 0 || menuRect.height <= 0)
            {
                if (autoSetWindowPosition)
                {
                    SetWindowScreenPosition(args.WindowRectTransform, screenCenter);
                }

                return screenCenter;
            }

            if (args.BlockRectTransform == null)
            {
                if (autoSetWindowPosition)
                {
                    SetWindowScreenPosition(args.WindowRectTransform, screenCenter);
                }

                return screenCenter;
            }

            var blockRect = GetScreenRect(args.BlockRectTransform, args.BlockRectOffset);
            var menuSize = new Vector2(menuRect.width, menuRect.height);
            foreach (var direction in GetOpenDirectionOrder(args.PreferredDirection))
            {
                if (!TryGetPlacementScreenCenter(blockRect, menuSize, direction, out screenCenter))
                {
                    continue;
                }

                if (autoSetWindowPosition)
                {
                    SetWindowScreenPosition(args.WindowRectTransform, screenCenter);
                }

                return screenCenter;
            }

            screenCenter = GetScreenCenter();
            if (autoSetWindowPosition)
            {
                SetWindowScreenPosition(args.WindowRectTransform, screenCenter);
            }

            return screenCenter;
        }

        /// <summary>
        /// 根据已知位置设置
        /// </summary>
        public static void SetWindowScreenPosition(RectTransform windowRectTransform, Vector2 screenPosition)
        {
            if (windowRectTransform == null)
            {
                return;
            }

            var parentRect = windowRectTransform.parent as RectTransform;
            if (parentRect == null)
            {
                windowRectTransform.anchoredPosition = Vector2.zero;
                return;
            }

            var camera = GetUICamera(windowRectTransform);
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPosition, camera, out var localPoint))
            {
                windowRectTransform.anchoredPosition = Vector2.zero;
                return;
            }

            windowRectTransform.anchoredPosition = localPoint;
        }

        private static Rect GetScreenRect(RectTransform rectTransform, YIUIRelativePositionRectOffset offset)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var camera = GetUICamera(rectTransform);
            var leftBottom = RectTransformUtility.WorldToScreenPoint(camera, corners[0]);
            var leftTop = RectTransformUtility.WorldToScreenPoint(camera, corners[1]);
            var rightTop = RectTransformUtility.WorldToScreenPoint(camera, corners[2]);
            var rightBottom = RectTransformUtility.WorldToScreenPoint(camera, corners[3]);

            var minX = Mathf.Min(leftBottom.x, leftTop.x, rightTop.x, rightBottom.x) - offset.Left;
            var maxX = Mathf.Max(leftBottom.x, leftTop.x, rightTop.x, rightBottom.x) + offset.Right;
            var minY = Mathf.Min(leftBottom.y, leftTop.y, rightTop.y, rightBottom.y) - offset.Bottom;
            var maxY = Mathf.Max(leftBottom.y, leftTop.y, rightTop.y, rightBottom.y) + offset.Top;
            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }

        private static bool TryGetPlacementScreenCenter(Rect blockRect,
                                                        Vector2 windowSize,
                                                        EYIUIRelativeOpenDirection direction,
                                                        out Vector2 screenCenter)
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            var halfWidth = windowSize.x * 0.5f;
            var halfHeight = windowSize.y * 0.5f;
            var anchorCenter = blockRect.center;
            var isUpperHalf = anchorCenter.y >= screenHeight * 0.5f;
            var isRightHalf = anchorCenter.x >= screenWidth * 0.5f;

            switch (direction)
            {
                case EYIUIRelativeOpenDirection.Right:
                {
                    if (blockRect.xMax + windowSize.x > screenWidth)
                    {
                        screenCenter = default;
                        return false;
                    }

                    var y = isUpperHalf ? blockRect.yMax - halfHeight : blockRect.yMin + halfHeight;
                    screenCenter = new Vector2(blockRect.xMax + halfWidth, Mathf.Clamp(y, halfHeight, screenHeight - halfHeight));
                    return true;
                }
                case EYIUIRelativeOpenDirection.Left:
                {
                    if (blockRect.xMin - windowSize.x < 0)
                    {
                        screenCenter = default;
                        return false;
                    }

                    var y = isUpperHalf ? blockRect.yMax - halfHeight : blockRect.yMin + halfHeight;
                    screenCenter = new Vector2(blockRect.xMin - halfWidth, Mathf.Clamp(y, halfHeight, screenHeight - halfHeight));
                    return true;
                }
                case EYIUIRelativeOpenDirection.Up:
                {
                    if (blockRect.yMax + windowSize.y > screenHeight)
                    {
                        screenCenter = default;
                        return false;
                    }

                    var x = isRightHalf ? blockRect.xMax - halfWidth : blockRect.xMin + halfWidth;
                    screenCenter = new Vector2(Mathf.Clamp(x, halfWidth, screenWidth - halfWidth), blockRect.yMax + halfHeight);
                    return true;
                }
                case EYIUIRelativeOpenDirection.Down:
                {
                    if (blockRect.yMin - windowSize.y < 0)
                    {
                        screenCenter = default;
                        return false;
                    }

                    var x = isRightHalf ? blockRect.xMax - halfWidth : blockRect.xMin + halfWidth;
                    screenCenter = new Vector2(Mathf.Clamp(x, halfWidth, screenWidth - halfWidth), blockRect.yMin - halfHeight);
                    return true;
                }
                default:
                {
                    screenCenter = default;
                    return false;
                }
            }
        }

        private static EYIUIRelativeOpenDirection[] GetOpenDirectionOrder(EYIUIRelativeOpenDirection preferredDirection)
        {
            return preferredDirection switch
            {
                EYIUIRelativeOpenDirection.Right => new[]
                {
                    EYIUIRelativeOpenDirection.Right,
                    EYIUIRelativeOpenDirection.Left,
                    EYIUIRelativeOpenDirection.Down,
                    EYIUIRelativeOpenDirection.Up,
                },
                EYIUIRelativeOpenDirection.Left => new[]
                {
                    EYIUIRelativeOpenDirection.Left,
                    EYIUIRelativeOpenDirection.Right,
                    EYIUIRelativeOpenDirection.Down,
                    EYIUIRelativeOpenDirection.Up,
                },
                EYIUIRelativeOpenDirection.Up => new[]
                {
                    EYIUIRelativeOpenDirection.Up,
                    EYIUIRelativeOpenDirection.Down,
                    EYIUIRelativeOpenDirection.Right,
                    EYIUIRelativeOpenDirection.Left,
                },
                EYIUIRelativeOpenDirection.Down => new[]
                {
                    EYIUIRelativeOpenDirection.Down,
                    EYIUIRelativeOpenDirection.Up,
                    EYIUIRelativeOpenDirection.Right,
                    EYIUIRelativeOpenDirection.Left,
                },
                _ => new[]
                {
                    EYIUIRelativeOpenDirection.Right,
                    EYIUIRelativeOpenDirection.Left,
                    EYIUIRelativeOpenDirection.Down,
                    EYIUIRelativeOpenDirection.Up,
                },
            };
        }

        private static Vector2 GetScreenCenter()
        {
            return new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        }

        private static Camera GetUICamera(Component component)
        {
            var canvas = component.GetComponentInParent<Canvas>();
            if (canvas == null)
            {
                return null;
            }

            return canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        }
    }
}