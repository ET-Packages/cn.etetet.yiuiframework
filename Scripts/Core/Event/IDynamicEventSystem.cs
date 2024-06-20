using System;

namespace ET
{
    public interface IDynamicEvent<A> : IClassEvent<A> where A : struct
    {
    }

    public interface IDynamicEventSystem<A> : ISystemType where A : struct
    {
        ETTask Run(Entity o, A message);
    }

    [EntitySystem]
    public abstract class DynamicEventSystem<T, A> : ClassEventSystem<T, A>, IDynamicEventSystem<A>
            where T : Entity, IDynamicEvent<A> where A : struct
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IDynamicEventSystem<A>);
        }

        //TODO 这里会有覆盖提示 等待官方提供异步方案
        public async ETTask Run(Entity o, A message)
        {
            await DynamicEvent((T)o, message);
        }

        protected override void Handle(Entity e, A t)
        {
            throw new NotImplementedException();
        }

        protected abstract ETTask DynamicEvent(T self, A message);
    }
}