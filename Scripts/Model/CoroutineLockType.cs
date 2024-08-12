namespace ET
{
    public static partial class CoroutineLockType
    {
        //这个是YIUI框架用的，用来区分协程锁的类型
        //你应该定义自己的协程锁类型 以免Key相同时发生冲突
        public const int YIUIFramework = PackageType.YIUI * 1000 + 1;
    }
}
