using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UIEventP5<P1, P2, P3, P4, P5> : UIEventBase, IUIEventInvoke<P1, P2, P3, P4, P5>
    {
        private LinkedList<UIEventHandleP5<P1, P2, P3, P4, P5>> m_UIEventHandles;
        public  LinkedList<UIEventHandleP5<P1, P2, P3, P4, P5>> UIEventHandles => m_UIEventHandles;

        public UIEventP5()
        {
        }

        public UIEventP5(string name) : base(name)
        {
        }

        public void Invoke(P1 p1, P2 p2, P3 p3, P4 p4, P5 p5)
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

                if (value != null)
                {
                    if (!value.Invoke(p1, p2, p3, p4, p5))
                    {
                        Logger.LogError($"UI事件名称:{EventName} 执行错误 请配合上面报错信息排查");
                    }
                }

                handle = next;
            }
        }

        public override bool IsTaskEvent => false;

        public override bool Clear()
        {
            if (m_UIEventHandles == null) return false;

            var first = m_UIEventHandles.First;
            while (first != null)
            {
                PublicUIEventP5<P1, P2, P3, P4, P5>.HandlerPool.Release(first.Value);
                first = m_UIEventHandles.First;
            }

            LinkedListPool<UIEventHandleP5<P1, P2, P3, P4, P5>>.Release(m_UIEventHandles);
            m_UIEventHandles = null;
            return true;
        }

        public UIEventHandleP5<P1, P2, P3, P4, P5> Add(Entity trigger, string onEventInvokeType)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP5<P1, P2, P3, P4, P5>>.Get();
            var handler = PublicUIEventP5<P1, P2, P3, P4, P5>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, trigger, onEventInvokeType);
        }

        public UIEventHandleP5<P1, P2, P3, P4, P5> Add(UIEventDelegate<P1, P2, P3, P4, P5> callback)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP5<P1, P2, P3, P4, P5>>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP5<P1, P2, P3, P4, P5>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, callback);
        }

        public bool Remove(UIEventHandleP5<P1, P2, P3, P4, P5> handle)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP5<P1, P2, P3, P4, P5>>.Get();

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
            return $"UIEventP5<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)},{GetParamTypeString(3)},{GetParamTypeString(4)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP5<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)},{GetParamTypeString(3)},{GetParamTypeString(4)}>";
        }
        #endif
    }
}