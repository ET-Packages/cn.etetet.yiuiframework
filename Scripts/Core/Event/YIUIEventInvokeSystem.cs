using System;

namespace ET
{
    public interface IYIUIEventInvokeSystem : ISystemType
    {
        void Invoke(Entity o);
    }

    public interface IYIUIEventInvokeSystem<in P1> : ISystemType
    {
        void Invoke(Entity o, P1 p1);
    }

    public interface IYIUIEventInvokeSystem<in P1, in P2> : ISystemType
    {
        void Invoke(Entity o, P1 p1, P2 p2);
    }

    public interface IYIUIEventInvokeSystem<in P1, in P2, in P3> : ISystemType
    {
        void Invoke(Entity o, P1 p1, P2 p2, P3 p3);
    }

    public interface IYIUIEventInvokeSystem<in P1, in P2, in P3, in P4> : ISystemType
    {
        void Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4);
    }

    public interface IYIUIEventInvokeSystem<in P1, in P2, in P3, in P4, in P5> : ISystemType
    {
        void Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }

    [EntitySystem]
    public abstract class YIUIEventInvokeSystem<T> : SystemObject, IYIUIEventInvokeSystem where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEventInvokeSystem);
        }

        public void Invoke(Entity o)
        {
            Invoke((T)o);
        }

        protected abstract void Invoke(T self);
    }

    [EntitySystem]
    public abstract class YIUIEventInvokeSystem<T, P1> : SystemObject, IYIUIEventInvokeSystem<P1> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEventInvokeSystem<P1>);
        }

        public void Invoke(Entity o, P1 p1)
        {
            Invoke((T)o, p1);
        }

        protected abstract void Invoke(T self, P1 p1);
    }

    [EntitySystem]
    public abstract class YIUIEventInvokeSystem<T, P1, P2> : SystemObject, IYIUIEventInvokeSystem<P1, P2> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEventInvokeSystem<P1, P2>);
        }

        public void Invoke(Entity o, P1 p1, P2 p2)
        {
            Invoke((T)o, p1, p2);
        }

        protected abstract void Invoke(T self, P1 p1, P2 p2);
    }

    [EntitySystem]
    public abstract class YIUIEventInvokeSystem<T, P1, P2, P3> : SystemObject, IYIUIEventInvokeSystem<P1, P2, P3> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEventInvokeSystem<P1, P2, P3>);
        }

        public void Invoke(Entity o, P1 p1, P2 p2, P3 p3)
        {
            Invoke((T)o, p1, p2, p3);
        }

        protected abstract void Invoke(T self, P1 p1, P2 p2, P3 p3);
    }

    [EntitySystem]
    public abstract class YIUIEventInvokeSystem<T, P1, P2, P3, P4> : SystemObject, IYIUIEventInvokeSystem<P1, P2, P3, P4> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEventInvokeSystem<P1, P2, P3, P4>);
        }

        public void Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4)
        {
            Invoke((T)o, p1, p2, p3, p4);
        }

        protected abstract void Invoke(T self, P1 p1, P2 p2, P3 p3, P4 p4);
    }

    [EntitySystem]
    public abstract class YIUIEventInvokeSystem<T, P1, P2, P3, P4, P5> : SystemObject, IYIUIEventInvokeSystem<P1, P2, P3, P4, P5> where T : Entity
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEventInvokeSystem<P1, P2, P3, P4, P5>);
        }

        public void Invoke(Entity o, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
        {
            Invoke((T)o, p1, p2, p3, p4, p5);
        }

        protected abstract void Invoke(T self, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5);
    }
}
