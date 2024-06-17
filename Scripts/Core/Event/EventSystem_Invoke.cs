namespace ET
{
    public partial class EventSystem
    {
        public void YIUIInvoke<A>(long invokeType, A args) where A : struct
        {
            this.Invoke(invokeType, args);
        }

        public T YIUIInvoke<A, T>(long invokeType, A args) where A : struct
        {
            return this.Invoke<A, T>(invokeType, args);
        }

        public void YIUIInvokeSync<A>(A args) where A : struct
        {
            this.YIUIInvoke(EYIUIInvokeType.Sync, args);
        }

        public T YIUIInvokeSync<A, T>(A args) where A : struct
        {
            return this.YIUIInvoke<A, T>(EYIUIInvokeType.Sync, args);
        }

        public void YIUIInvokeAsync<A>(A args) where A : struct
        {
            this.YIUIInvoke(EYIUIInvokeType.Async, args);
        }

        public T YIUIInvokeAsync<A, T>(A args) where A : struct
        {
            return this.YIUIInvoke<A, T>(EYIUIInvokeType.Async, args);
        }
    }
}
