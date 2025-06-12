namespace ET
{
    /// <summary>
    /// 特殊情况下使用的 提前检查一个Invoke是否存在
    /// </summary>
    public partial class EventSystem
    {
        public bool CheckInvoke<A>(long type) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                return false;
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                return false;
            }

            var aInvokeHandler = invokeHandler as AInvokeHandler<A>;
            if (aInvokeHandler == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckInvoke<A, T>(long type) where A : struct
        {
            if (!this.allInvokers.TryGetValue(typeof(A), out var invokeHandlers))
            {
                return false;
            }

            if (!invokeHandlers.TryGetValue(type, out var invokeHandler))
            {
                return false;
            }

            var aInvokeHandler = invokeHandler as AInvokeHandler<A, T>;
            if (aInvokeHandler == null)
            {
                return false;
            }

            return true;
        }

        public bool CheckInvoke<A>() where A : struct
        {
            return CheckInvoke<A>(0);
        }

        public bool CheckInvoke<A, T>() where A : struct
        {
            return CheckInvoke<A, T>(0);
        }
    }
}