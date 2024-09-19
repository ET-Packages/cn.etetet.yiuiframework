using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIMgrComponentSystem
    {
        internal static async ETTask<HashWaitError> OpenPanelWaitAsync(this YIUIMgrComponent self, string componentName, Entity root)
        {
            var panel = await self.OpenPanelAsync(componentName, root);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitParamAsync(this YIUIMgrComponent self, string componentName, Entity root, params object[] paramMore)
        {
            var panel = await self.OpenPanelParamAsync(componentName, root, paramMore);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3, p4);
            return await PanelWait(panel);
        }

        internal static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4, P5>(this YIUIMgrComponent self, string componentName, Entity root, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            var panel = await self.OpenPanelAsync(componentName, root, p1, p2, p3, p4, p5);
            return await PanelWait(panel);
        }
    }
}
