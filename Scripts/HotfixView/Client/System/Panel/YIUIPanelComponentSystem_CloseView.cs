using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIPanelComponentSystem
    {
        public static void CloseView<T>(this YIUIPanelComponent self, bool tween = true) where T : Entity
        {
            self.CloseViewAsync<T>(tween).NoContext();
        }

        public static void CloseView(this YIUIPanelComponent self, string resName, bool tween = true)
        {
            self.CloseViewAsync(resName, tween).NoContext();
        }

        public static async ETTask<bool> CloseViewAsync<T>(this YIUIPanelComponent self, bool tween = true) where T : Entity
        {
            var (exist, entity) = self.ExistView<T>();
            if (!exist) return false;
            return await CloseViewAsync(entity, tween);
        }

        public static async ETTask<bool> CloseViewAsync(this YIUIPanelComponent self, string resName, bool tween = true)
        {
            var (exist, entity) = self.ExistView(resName);
            if (!exist) return false;
            return await CloseViewAsync(entity, tween);
        }

        private static async ETTask<bool> CloseViewAsync(Entity entity, bool tween)
        {
            var uibase = entity.GetParent<YIUIChild>();
            if (uibase == null) return false;

            var viewComponent = uibase.GetComponent<YIUIViewComponent>(true);
            if (viewComponent == null) return false;

            return await viewComponent.CloseAsync(tween);
        }
    }
}