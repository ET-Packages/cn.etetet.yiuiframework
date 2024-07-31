using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UIEventP0 : UIEventBase, IUIEventInvoke
    {
        private LinkedList<UIEventHandleP0> m_UIEventHandles;
        public  LinkedList<UIEventHandleP0> UIEventHandles => m_UIEventHandles;

        public UIEventP0()
        {
        }

        public UIEventP0(string name) : base(name)
        {
        }

        public void Invoke()
        {
            if (m_UIEventHandles == null)
            {
                Logger.LogWarning($"{EventName} 未绑定任何事件");
                return;
            }

            var handle = m_UIEventHandles.First;
            while (handle != null)
            {
                var next  = handle.Next;
                var value = handle.Value;
                handle = next;

                if (value == null)
                {
                    continue;
                }

                if (value.OnEventInvokeType != null)
                {
                    if (Trigger == null)
                    {
                        Logger.LogError($"UI事件名称:{EventName}  事件:{value.OnEventInvokeType.Name} Trigger == null");
                        continue;
                    }

                    var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(Trigger.GetType(), typeof(IYIUIEventInvokeSystem));
                    if (iEventSystems is not { Count: > 0 })
                    {
                        Logger.LogError($"类:{Trigger.GetType()} UI事件名称:{EventName} 没有具体实现的事件 IYIUIEventInvokeSystem 请检查");
                        continue;
                    }

                    foreach (IYIUIEventInvokeSystem eventSystem in iEventSystems)
                    {
                        if (eventSystem.GetType() == value.OnEventInvokeType)
                        {
                            try
                            {
                                eventSystem.Invoke(Trigger);
                            }
                            catch (Exception e)
                            {
                                Logger.LogError($"类:{Trigger.GetType()} UI事件名称:{EventName} 事件:{value.OnEventInvokeType.Name} 事件回调错误: {e.Message}");
                            }
                        }
                    }
                }
                else if (value.UIEventParamDelegate != null)
                {
                    try
                    {
                        value.UIEventParamDelegate.Invoke();
                    }
                    catch (Exception e)
                    {
                        Logger.LogError($"UI事件名称:{EventName} 委托:{value.UIEventParamDelegate.GetType().Name} 委托回调错误: {e.Message}");
                    }
                }
                else
                {
                    Logger.LogError($"UI事件名称:{EventName} 没有实现事件 也没有实现委托 请检查");
                }
            }
        }

        public override bool IsTaskEvent => false;

        public override bool Clear()
        {
            if (m_UIEventHandles == null) return false;

            var first = m_UIEventHandles.First;
            while (first != null)
            {
                PublicUIEventP0.HandlerPool.Release(first.Value);
                first = m_UIEventHandles.First;
            }

            LinkedListPool<UIEventHandleP0>.Release(m_UIEventHandles);
            m_UIEventHandles = null;
            return true;
        }

        public UIEventHandleP0 Add(Entity trigger, Type onEventInvokeType)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP0>.Get();
            m_Trigger        =   trigger;
            var handler = PublicUIEventP0.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, onEventInvokeType);
        }

        public UIEventHandleP0 Add(UIEventDelegate callback)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP0>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP0.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, callback);
        }

        public bool Remove(UIEventHandleP0 handle)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP0>.Get();

            if (handle == null)
            {
                Logger.LogError($"{EventName} UIEventParamHandle == null");
                return false;
            }

            return m_UIEventHandles.Remove(handle);
        }

        #if UNITY_EDITOR
        public override string GetEventType()
        {
            return "UIEventP0";
        }

        public override string GetEventHandleType()
        {
            return "UIEventHandleP0";
        }
        #endif
    }
}
