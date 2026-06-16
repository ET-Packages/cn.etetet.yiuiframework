#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public partial class UIDataBindChange
    {
        public override string GetYIUIVisionText()
        {
            var dataCount = m_Datas != null ? m_Datas.Count : 0;
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, GetType().Name, dataCount);

            if (m_Datas == null || m_Datas.Count <= 0)
            {
                builder.AppendLine("- 未配置点击改值 Data");
            }
            else
            {
                foreach (var dataRef in m_Datas)
                {
                    if (dataRef == null)
                    {
                        builder.AppendLine("- null：读取失败，UIDataChangeRef=null");
                        continue;
                    }

                    builder.AppendLine(dataRef.GetYIUIVisionText());
                }
            }

            builder.Append("参数：invokeClick=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_InvokeClick));
            builder.Append("，skipWhenDrag=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SkipWhenDrag));
            builder.Append("，selectable=");
            builder.AppendLine(YIUIVisionTextHelper.FormatUnityObject(m_Selectable));
            builder.Append("作用：点击时把绑定 Data 改成预设目标值，并触发 Change 回调");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
