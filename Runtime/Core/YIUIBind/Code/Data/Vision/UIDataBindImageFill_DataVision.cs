#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public sealed partial class UIDataBindImageFill
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, nameof(UIDataBindImageFill), YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：tweenSpeed=");
            builder.Append(YIUIVisionTextHelper.FormatFloat(m_TweenSpeed));
            builder.Append("，tweenType=");
            builder.Append(m_TweenType);
            builder.AppendLine();
            builder.Append("目标：Image=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_Image));
            if (m_Image != null)
            {
                builder.Append("，fillAmount=");
                builder.Append(YIUIVisionTextHelper.FormatFloat(m_Image.fillAmount));
            }

            builder.AppendLine();
            builder.Append("作用：用 Float Data 同步 Image.fillAmount");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
