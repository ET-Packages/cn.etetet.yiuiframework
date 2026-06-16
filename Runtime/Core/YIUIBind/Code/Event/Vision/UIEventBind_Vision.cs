#if UNITY_EDITOR

using System.Collections.Generic;
using System.Text;

namespace YIUIFramework
{
    public abstract partial class UIEventBind : IYIUIVisionDescriber
    {
        public virtual string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            AppendYIUIVisionEventText(builder);
            AppendYIUIVisionTriggerText(builder);
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }

        protected virtual void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            builder.AppendLine("触发：通用 UIEventBind，当前组件尚未定制更细的触发视觉文本");
        }

        protected UIEventBase GetYIUIVisionEvent()
        {
            UIEventBase uiEvent = null;
            var eventTable = EventTable;
            if (eventTable != null && eventTable.EventDic != null && !string.IsNullOrEmpty(m_EventName))
            {
                eventTable.EventDic.TryGetValue(m_EventName, out uiEvent);
            }

            if (uiEvent != null)
            {
                return uiEvent;
            }

            return m_UIEvent;
        }

        private void AppendYIUIVisionEventText(StringBuilder builder)
        {
            var uiEvent = GetYIUIVisionEvent();
            var expectedParams = GetFilterParamType;
            var actualParams = uiEvent != null ? uiEvent.AllEventParamType : null;
            var eventFound = uiEvent != null;
            var modeMatch = uiEvent != null && uiEvent.IsTaskEvent == IsTaskEvent;
            var paramMatch = uiEvent != null && YIUIVisionTextHelper.EventParamTypesMatch(expectedParams, actualParams);

            builder.Append(GetType().Name);
            builder.Append("：eventName=");
            builder.AppendLine(YIUIVisionTextHelper.Quote(m_EventName ?? ""));

            builder.Append("状态：eventTable=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(EventTable));
            builder.Append("，eventFound=");
            builder.Append(YIUIVisionTextHelper.FormatBool(eventFound));
            builder.Append("，modeMatch=");
            builder.Append(YIUIVisionTextHelper.FormatBool(modeMatch));
            builder.Append("，paramMatch=");
            builder.AppendLine(YIUIVisionTextHelper.FormatBool(paramMatch));

            builder.Append("组件期望：mode=");
            builder.Append(YIUIVisionTextHelper.FormatEventMode(IsTaskEvent));
            builder.Append("，params=");
            builder.AppendLine(YIUIVisionTextHelper.FormatEventParamTypes(expectedParams));

            if (uiEvent == null)
            {
                builder.AppendLine("事件定义：null");
                return;
            }

            builder.Append("事件定义：mode=");
            builder.Append(YIUIVisionTextHelper.FormatEventMode(uiEvent.IsTaskEvent));
            builder.Append("，type=");
            builder.Append(uiEvent.GetEventType());
            builder.Append("，handle=");
            builder.Append(uiEvent.GetEventHandleType());
            builder.Append("，params=");
            builder.Append(YIUIVisionTextHelper.FormatEventParamTypes(actualParams));
            builder.Append("，binds=");
            builder.AppendLine(uiEvent.GetBindCount().ToString());
        }
    }
}

#endif
