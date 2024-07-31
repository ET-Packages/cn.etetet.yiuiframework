using System;

namespace ET
{
    public interface IYIUITaskEventInvokeSystem : ISystemType
    {
        ETTask Invoke(Entity o);
    }

    public interface IYIUITaskEventInvokeSystem<in P1> : ISystemType
    {
        ETTask Invoke(Entity o, P1 p1);
    }

    public interface IYIUITaskEventInvokeSystem<in P1, in P2> : ISystemType
    {
        ETTask Invoke(Entity o, P1 p1, P2 p2);
    }

    public interface IYIUITaskEventInvokeSystem<in P1, in P2, in P3> : ISystemType
    {
        ETTask Invoke(Entity o, P1 p1, P2 p2, P3 p3);
    }

    public interface IYIUITaskEventInvokeSystem<in P1, in P2, in P3, in P4> : ISystemType
    {
        ETTask Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4);
    }

    public interface IYIUITaskEventInvokeSystem<in P1, in P2, in P3, in P4, in P5> : ISystemType
    {
        ETTask Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }

    [EntitySystem]
    public abstract class YIUITaskEventInvokeSystem<T> : SystemObject, IYIUITaskEventInvokeSystem where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUITaskEventInvokeSystem);
        }

        public async ETTask Invoke(Entity o)
        {
            await Invoke((T)o);
        }

        protected abstract ETTask Invoke(T self);
    }

    [EntitySystem]
    public abstract class YIUITaskEventInvokeSystem<T, P1> : SystemObject, IYIUITaskEventInvokeSystem<P1> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUITaskEventInvokeSystem<P1>);
        }

        public async ETTask Invoke(Entity o, P1 p1)
        {
            await Invoke((T)o, p1);
        }

        protected abstract ETTask Invoke(T self, P1 p1);
    }

    [EntitySystem]
    public abstract class YIUITaskEventInvokeSystem<T, P1, P2> : SystemObject, IYIUITaskEventInvokeSystem<P1, P2> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUITaskEventInvokeSystem<P1, P2>);
        }

        public async ETTask Invoke(Entity o, P1 p1, P2 p2)
        {
            await Invoke((T)o, p1, p2);
        }

        protected abstract ETTask Invoke(T self, P1 p1, P2 p2);
    }

    [EntitySystem]
    public abstract class YIUITaskEventInvokeSystem<T, P1, P2, P3> : SystemObject, IYIUITaskEventInvokeSystem<P1, P2, P3> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUITaskEventInvokeSystem<P1, P2, P3>);
        }

        public async ETTask Invoke(Entity o, P1 p1, P2 p2, P3 p3)
        {
            await Invoke((T)o, p1, p2, p3);
        }

        protected abstract ETTask Invoke(T self, P1 p1, P2 p2, P3 p3);
    }

    [EntitySystem]
    public abstract class YIUITaskEventInvokeSystem<T, P1, P2, P3, P4> : SystemObject, IYIUITaskEventInvokeSystem<P1, P2, P3, P4> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUITaskEventInvokeSystem<P1, P2, P3, P4>);
        }

        public async ETTask Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            await Invoke((T)o, p1, p2, p3, p4);
        }

        protected abstract ETTask Invoke(T self, P1 p1, P2 p2, P3 p3, P4 p4);
    }

    [EntitySystem]
    public abstract class YIUITaskEventInvokeSystem<T, P1, P2, P3, P4, P5> : SystemObject, IYIUITaskEventInvokeSystem<P1, P2, P3, P4, P5> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUITaskEventInvokeSystem<P1, P2, P3, P4, P5>);
        }

        public async ETTask Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            await Invoke((T)o, p1, p2, p3, p4, p5);
        }

        protected abstract ETTask Invoke(T self, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }
}
