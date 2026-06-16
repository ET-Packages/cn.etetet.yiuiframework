#if ET9

namespace ET
{
    public abstract class IValue<T> : DisposeObject where T : struct
    {
        public T Value
        {
            get;
            set;
        }
    }

    [EnableClass]
    public class ValueTypeWrap<T> : IValue<T>, IPool where T : struct
    {
        public static ValueTypeWrap<T> Create(T value, bool isFromPool = true)
        {
            ValueTypeWrap<T> vw = ObjectPool.Fetch<ValueTypeWrap<T>>(isFromPool);
            vw.Value = value;
            return vw;
        }

        public bool IsFromPool { get; set; }

        public override void Dispose()
        {
            this.Value = default;
            ObjectPool.Recycle(this);
        }
    }
}
#endif
