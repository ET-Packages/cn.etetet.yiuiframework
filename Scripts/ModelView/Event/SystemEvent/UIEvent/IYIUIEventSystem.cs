using System;

namespace ET.Client
{
    public struct YIUIEvent
    {
    }

    public interface IYIUIEvent<A> : IClassEvent<A> where A : struct
    {
    }

    //A = 消息类型 = 如:YIUIEventPanelOpenBefore
    public interface IYIUIEventSystem<in A> : ISystemType where A : struct
    {
        ETTask Run(Entity o, A message);
    }

    [EntitySystem]
    public abstract class YIUIEventSystem<T, A> : ClassEventSystem<T, A>, IYIUIEventSystem<A> where T : Entity, IYIUIEvent<A> where A : struct
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEventSystem<A>);
        }

        //TODO 这里会有覆盖提示 等待官方提供异步方案
        public new async ETTask Run(Entity o, A message)
        {
            await YIUIEvent((T)o, message);
        }

        protected override void Handle(Entity e,    A t)
        {
            throw new NotImplementedException();
        }
        
        protected abstract ETTask YIUIEvent(T   self, A message);
    }
}