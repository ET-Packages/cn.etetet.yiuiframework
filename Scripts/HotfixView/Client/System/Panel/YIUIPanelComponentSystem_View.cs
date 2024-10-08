using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof(YIUIViewComponent))]
    public static partial class YIUIPanelComponentSystem
    {
        internal static void InitPanelViewData(this YIUIPanelComponent self)
        {
            self.m_ExistView.Clear();
            self.m_ViewParent.Clear();
            self.m_PanelSplitData = self.UIBase.CDETable.PanelSplitData;
            self.CreateCommonView();
            self.AddViewParent(self.m_PanelSplitData.AllCommonView);
            self.AddViewParent(self.m_PanelSplitData.AllCreateView);
            self.AddViewParent(self.m_PanelSplitData.AllPopupView);
        }

        private static void AddViewParent(this YIUIPanelComponent self, List<RectTransform> listParent)
        {
            foreach (var parent in listParent)
            {
                var viewName = parent.name.Replace(YIUIConstHelper.Const.UIParentName, "");
                self.m_ViewParent.Add(viewName, parent);
            }
        }

        private static void CreateCommonView(this YIUIPanelComponent self)
        {
            foreach (var commonParentView in self.m_PanelSplitData.AllCommonView)
            {
                var viewName = commonParentView.name.Replace(YIUIConstHelper.Const.UIParentName, "");

                //通用view的名称是不允许修改的 如果修改了 那么就创建一个新的
                var viewTsf = commonParentView.FindChildByName(viewName);
                if (viewTsf == null)
                {
                    Debug.LogError($"{viewName} 当前通用View 不存在于父级下 所以无法自动创建 将会动态创建");
                    continue;
                }

                var data = YIUIBindHelper.GetBindVoByResName(viewName);
                if (data == null) return;
                var vo = data.Value;
                if (vo.CodeType != EUICodeType.View)
                {
                    Log.Error($"打开错误必须是一个view类型");
                    return;
                }

                //通用创建 这个时候通用UI一定是没有创建的 否则就有问题
                var view = YIUIFactory.CreateByObjVo(vo, viewTsf.gameObject, self.UIBase.OwnerUIEntity);

                if (view != null)
                {
                    viewTsf.gameObject.SetActive(false);
                    self.m_ExistView.Add(viewName, view);
                }
            }
        }

        private static RectTransform GetViewParent(this YIUIPanelComponent self, string viewName)
        {
            self.m_ViewParent.TryGetValue(viewName, out var value);
            return value;
        }

        internal static async ETTask<Entity> GetView(this YIUIPanelComponent self, string resName)
        {
            if (self.UIBase.OwnerUIEntity == null)
            {
                Log.Error($"没有找到ET UI组件");
                return null;
            }

            using var coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUIFramework, resName.GetHashCode());

            var data = YIUIBindHelper.GetBindVoByResName(resName);
            if (data == null) return null;
            var vo = data.Value;
            if (vo.CodeType != EUICodeType.View)
            {
                Log.Error($"打开错误必须是一个view类型");
                return null;
            }

            var viewParent = self.GetViewParent(resName);
            if (viewParent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {resName}");
                return null;
            }

            if (self.m_ExistView.TryGetValue(resName, out var baseView))
            {
                return baseView;
            }

            var view = await YIUIFactory.InstantiateAsync(vo, self.UIBase.OwnerUIEntity, viewParent);

            self.m_ExistView.Add(resName, view);
            return view;
        }

        internal static async ETTask<Entity> GetView<T>(this YIUIPanelComponent self) where T : Entity
        {
            if (self.UIBase.OwnerUIEntity == null)
            {
                Log.Error($"没有找到ET UI组件");
                return null;
            }

            using var coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUIFramework, typeof(T).GetHashCode());

            var data = YIUIBindHelper.GetBindVoByType<T>();
            if (data == null) return null;
            var vo = data.Value;
            if (vo.CodeType != EUICodeType.View)
            {
                Log.Error($"打开错误必须是一个view类型");
                return null;
            }

            var viewName   = vo.ResName;
            var viewParent = self.GetViewParent(viewName);
            if (viewParent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return null;
            }

            if (self.m_ExistView.TryGetValue(viewName, out var baseView))
            {
                return baseView;
            }

            var view = await YIUIFactory.InstantiateAsync(vo, self.UIBase.OwnerUIEntity, viewParent);

            self.m_ExistView.Add(viewName, view);
            return view;
        }

        public static (bool, Entity) ExistView<T>(this YIUIPanelComponent self) where T : Entity
        {
            if (self.UIBase.OwnerUIEntity == null)
            {
                Log.Error($"没有找到ET UI组件");
                return (false, null);
            }

            var data = YIUIBindHelper.GetBindVoByType<T>();
            if (data == null) return (false, null);
            var vo = data.Value;
            if (vo.CodeType != EUICodeType.View)
            {
                Log.Error($"打开错误必须是一个view类型");
                return (false, null);
            }

            var viewName   = vo.ResName;
            var viewParent = self.GetViewParent(viewName);
            if (viewParent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {viewName}");
                return (false, null);
            }

            if (self.m_ExistView.TryGetValue(viewName, out var baseView))
            {
                return (true, baseView);
            }

            return (false, null);
        }

        public static (bool, Entity) ExistView(this YIUIPanelComponent self, string resName)
        {
            if (self.UIBase.OwnerUIEntity == null)
            {
                Log.Error($"没有找到ET UI组件");
                return (false, null);
            }

            var data = YIUIBindHelper.GetBindVoByResName(resName);
            if (data == null) return (false, null);
            var vo = data.Value;
            if (vo.CodeType != EUICodeType.View)
            {
                Log.Error($"打开错误必须是一个view类型");
                return (false, null);
            }

            var viewParent = self.GetViewParent(resName);
            if (viewParent == null)
            {
                Debug.LogError($"不存在这个View  请检查 {resName}");
                return (false, null);
            }

            if (self.m_ExistView.TryGetValue(resName, out var baseView))
            {
                return (true, baseView);
            }

            return (false, null);
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        internal static async ETTask OpenViewBefore(this YIUIPanelComponent self, Entity view)
        {
            if (!view.GetParent<YIUIChild>().GetComponent<YIUIWindowComponent>().WindowFirstOpen)
            {
                await self.CloseLastView(view);
            }
        }

        /// <summary>
        /// 打开之后
        /// </summary>
        internal static async ETTask OpenViewAfter(this YIUIPanelComponent self, Entity view, bool success)
        {
            if (success)
            {
                if (view.GetParent<YIUIChild>().GetComponent<YIUIWindowComponent>().WindowFirstOpen)
                {
                    await self.CloseLastView(view);
                }
            }
            else
            {
                view.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>().Close(false);
            }
        }

        /// <summary>
        /// 关闭上一个
        /// </summary>
        private static async ETTask CloseLastView(this YIUIPanelComponent self, Entity view)
        {
            //其他需要被忽略 Panel下的view 如果是窗口类型 那么他只能同时存在一个  弹窗层可以存在多个
            if (view.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>().ViewWindowType != EViewWindowType.View)
            {
                return;
            }

            //View只有切换没有关闭
            var skipTween = view.GetParent<YIUIChild>().GetComponent<YIUIWindowComponent>().WindowSkipOtherCloseTween;

            if (self.CurrentOpenView != null && self.CurrentOpenView != view && self.CurrentOpenViewActiveSelf)
            {
                var uiBase = self.CurrentOpenView.GetParent<YIUIChild>();

                var tween = true;
                var skipClose = false;
                //View 没有自动回退功能  比如AView 关闭 自动吧上一个BView 给打开 没有这种需求 也不能有这个需求
                //只能有 打开一个新View 上一个View的自动处理 99% 都是吧上一个隐藏即可
                //外部就只需要关心 打开 A B C 即可
                //因为这是View  不是 Panel
                //如果你想要 先打开了 A B C  然后关闭 C 自动回退到B 关闭B又回退到A 那么你就需要自己实现
                //因为这是View  不是 Panel
                switch (uiBase.GetComponent<YIUIViewComponent>().StackOption)
                {
                    case EViewStackOption.None:
                        skipClose = true;
                        break;
                    case EViewStackOption.Visible:
                        tween = false;
                        break;
                    case EViewStackOption.VisibleTween:
                        tween = !skipTween;
                        break;
                    default:
                        Debug.LogError($"新增类型未实现 {uiBase.GetComponent<YIUIViewComponent>().StackOption}");
                        tween = false;
                        break;
                }

                if (!skipClose)
                {
                    await self.CurrentOpenView.GetParent<YIUIChild>().GetComponent<YIUIViewComponent>().CloseAsync(tween);
                }
            }

            self.u_CurrentOpenView = view;
        }

        /// <summary>
        /// Panel被关闭前需要触发关闭所有View
        /// </summary>
        internal static async ETTask<bool> CloseAllView(this YIUIPanelComponent self, bool tween = true)
        {
            foreach (Entity view in self.m_ExistView.Values)
            {
                var uibase        = view.GetParent<YIUIChild>();
                var viewComponent = uibase?.GetComponent<YIUIViewComponent>();
                if (viewComponent != null && uibase is { ActiveSelf: true })
                {
                    await viewComponent.CloseAsync(tween);
                }
            }

            return true;
        }
    }
}