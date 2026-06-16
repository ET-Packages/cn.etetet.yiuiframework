#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Text;

namespace YIUIFramework
{
    public sealed partial class UIBindEventTable : IYIUIVisionDescriber
    {
        public string GetYIUIVisionText()
        {
            var eventCount = EventDic != null ? EventDic.Count : 0;
            var builder = new StringBuilder();
            builder.Append("UIBindEventTable：共有 ");
            builder.Append(eventCount);
            builder.AppendLine(" 个 Event");

            if (EventDic == null || EventDic.Count <= 0)
            {
                builder.AppendLine("- 未配置 Event");
                return YIUIVisionTextHelper.TrimEndLine(builder);
            }

            var keys = new List<string>(EventDic.Keys);
            keys.Sort(StringComparer.Ordinal);

            foreach (var key in keys)
            {
                if (EventDic.TryGetValue(key, out var uiEvent))
                {
                    YIUIVisionTextHelper.AppendEventLine(builder, key, uiEvent);
                }
            }

            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
