#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public sealed partial class UIDataBindImage
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, nameof(UIDataBindImage), YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：setNativeSize=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SetNativeSize));
            builder.Append("，changeEnabled=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_ChangeEnabled));
            builder.AppendLine();
            builder.Append("目标：Image=");
            builder.AppendLine(YIUIVisionTextHelper.FormatUnityObject(m_Image));
            builder.Append("作用：用 String Data 作为 Sprite 资源名加载图片");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
