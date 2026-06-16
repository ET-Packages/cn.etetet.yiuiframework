#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public sealed partial class UIDataBindAnimation
    {
        public override string GetYIUIVisionText()
        {
            var clipName = m_Animation != null && m_Animation.clip != null ? m_Animation.clip.name : "";
            var extraParams = "animation=" + YIUIVisionTextHelper.FormatUnityObject(m_Animation) +
                              "，defaultClip=" + YIUIVisionTextHelper.Quote(clipName);
            return GetYIUIDataBoolVisionText(nameof(UIDataBindAnimation), extraParams, "条件为 true 时播放默认 Animation clip，false 时停止默认 clip");
        }
    }

    public sealed partial class UIDataBindAnimationByName
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, nameof(UIDataBindAnimationByName), YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：animation=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_Animation));
            builder.Append("，availableClips=");
            builder.AppendLine(YIUIVisionTextHelper.FormatStringList(m_AllClipName));
            builder.Append("作用：用 String Data 作为 Animation clip 名称并播放对应动画");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
