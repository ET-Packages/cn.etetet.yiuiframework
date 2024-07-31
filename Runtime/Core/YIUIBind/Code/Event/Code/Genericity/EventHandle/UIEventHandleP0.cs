using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP0
    {
        public static readonly ObjectPool<UIEventHandleP0> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 无参数
    /// </summary>
    public sealed class UIEventHandleP0
    {
        private LinkedList<UIEventHandleP0>     m_UIEventList;
        private LinkedListNode<UIEventHandleP0> m_UIEventNode;

        public Type OnEventInvokeType { get; private set; }

        public UIEventDelegate UIEventParamDelegate { get; private set; }

        public UIEventHandleP0()
        {
        }

        internal UIEventHandleP0 Init(LinkedList<UIEventHandleP0> uiEventList, LinkedListNode<UIEventHandleP0> uiEventNode, Type onEventInvokeType)
        {
            if (UIEventParamDelegate != null)
            {
                Logger.LogError($"错误当前已经添加委托 无法添加分发事件 {onEventInvokeType.Name}");
                return null;
            }

            m_UIEventList     = uiEventList;
            m_UIEventNode     = uiEventNode;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UIEventHandleP0 Init(LinkedList<UIEventHandleP0> uiEventList, LinkedListNode<UIEventHandleP0> uiEventNode, UIEventDelegate uiEventDelegate)
        {
            m_UIEventList        = uiEventList;
            m_UIEventNode        = uiEventNode;
            UIEventParamDelegate = uiEventDelegate;
            return this;
        }

        public void Dispose()
        {
            OnEventInvokeType    = null;
            UIEventParamDelegate = null;
            if (m_UIEventList == null || m_UIEventNode == null) return;
            m_UIEventList.Remove(m_UIEventNode);
            m_UIEventNode = null;
            m_UIEventList = null;
        }
    }
}
