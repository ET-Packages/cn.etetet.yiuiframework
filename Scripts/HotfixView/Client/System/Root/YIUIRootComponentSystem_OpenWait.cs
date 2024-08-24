using YIUIFramework;

namespace ET.Client
{
    public static partial class YIUIRootComponentSystem
    {
        public static async ETTask<HashWaitError> OpenPanelWaitAsync<T>(this YIUIRootComponent self, long timeout = 0)
        where T : Entity, IAwake, IYIUIOpen
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync<T>(self, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitParamAsync<T>(this YIUIRootComponent self, long timeout = 0, params object[] paramMore)
        where T : Entity, IYIUIOpen<ParamVo>
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitParamAsync<T>(self, timeout, paramMore);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1>(this YIUIRootComponent self, P1 p1, long timeout = 0)
        where T : Entity, IYIUIOpen<P1>
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync<T, P1>(self, p1, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2>(this YIUIRootComponent self, P1 p1, P2 p2, long timeout = 0)
        where T : Entity, IYIUIOpen<P1, P2>
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync<T, P1, P2>(self, p1, p2, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2, P3>(this YIUIRootComponent self, P1 p1, P2 p2, P3 p3, long timeout = 0)
        where T : Entity, IYIUIOpen<P1, P2, P3>
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync<T, P1, P2, P3>(self, p1, p2, p3, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4>(this YIUIRootComponent self, P1 p1, P2 p2, P3 p3, P4 p4, long timeout = 0)
        where T : Entity, IYIUIOpen<P1, P2, P3, P4>
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync<T, P1, P2, P3, P4>(self, p1, p2, p3, p4, timeout);
        }

        public static async ETTask<HashWaitError> OpenPanelWaitAsync<T, P1, P2, P3, P4, P5>(this YIUIRootComponent self, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, long timeout = 0)
        where T : Entity, IYIUIOpen<P1, P2, P3, P4, P5>
        {
            return await YIUIMgrComponent.Inst.OpenPanelWaitAsync<T, P1, P2, P3, P4, P5>(self, p1, p2, p3, p4, p5, timeout);
        }
    }
}
