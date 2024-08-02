﻿using System;

namespace ET
{
    /// <summary>
    /// 标记Entity的System静态类 用于自动生成System函数
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class YIUIEntitySystemOfAttribute: EnableClassAttribute
    {
        public Type type;

        /// <summary>
        /// 标记Entity的System静态类 用于自动生成System函数
        /// </summary>
        /// <param name="type">Entity类型</param>
        /// <param name="ignoreAwake">是否忽略生成AwakeSystem</param>
        public YIUIEntitySystemOfAttribute(Type type, bool ignoreAwake = false)
        {
            this.type = type;
        }
    }
}