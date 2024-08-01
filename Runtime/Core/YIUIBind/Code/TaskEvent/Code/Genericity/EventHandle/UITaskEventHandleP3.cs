using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUITaskEventP3<P1, P2, P3>
    {
        public static readonly ObjectPool<UITaskEventHandleP3<P1, P2, P3>> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 3个泛型参数
    /// </summary>
    public sealed class UITaskEventHandleP3<P1, P2, P3>
    {
        private LinkedList<UITaskEventHandleP3<P1, P2, P3>>     m_UITaskEventList;
        private LinkedListNode<UITaskEventHandleP3<P1, P2, P3>> m_UITaskEventNode;

        public UITaskEventDelegate<P1, P2, P3> UITaskEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public Type OnEventInvokeType { get; private set; }

        public UITaskEventHandleP3()
        {
        }

        internal UITaskEventHandleP3<P1, P2, P3> Init(LinkedList<UITaskEventHandleP3<P1, P2, P3>> uiTaskEventList, LinkedListNode<UITaskEventHandleP3<P1, P2, P3>> uiTaskEventNode, Entity trigger, Type onEventInvokeType)
        {
            m_UITaskEventList = uiTaskEventList;
            m_UITaskEventNode = uiTaskEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UITaskEventHandleP3<P1, P2, P3> Init(LinkedList<UITaskEventHandleP3<P1, P2, P3>> uiTaskEventList, LinkedListNode<UITaskEventHandleP3<P1, P2, P3>> uiTaskEventNode, UITaskEventDelegate<P1, P2, P3> uiTaskEventDelegate)
        {
            m_UITaskEventList        = uiTaskEventList;
            m_UITaskEventNode        = uiTaskEventNode;
            UITaskEventParamDelegate = uiTaskEventDelegate;
            return this;
        }

        internal async ETTask Invoke(P1 p1, P2 p2, P3 p3)
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType.Name} Trigger == null");
                    return;
                }

                var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(Trigger.GetType(), typeof(IYIUITaskEventInvokeSystem<P1, P2, P3>));
                if (iEventSystems is not { Count: > 0 })
                {
                    Logger.LogError($"类:{Trigger.GetType()} UI事件名称:{OnEventInvokeType.Name} 没有具体实现的事件 IYIUITaskEventInvokeSystem {typeof(P1).Name} {typeof(P2).Name} {typeof(P3).Name} 请检查");
                    return;
                }

                foreach (IYIUITaskEventInvokeSystem<P1, P2, P3> eventSystem in iEventSystems)
                {
                    if (eventSystem.GetType() == OnEventInvokeType)
                    {
                        try
                        {
                            await eventSystem.Invoke(Trigger, p1, p2, p3);
                            return;
                        }
                        catch (Exception e)
                        {
                            Logger.LogError($"类:{Trigger.GetType()} UI事件名称:{OnEventInvokeType.Name} 事件:{OnEventInvokeType.Name} 事件回调错误: {e.Message}");
                        }
                    }
                }
            }
            else if (UITaskEventParamDelegate != null)
            {
                try
                {
                    await UITaskEventParamDelegate.Invoke(p1, p2, p3);
                }
                catch (Exception e)
                {
                    Logger.LogError($"委托:{UITaskEventParamDelegate.GetType().Name} 委托回调错误: {e.Message}");
                }
            }
            else
            {
                Logger.LogError($"没有实现事件 也没有实现委托 请检查");
            }
        }

        public void Dispose()
        {
            OnEventInvokeType        = null;
            UITaskEventParamDelegate = null;
            m_Trigger                = null;
            if (m_UITaskEventList == null || m_UITaskEventNode == null) return;
            m_UITaskEventList.Remove(m_UITaskEventNode);
            m_UITaskEventNode = null;
            m_UITaskEventList = null;
        }
    }
}