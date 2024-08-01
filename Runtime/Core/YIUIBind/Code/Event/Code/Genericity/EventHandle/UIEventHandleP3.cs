using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP3<P1, P2, P3>
    {
        public static readonly ObjectPool<UIEventHandleP3<P1, P2, P3>> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 3个泛型参数
    /// </summary>
    public sealed class UIEventHandleP3<P1, P2, P3>
    {
        private LinkedList<UIEventHandleP3<P1, P2, P3>>     m_UIEventList;
        private LinkedListNode<UIEventHandleP3<P1, P2, P3>> m_UIEventNode;

        public UIEventDelegate<P1, P2, P3> UIEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public Type OnEventInvokeType { get; private set; }

        public UIEventHandleP3()
        {
        }

        internal UIEventHandleP3<P1, P2, P3> Init(LinkedList<UIEventHandleP3<P1, P2, P3>> uiEventList, LinkedListNode<UIEventHandleP3<P1, P2, P3>> uiEventNode, Entity trigger, Type onEventInvokeType)
        {
            m_UIEventList     = uiEventList;
            m_UIEventNode     = uiEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UIEventHandleP3<P1, P2, P3> Init(LinkedList<UIEventHandleP3<P1, P2, P3>> uiEventList, LinkedListNode<UIEventHandleP3<P1, P2, P3>> uiEventNode, UIEventDelegate<P1, P2, P3> uiEventDelegate)
        {
            m_UIEventList        = uiEventList;
            m_UIEventNode        = uiEventNode;
            UIEventParamDelegate = uiEventDelegate;
            return this;
        }

        internal bool Invoke(P1 p1, P2 p2, P3 p3)
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType.Name} Trigger == null");
                    return false;
                }

                var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(Trigger.GetType(), typeof(IYIUIEventInvokeSystem<P1, P2, P3>));
                if (iEventSystems is not { Count: > 0 })
                {
                    Logger.LogError($"类:{Trigger.GetType()} UI事件名称:{OnEventInvokeType.Name} 没有具体实现的事件 IYIUIEventInvokeSystem {typeof(P1).Name} {typeof(P2).Name} {typeof(P3).Name} 请检查");
                    return false;
                }

                foreach (IYIUIEventInvokeSystem<P1, P2, P3> eventSystem in iEventSystems)
                {
                    if (eventSystem.GetType() == OnEventInvokeType)
                    {
                        try
                        {
                            eventSystem.Invoke(Trigger, p1, p2, p3);
                            return true;
                        }
                        catch (Exception e)
                        {
                            Logger.LogError($"类:{Trigger.GetType()} UI事件名称:{OnEventInvokeType.Name} 事件:{OnEventInvokeType.Name} 事件回调错误: {e.Message}");
                        }
                    }
                }
            }
            else if (UIEventParamDelegate != null)
            {
                try
                {
                    UIEventParamDelegate.Invoke(p1, p2, p3);
                    return true;
                }
                catch (Exception e)
                {
                    Logger.LogError($"委托:{UIEventParamDelegate.GetType().Name} 委托回调错误: {e.Message}");
                }
            }
            else
            {
                Logger.LogError($"没有实现事件 也没有实现委托 请检查");
            }

            return false;
        }

        public void Dispose()
        {
            OnEventInvokeType    = null;
            UIEventParamDelegate = null;
            m_Trigger            = null;
            if (m_UIEventList == null || m_UIEventNode == null) return;
            m_UIEventList.Remove(m_UIEventNode);
            m_UIEventNode = null;
            m_UIEventList = null;
        }
    }
}