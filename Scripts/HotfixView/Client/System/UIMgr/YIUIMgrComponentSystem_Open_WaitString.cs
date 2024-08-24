using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        internal static async ETTask<HashWaitError> OpenPanelWaitAsync(this YIUIMgrComponent self, string componentName, Entity root, long timeout = 0)
        {
            var panel = await self.OpenPanelAsync(componentName, root);
            return await PanelWait(panel, timeout);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitParamAsync(this YIUIMgrComponent self, string componentName, Entity root, long timeout = 0, params object[] paramMore)
        {
            var panel = await self.OpenPanelParamAsync(componentName, root, paramMore);
            return await PanelWait(panel, timeout);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, long timeout = 0)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1);
            return await PanelWait(panel, timeout);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, long timeout = 0)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2);
            return await PanelWait(panel, timeout);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3, long timeout = 0)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3);
            return await PanelWait(panel, timeout);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3, P4 p4, long timeout = 0)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3, p4);
            return await PanelWait(panel, timeout);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4, P5>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, long timeout = 0)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3, p4, p5);
            return await PanelWait(panel, timeout);
        }
    }
}
