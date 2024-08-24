namespace ET.Client
{
    public static partial class YIUIRootComponentSystem
    {
        public static async ETTask<HashWaitError> OpenPanelWaitAsync(this YIUIRootComponent self, string componentName, long timeout = 0)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitParamAsync(this YIUIRootComponent self, string componentName, long timeout = 0, params object[] paramMore)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitParamAsync(componentName, self, timeout, paramMore);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1>(this YIUIRootComponent self, string componentName, P1 p1, long timeout = 0)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2, long timeout = 0)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3, long timeout = 0)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2, p3, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3, P4 p4, long timeout = 0)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2, p3, p4, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<P1, P2, P3, P4, P5>(this YIUIRootComponent self, string componentName, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, long timeout = 0)
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync(componentName, self, p1, p2, p3, p4, p5, timeout);
        }
    }
}
