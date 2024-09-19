using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        private static async ETTask<HashWaitError> PanelWait(Entity panel)
        {
            if (panel == null) return HashWaitError.Error;
            var guid            = IdGenerater.Instance.GenerateId();
            var hashWait        = YIUIMgrComponent.Inst.Root.GetComponent<HashWait>().Wait(guid);
            var windowComponent = panel.GetParent<YIUIChild>().GetComponent<YIUIWindowComponent>();
            var waitComponent   = windowComponent.GetComponent<YIUIWaitComponent>();
            if (waitComponent == null)
            {
                waitComponent = windowComponent.AddComponent<YIUIWaitComponent, long>(guid);
            }
            else
            {
                waitComponent.Reset(guid);
            }

            return await hashWait;
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<T>(this YIUIMgrComponent self, Entity root)
        where T : Entity, IAwake, IYIUIOpen
        {
            var panel = await self.OpenPanelAsync<T>(root);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitParamAsync<T>(this YIUIMgrComponent self, Entity root, params object[] paramMore)
        where T : Entity, IYIUIOpen<ParamVo>
        {
            var panel = await self.OpenPanelParamAsync<T>(root, paramMore);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1>(this YIUIMgrComponent self, Entity root, P1 p1)
        where T : Entity, IYIUIOpen<P1>
        {
            var panel = await self.OpenPanelAsync<T, P1>(root, p1);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2)
        where T : Entity, IYIUIOpen<P1, P2>
        {
            var panel = await self.OpenPanelAsync<T, P1, P2>(root, p1, p2);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2, P3>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3)
        where T : Entity, IYIUIOpen<P1, P2, P3>
        {
            var panel = await self.OpenPanelAsync<T, P1, P2, P3>(root, p1, p2, p3);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3, P4 p4)
        where T : Entity, IYIUIOpen<P1, P2, P3, P4>
        {
            var panel = await self.OpenPanelAsync<T, P1, P2, P3, P4>(root, p1, p2, p3, p4);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4, P5>(this YIUIMgrComponent self, Entity root, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        where T : Entity, IYIUIOpen<P1, P2, P3, P4, P5>
        {
            var panel = await self.OpenPanelAsync<T, P1, P2, P3, P4, P5>(root, p1, p2, p3, p4, p5);
            return await PanelWait(panel);
        }
    }
}
