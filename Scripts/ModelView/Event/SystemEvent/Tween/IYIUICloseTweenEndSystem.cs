using System;

namespace ET.Client
{
    public interface IYIUICloseTweenEnd
    {
    }

    public interface IYIUICloseTweenEndSystem : ISystemType
    {
        void Run(Entity o);
    }

    [EntitySystem]
    public abstract class YIUICloseTweenEndSystem<T> : SystemObject, IYIUICloseTweenEndSystem where T : Entity, IYIUICloseTweenEnd
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUICloseTweenEndSystem);
        }

        void IYIUICloseTweenEndSystem.Run(Entity o)
        {
            this.YIUICloseTweenEnd((T)o);
        }

        protected abstract void YIUICloseTweenEnd(T self);
    }
}
