#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public partial class UIDataBindColor
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, GetType().Name, YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("目标：Graphic=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_Graphic));
            if (m_Graphic != null)
            {
                builder.Append("，color=");
                builder.Append(YIUIVisionTextHelper.FormatColor(m_Graphic.color));
            }

            builder.AppendLine();
            builder.Append("作用：用 Color Data 同步 Graphic.color");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }

    public sealed partial class UIDataBindRawImage
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, nameof(UIDataBindRawImage), YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：setNativeSize=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SetNativeSize));
            builder.Append("，changeEnabled=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_ChangeEnabled));
            builder.Append("，lastResName=");
            builder.Append(YIUIVisionTextHelper.Quote(m_LastResName));
            builder.AppendLine();
            builder.Append("目标：RawImage=");
            builder.AppendLine(YIUIVisionTextHelper.FormatUnityObject(m_RawImage));
            builder.Append("作用：用 String Data 作为 Texture2D 资源名加载 RawImage.texture");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
