﻿namespace ET.Client
{
    public static partial class YIUIRootComponentSystem
    {
        public static async ETTask<HashWaitError> OpenPanelWaitAsync(this YIUIRootComponent self, string componentName)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitParamAsync(this YIUIRootComponent self, string componentName, params object[] paramMore)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitParamAsync(componentName, self, paramMore);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1>(this YIUIRootComponent self, string componentName, P1 p1)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2, p3);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2, p3, p4);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4, P5>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2, p3, p4, p5);
        }
    }
}
