using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false)]
    public sealed class IgnoreYIUIEventPublishCheckAttribute : Attribute
    {
    }
}
