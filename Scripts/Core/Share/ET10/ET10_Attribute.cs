#if !ET10
using System;

namespace ET
{
    [Conditional("ET10")]
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class IgnoreCircularDependencyAttribute : Attribute
    {
    }
}
#endif