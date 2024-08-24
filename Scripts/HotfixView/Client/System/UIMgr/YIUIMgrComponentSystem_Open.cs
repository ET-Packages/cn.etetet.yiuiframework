using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        /// <summary>
        /// 获取PanelInfo
        /// 没有则创建  相当于一个打开过了 UI基础配置档
        /// 这个根据BindVo创建  为什么没有直接用VO  因为里面有Panel 实例对象
        /// 这个k 根据resName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static PanelInfo GetPanelInfo<T>(this YIUIMgrComponent self) where T : Entity
        {
            var type = typeof(T);
            var name = type.Name;
            if (self.m_PanelCfgMap.TryGetValue(name, out var info))
            {
                return info;
            }

            var data = YIUIBindHelper.GetBindVoByType(type);
            if (data == null) return null;
            var vo = data.Value;

            if (vo.CodeType != EUICodeType.Panel)
            {
                Log.Error($"这个对象不是 Panel 无法打开 {typeof(T)}");
                return null;
            }

            self.m_PanelCfgMap.Add(name, new PanelInfo(vo));

            return self.m_PanelCfgMap[name];
        }

        /// <summary>
        /// 获取PanelInfo
        /// 没有则创建  相当于一个打开过了 UI基础配置档
        /// 这个根据BindVo创建  为什么没有直接用VO  因为里面有Panel 实例对象
        /// </summary>
        internal static PanelInfo GetPanelInfo(this YIUIMgrComponent self, string componentName)
        {
            if (self.m_PanelCfgMap.TryGetValue(componentName, out var info))
            {
                return info;
            }

            var resName = componentName.Replace("Component", "");
            var data    = YIUIBindHelper.GetBindVoByResName(resName);
            if (data == null) return null;
            var vo = data.Value;

            if (vo.CodeType != EUICodeType.Panel)
            {
                Log.Error($"这个对象不是 Panel 无法打开 {componentName}");
                return null;
            }

            self.m_PanelCfgMap.Add(componentName, new PanelInfo(vo));

            return self.m_PanelCfgMap[componentName];
        }

        /// <summary>
        /// 获取UI名称 用字符串开界面 不用类型 减少GC
        /// 另外也方便之后有可能需要的扩展 字符串会更好使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static string GetPanelName<T>(this YIUIMgrComponent self) where T : Entity
        {
            var panelInfo = self.GetPanelInfo<T>();
            return panelInfo?.Name;
        }

        [EnableAccessEntiyChild]
        internal static async ETTask<PanelInfo> OpenPanelStartAsync(this YIUIMgrComponent self, string panelName, Entity parentEntity)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogError($"<color=red> 无法打开 这是一个空名称 </color>");
                return null;
            }

            if (!self.m_PanelCfgMap.TryGetValue(panelName, out var info))
            {
                Debug.LogError($"请检查 {panelName} 没有获取到PanelInfo  1. panel上使用特性 [YIUI(typeof(XXPanelComponent))]  2. 检查是否没有注册上");
                return null;
            }

            var coroutineLockCode = info.PanelLayer == EPanelLayer.Panel ? YIUIConstHelper.Const.UIProjectName.GetHashCode() : panelName.GetHashCode();

            using var coroutineLock = await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.YIUIFramework, coroutineLockCode);

            #if YIUIMACRO_PANEL_OPENCLOSE
            Debug.Log($"<color=yellow> 打开UI: {panelName} </color>");
            #endif

            EventSystem.Instance?.Publish(self.Root(), new YIUIEventPanelOpenBefore { UIPkgName = info.PkgName, UIResName = info.ResName, UIComponentName = info.Name, PanelLayer = info.PanelLayer, });

            if (info.UIBase == null)
            {
                var uiCom = await YIUIFactory.CreatePanelAsync(info, parentEntity);
                if (uiCom == null)
                {
                    Debug.LogError($"面板[{panelName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                    return null;
                }

                var uiBase = uiCom.GetParent<YIUIChild>();
                uiBase.SetActive(false);
                info.ResetUI(uiBase);
                info.ResetEntity(uiCom);
            }
            else
            {
                info.UIBase.SetParent(parentEntity);
            }

            self.AddUI(info);

            return info;
        }

        /// <summary>
        /// 打开之前
        /// </summary>
        internal static async ETTask OpenPanelBefore(this YIUIMgrComponent self, PanelInfo info)
        {
            if (info.UIWindow is { WindowLastClose: false })
            {
                await self.AddUICloseElse(info);
            }
        }

        /// <summary>
        /// 打开之后
        /// </summary>
        internal static async ETTask OpenPanelAfter(this YIUIMgrComponent self, PanelInfo info, bool success)
        {
            if (success)
            {
                if (info.UIWindow is { WindowLastClose: true })
                {
                    await self.AddUICloseElse(info);
                }
            }
            else
            {
                #if YIUIMACRO_PANEL_OPENCLOSE
                Debug.Log($"<color=yellow> 打开UI失败: {info.ResName} </color>");
                #endif

                //如果打开失败直接屏蔽
                info?.UIBase?.SetActive(false);
                info?.UIPanel?.Close();
            }

            EventSystem.Instance?.Publish(
                self.Root(), new YIUIEventPanelOpenAfter
                {
                    Success = success, UIPkgName = info.PkgName, UIResName = info.ResName, UIComponentName = info.Name, PanelLayer = info.PanelLayer,
                });
        }

        internal static async ETTask<T> OpenPanelAsync<T>(this YIUIMgrComponent self, Entity root)
        where T : Entity, IAwake, IYIUIOpen
        {
            var info = await self.OpenPanelStartAsync(self.GetPanelName<T>(), root ?? self);
            if (info == null) return default;

            var success   = false;
            var component = (T)info.OwnerUIEntity;
            if (component == null)
            {
                Log.Error($"面板[{info.ResName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                return default;
            }

            await self.OpenPanelBefore(info);

            try
            {
                success = await info.UIPanel.Open();
            }
            catch (Exception e)
            {
                Debug.LogError($"Panel Open方法try报错={info.ResName}, 请检查={e.Message}{e.StackTrace}");
                return default;
            }

            await self.OpenPanelAfter(info, success);

            return success ? component : null;
        }

        internal static async ETTask<T> OpenPanelParamAsync<T>(this YIUIMgrComponent self, Entity root, params object[] paramMore)
        where T : Entity, IYIUIOpen<ParamVo>
        {
            var info = await self.OpenPanelStartAsync(self.GetPanelName<T>(), root ?? self);
            if (info == null) return default;

            var success   = false;
            var component = (T)info.OwnerUIEntity;
            if (component == null)
            {
                Log.Error($"面板[{info.ResName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                return default;
            }

            await self.OpenPanelBefore(info);

            var p = ParamVo.Get(paramMore);

            try
            {
                success = await info.UIPanel.Open(p);
            }
            catch (Exception e)
            {
                Debug.LogError($"Panel Open方法try报错={info.ResName}, 请检查={e.Message}{e.StackTrace}");
                return default;
            }

            await self.OpenPanelAfter(info, success);

            ParamVo.Put(p);

            return success ? component : null;
        }

        internal static async ETTask<T> OpenPanelAsync<T, P1>(this YIUIMgrComponent self, Entity root, P1 p1)
        where T : Entity, IYIUIOpen<P1>
        {
            var info = await self.OpenPanelStartAsync(self.GetPanelName<T>(), root ?? self);
            if (info == null) return default;

            var success   = false;
            var component = (T)info.OwnerUIEntity;
            if (component == null)
            {
                Log.Error($"面板[{info.ResName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                return default;
            }

            await self.OpenPanelBefore(info);

            try
            {
                success = await info.UIPanel.Open(p1);
            }
            catch (Exception e)
            {
                Debug.LogError($"Panel Open方法try报错={info.ResName}, 请检查={e.Message}{e.StackTrace}");
                return default;
            }

            await self.OpenPanelAfter(info, success);

            return success ? component : null;
        }

        internal static async ETTask<T> OpenPanelAsync<T, P1, P2>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2)
        where T : Entity, IYIUIOpen<P1, P2>
        {
            var info = await self.OpenPanelStartAsync(self.GetPanelName<T>(), root ?? self);
            if (info == null) return default;

            var success   = false;
            var component = (T)info.OwnerUIEntity;
            if (component == null)
            {
                Log.Error($"面板[{info.ResName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                return default;
            }

            await self.OpenPanelBefore(info);

            try
            {
                success = await info.UIPanel.Open(p1, p2);
            }
            catch (Exception e)
            {
                Debug.LogError($"Panel Open方法try报错={info.ResName}, 请检查={e.Message}{e.StackTrace}");
                return default;
            }

            await self.OpenPanelAfter(info, success);

            return success ? component : null;
        }

        internal static async ETTask<T> OpenPanelAsync<T, P1, P2, P3>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3)
        where T : Entity, IYIUIOpen<P1, P2, P3>
        {
            var info = await self.OpenPanelStartAsync(self.GetPanelName<T>(), root ?? self);
            if (info == null) return default;

            var success   = false;
            var component = (T)info.OwnerUIEntity;
            if (component == null)
            {
                Log.Error($"面板[{info.ResName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                return default;
            }

            await self.OpenPanelBefore(info);

            try
            {
                success = await info.UIPanel.Open(p1, p2, p3);
            }
            catch (Exception e)
            {
                Debug.LogError($"Panel Open方法try报错={info.ResName}, 请检查={e.Message}{e.StackTrace}");
                return default;
            }

            await self.OpenPanelAfter(info, success);

            return success ? component : default;
        }

        internal static async ETTask<T> OpenPanelAsync<T, P1, P2, P3, P4>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3, P4 p4)
        where T : Entity, IYIUIOpen<P1, P2, P3, P4>
        {
            var info = await self.OpenPanelStartAsync(self.GetPanelName<T>(), root ?? self);
            if (info == null) return default;

            var success   = false;
            var component = (T)info.OwnerUIEntity;
            if (component == null)
            {
                Log.Error($"面板[{info.ResName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                return default;
            }

            await self.OpenPanelBefore(info);

            try
            {
                success = await info.UIPanel.Open(p1, p2, p3, p4);
            }
            catch (Exception e)
            {
                Debug.LogError($"Panel Open方法try报错={info.ResName}, 请检查={e.Message}{e.StackTrace}");
                return default;
            }

            await self.OpenPanelAfter(info, success);

            return success ? component : null;
        }

        internal static async ETTask<T> OpenPanelAsync<T, P1, P2, P3, P4, P5>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        where T : Entity, IYIUIOpen<P1, P2, P3, P4, P5>
        {
            var info = await self.OpenPanelStartAsync(self.GetPanelName<T>(), root ?? self);
            if (info == null) return default;

            var success   = false;
            var component = (T)info.OwnerUIEntity;
            if (component == null)
            {
                Log.Error($"面板[{info.ResName}]没有创建成功，packName={info.PkgName}, resName={info.ResName}");
                return default;
            }

            await self.OpenPanelBefore(info);

            try
            {
                success = await info.UIPanel.Open(p1, p2, p3, p4, p5);
            }
            catch (Exception e)
            {
                Debug.LogError($"Panel Open方法try报错={info.ResName}, 请检查={e.Message}{e.StackTrace}");
                return default;
            }

            await self.OpenPanelAfter(info, success);

            return success ? component : null;
        }
    }
}
