using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YIUIFramework
{
    /// <summary>
    /// 将遮罩上的一次点击或拖动转交给指定的下层 UI 根节点。
    /// </summary>
    [AddComponentMenu("YIUIFramework/Widget/滑动穿透 【YIUIPointerEventPassThrough】")]
    public sealed class YIUIPointerEventPassThrough : MonoBehaviour,
            IInitializePotentialDragHandler,
            IBeginDragHandler,
            IDragHandler,
            IPointerClickHandler
    {
        private RectTransform m_Target;
        private Action m_CloseAction;

        public void Configure(RectTransform target, Action closeAction)
        {
            m_Target = target;
            m_CloseAction = closeAction;
            enabled = target != null;
        }

        public void Clear()
        {
            m_Target = null;
            m_CloseAction = null;
            enabled = false;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            var dragTarget = GetDragTarget(eventData);
            if (dragTarget != null)
            {
                ExecuteEvents.Execute(dragTarget, eventData, ExecuteEvents.initializePotentialDrag);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            var dragTarget = GetDragTarget(eventData);
            if (dragTarget != null)
            {
                eventData.pointerDrag = dragTarget;
                ExecuteEvents.Execute(dragTarget, eventData, ExecuteEvents.beginDragHandler);
            }

            InvokeClose();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // 仅用于成为初始拖动目标；BeginDrag 已把后续事件切换给下层 UI。
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var target = m_Target;
            var shouldPassClick = IsPointerInsideTarget(target, eventData);
            InvokeClose();

            if (shouldPassClick && target != null)
            {
                ExecuteEvents.ExecuteHierarchy(target.gameObject, eventData, ExecuteEvents.pointerClickHandler);
            }
        }

        private GameObject GetDragTarget(PointerEventData eventData)
        {
            if (!IsPointerInsideTarget(m_Target, eventData))
            {
                return null;
            }

            var dragTarget = ExecuteEvents.GetEventHandler<IDragHandler>(m_Target.gameObject);
            return dragTarget != gameObject ? dragTarget : null;
        }

        private static bool IsPointerInsideTarget(RectTransform target, PointerEventData eventData)
        {
            return target != null && eventData != null &&
                    RectTransformUtility.RectangleContainsScreenPoint(target, eventData.position, eventData.pressEventCamera);
        }

        private void InvokeClose()
        {
            var closeAction = m_CloseAction;
            if (closeAction != null)
            {
                closeAction.Invoke();
            }
        }
    }
}