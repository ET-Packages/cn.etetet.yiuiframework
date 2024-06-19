using System;

namespace ET
{
    public interface IDynamicEvent<in A> where A : struct
    {
    }

    public interface IDynamicEventSystem<in A> : ISystemType where A : struct
    {
        ETTask Run(Entity o, A message);
    }

    [EntitySystem]
    public abstract class DynamicEventSystem<T, A> : SystemObject, IDynamicEventSystem<A> where T : Entity, IDynamicEvent<A> where A : struct
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IDynamicEventSystem<A>);
        }

        public async ETTask Run(Entity o, A message)
        {
            await DynamicEvent((T)o, message);
        }

        protected abstract ETTask DynamicEvent(T self, A message);
    }
}
