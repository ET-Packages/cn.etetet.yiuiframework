#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public sealed partial class UIDataBindImageFormat
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, nameof(UIDataBindImageFormat), YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：format=");
            builder.Append(YIUIVisionTextHelper.Quote(m_Format));
            builder.Append("，setNativeSize=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SetNativeSize));
            builder.Append("，changeEnabled=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_ChangeEnabled));
            builder.AppendLine();
            builder.Append("目标：Image=");
            builder.AppendLine(YIUIVisionTextHelper.FormatUnityObject(m_Image));
            builder.Append("作用：格式化 Data 后作为 Sprite 资源名加载图片");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
