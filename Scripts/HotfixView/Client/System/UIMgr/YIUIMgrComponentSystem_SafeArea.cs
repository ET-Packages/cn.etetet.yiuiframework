using System;
using UnityEngine;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        internal static void InitSafeArea(this YIUIMgrComponent self)
        {
            var safeAreaX = Math.Max(Screen.safeArea.x, Screen.width - Screen.safeArea.xMax);
            var safeAreaY = Math.Max(Screen.safeArea.y, Screen.height - Screen.safeArea.yMax);

            #if UNITY_EDITOR

            //这里可调试 
            //safeAreaX = 100;
            //safeAreaY = 100;
            #endif

            YIUIMgrComponent.g_SafeArea = new Rect(safeAreaX,
                                                   safeAreaY,
                                                   YIUIMgrComponent.DesignScreenWidth_F - self.GetSafeValue(safeAreaX),
                                                   YIUIMgrComponent.DesignScreenHeight_F - self.GetSafeValue(safeAreaY));

            self.InitUISafeArea();
        }

        private static float GetSafeValue(this YIUIMgrComponent self, float safeValue)
        {
            return YIUIMgrComponent.DoubleSafe ? safeValue * 2 : safeValue;
        }

        private static void InitUISafeArea(this YIUIMgrComponent self)
        {
            self.UILayerRoot.anchoredPosition = new Vector2(YIUIMgrComponent.g_SafeArea.x, -YIUIMgrComponent.g_SafeArea.y);
            if (YIUIMgrComponent.DoubleSafe)
            {
                self.UILayerRoot.offsetMax = new Vector2(-YIUIMgrComponent.g_SafeArea.x, self.UILayerRoot.offsetMax.y);
                self.UILayerRoot.offsetMin = new Vector2(self.UILayerRoot.offsetMin.x, YIUIMgrComponent.g_SafeArea.y);
            }
            else
            {
                //TODO 单边时需要考虑手机是左还是右
                self.UILayerRoot.offsetMax = new Vector2(0, self.UILayerRoot.offsetMax.y);
                self.UILayerRoot.offsetMin = new Vector2(self.UILayerRoot.offsetMin.x, 0);
            }
        }
    }
}
