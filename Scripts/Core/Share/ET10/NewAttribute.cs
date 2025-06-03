#if !ET10
using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class IgnoreCircularDependencyAttribute : Attribute
    {
    }
}
#endif