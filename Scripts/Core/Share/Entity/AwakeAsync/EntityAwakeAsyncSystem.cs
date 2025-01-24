using System;

namespace ET
{
    public static class EntityAwakeAsyncSystem
    {
        public static async ETTask AwakeAsync(this EntitySystemSingleton self, Entity component)
        {
            if (component is not IAwakeAsync)
            {
                return;
            }

            var iAwakeSystems = self.TypeSystems.GetSystems(component.GetType(), typeof(IAwakeAsyncSystem));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeAsyncSystem aAwakeAsyncSystem in iAwakeSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static async ETTask AwakeAsync<P1>(this EntitySystemSingleton self, Entity component, P1 p1)
        {
            if (component is not IAwakeAsync<P1>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IAwakeAsyncSystem<P1>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            foreach (IAwakeAsyncSystem<P1> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(component, p1);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static async ETTask AwakeAsync<P1, P2>(this EntitySystemSingleton self, Entity component, P1 p1, P2 p2)
        {
            if (component is not IAwakeAsync<P1, P2>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IAwakeAsyncSystem<P1, P2>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            foreach (IAwakeAsyncSystem<P1, P2> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(component, p1, p2);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static async ETTask AwakeAsync<P1, P2, P3>(this EntitySystemSingleton self, Entity component, P1 p1, P2 p2, P3 p3)
        {
            if (component is not IAwakeAsync<P1, P2, P3>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IAwakeAsyncSystem<P1, P2, P3>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            foreach (IAwakeAsyncSystem<P1, P2, P3> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(component, p1, p2, p3);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static async ETTask AwakeAsync<P1, P2, P3, P4>(this EntitySystemSingleton self, Entity component, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            if (component is not IAwakeAsync<P1, P2, P3, P4>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IAwakeAsyncSystem<P1, P2, P3, P4>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            foreach (IAwakeAsyncSystem<P1, P2, P3, P4> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(component, p1, p2, p3, p4);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public static async ETTask AwakeAsync<P1, P2, P3, P4, P5>(this EntitySystemSingleton self, Entity component, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            if (component is not IAwakeAsync<P1, P2, P3, P4, P5>)
            {
                return;
            }

            var iAwakeAsyncSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IAwakeAsyncSystem<P1, P2, P3, P4, P5>));
            if (iAwakeAsyncSystems == null)
            {
                return;
            }

            foreach (IAwakeAsyncSystem<P1, P2, P3, P4, P5> aAwakeAsyncSystem in iAwakeAsyncSystems)
            {
                if (aAwakeAsyncSystem == null)
                {
                    continue;
                }

                try
                {
                    await aAwakeAsyncSystem.Run(component, p1, p2, p3, p4, p5);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}