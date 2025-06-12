namespace ET
{
    /// <summary>
    /// 特殊情况下使用的 安全的使用Invoke
    /// 如果存在则调用 如果不存在也不会报错
    /// </summary>
    public partial class EventSystem
    {
        public void SafetyInvoke<A>(long type, A args) where A : struct
        {
            if (!CheckInvoke<A>(type))
            {
                return;
            }

            Invoke(type, args);
        }

        public T SafetyInvoke<A, T>(long type, A args, T defaultValue = default) where A : struct
        {
            if (!CheckInvoke<A, T>(type))
            {
                return defaultValue;
            }

            return Invoke<A, T>(type, args);
        }

        public void SafetyInvoke<A>(A args) where A : struct
        {
            SafetyInvoke<A>(0, args);
        }

        public T SafetyInvoke<A, T>(A args, T defaultValue = default) where A : struct
        {
            return SafetyInvoke(0, args, defaultValue);
        }
    }
}