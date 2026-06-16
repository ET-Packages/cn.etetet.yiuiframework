#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Text;

namespace YIUIFramework
{
    public sealed partial class UIBindDataTable : IYIUIVisionDescriber
    {
        public string GetYIUIVisionText()
        {
            var dataCount = DataDic != null ? DataDic.Count : 0;
            var builder = new StringBuilder();
            builder.Append("UIBindDataTable：共有 ");
            builder.Append(dataCount);
            builder.AppendLine(" 个 Data");

            if (DataDic == null || DataDic.Count <= 0)
            {
                builder.AppendLine("- 未配置 Data");
                return YIUIVisionTextHelper.TrimEndLine(builder);
            }

            var keys = new List<string>(DataDic.Keys);
            keys.Sort(StringComparer.Ordinal);

            foreach (var key in keys)
            {
                if (DataDic.TryGetValue(key, out var data))
                {
                    YIUIVisionTextHelper.AppendDataLine(builder, data, true);
                }
            }

            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
