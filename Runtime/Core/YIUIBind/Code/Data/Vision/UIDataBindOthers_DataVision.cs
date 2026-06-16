#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public partial class UIDataBindDropdown
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, GetType().Name, YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("目标：Dropdown=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_Dropdown));
            if (m_Dropdown != null)
            {
                builder.Append("，value=");
                builder.Append(m_Dropdown.value);
                builder.Append("，options=");
                builder.Append(m_Dropdown.options != null ? m_Dropdown.options.Count : 0);
            }

            builder.AppendLine();
            builder.Append("作用：双向同步 Int Data 与 Dropdown.value");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }

    public partial class UIDataBindScrollbar
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, GetType().Name, YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：tweenSpeed=");
            builder.Append(YIUIVisionTextHelper.FormatFloat(m_TweenSpeed));
            builder.Append("，tweenType=");
            builder.Append(m_TweenType);
            builder.AppendLine();
            builder.Append("目标：Scrollbar=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_Scrollbar));
            if (m_Scrollbar != null)
            {
                builder.Append("，value=");
                builder.Append(YIUIVisionTextHelper.FormatFloat(m_Scrollbar.value));
                builder.Append("，size=");
                builder.Append(YIUIVisionTextHelper.FormatFloat(m_Scrollbar.size));
            }

            builder.AppendLine();
            builder.Append("作用：用 Float Data 同步 Scrollbar.value");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }

    public sealed partial class UIDataBindSelectable
    {
        public override string GetYIUIVisionText()
        {
            var extraParams = "selectable=" + YIUIVisionTextHelper.FormatUnityObject(m_Selectable);
            if (m_Selectable != null)
            {
                extraParams += "，interactable=" + YIUIVisionTextHelper.FormatBool(m_Selectable.interactable);
            }

            return GetYIUIDataBoolVisionText(nameof(UIDataBindSelectable), extraParams, "控制 Selectable.interactable");
        }
    }

    public partial class UIDataBindSlider
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, GetType().Name, YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：tweenSpeed=");
            builder.Append(YIUIVisionTextHelper.FormatFloat(m_TweenSpeed));
            builder.Append("，tweenType=");
            builder.Append(m_TweenType);
            builder.AppendLine();
            builder.Append("目标：Slider=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_Slider));
            if (m_Slider != null)
            {
                builder.Append("，value=");
                builder.Append(YIUIVisionTextHelper.FormatFloat(m_Slider.value));
                builder.Append("，min=");
                builder.Append(YIUIVisionTextHelper.FormatFloat(m_Slider.minValue));
                builder.Append("，max=");
                builder.Append(YIUIVisionTextHelper.FormatFloat(m_Slider.maxValue));
            }

            builder.AppendLine();
            builder.Append("作用：用 Float Data 同步 Slider.value");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }

    public sealed partial class UIDataBindToggle
    {
        public override string GetYIUIVisionText()
        {
            var extraParams = "toggle=" + YIUIVisionTextHelper.FormatUnityObject(m_Toggle);
            if (m_Toggle != null)
            {
                extraParams += "，isOn=" + YIUIVisionTextHelper.FormatBool(m_Toggle.isOn);
            }

            return GetYIUIDataBoolVisionText(GetType().Name, extraParams, "用 bool 条件结果同步 Toggle.isOn");
        }
    }

    public sealed partial class UIDataBindVideoPlayer
    {
        public override string GetYIUIVisionText()
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, nameof(UIDataBindVideoPlayer), YIUIVisionTextHelper.GetDataSelectCount(DataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, DataSelectDic);

            builder.Append("参数：changeEnabled=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_ChangeEnabled));
            builder.Append("，lastResName=");
            builder.Append(YIUIVisionTextHelper.Quote(m_LastResName));
            builder.AppendLine();
            builder.Append("目标：VideoPlayer=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_VideoPlayer));
            if (m_VideoPlayer != null)
            {
                builder.Append("，clip=");
                builder.Append(YIUIVisionTextHelper.FormatUnityObject(m_VideoPlayer.clip));
            }

            builder.AppendLine();
            builder.Append("作用：用 String Data 作为 VideoClip 资源名加载 VideoPlayer.clip");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
