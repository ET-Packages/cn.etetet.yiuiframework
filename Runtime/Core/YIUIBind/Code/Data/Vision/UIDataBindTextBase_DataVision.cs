#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public abstract partial class UIDataBindTextBase
    {
        public override string GetYIUIVisionText()
        {
            return GetYIUIDataTextVisionText(GetType().Name, null);
        }

        protected string GetYIUIDataTextVisionText(string componentName, string targetText)
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, componentName, YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：format=");
            builder.Append(YIUIVisionTextHelper.Quote(m_Format));
            builder.Append("，changeEnabled=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_ChangeEnabled));
            builder.Append("，numberPrecision=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_NumberPrecision));
            builder.Append("，numberPrecisionStr=");
            builder.Append(YIUIVisionTextHelper.Quote(m_NumberPrecisionStr));

            if (!string.IsNullOrEmpty(targetText))
            {
                builder.AppendLine();
                builder.Append(targetText);
            }

            builder.AppendLine();
            builder.Append("作用：同步文本内容");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
