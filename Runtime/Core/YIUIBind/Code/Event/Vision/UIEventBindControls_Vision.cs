#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public partial class UIEventBindDropdown
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var dropdown = YIUIEventVisionTextHelper.ResolveComponent(this, m_Dropdown);
            builder.Append("触发：Dropdown.onValueChanged；参数=int 当前选中索引；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatDropdown(dropdown));
        }
    }

    public partial class UIEventBindInputField
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var inputField = YIUIEventVisionTextHelper.ResolveComponent(this, m_InputField);
            builder.Append("触发：InputField.onValueChanged；参数=string 当前输入值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatInputField(inputField));
        }
    }

    public partial class UIEventBindInputFieldEnd
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var inputField = YIUIEventVisionTextHelper.ResolveComponent(this, m_InputField);
            builder.Append("触发：InputField.onEndEdit；参数=string 结束编辑值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatInputField(inputField));
        }
    }

    public partial class UIEventBindScrollbar
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var scrollbar = YIUIEventVisionTextHelper.ResolveComponent(this, m_Scrollbar);
            builder.Append("触发：Scrollbar.onValueChanged；参数=float 当前滚动值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatScrollbar(scrollbar));
        }
    }

    public partial class UIEventBindSlider
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var slider = YIUIEventVisionTextHelper.ResolveComponent(this, m_Slider);
            builder.Append("触发：Slider.onValueChanged；参数=float 当前滑动值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSlider(slider));
        }
    }

    public partial class UIEventBindToggle
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var toggle = YIUIEventVisionTextHelper.ResolveComponent(this, m_Toggle);
            builder.Append("触发：Toggle.onValueChanged；参数=bool 当前开关状态；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatToggle(toggle));
        }
    }
}

#if TextMeshPro

namespace YIUIFramework
{
    public partial class UIEventBindDropdownTMP
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var dropdown = YIUIEventVisionTextHelper.ResolveComponent(this, m_Dropdown);
            builder.Append("触发：TMP_Dropdown.onValueChanged；参数=int 当前选中索引；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatTMPDropdown(dropdown));
        }
    }

    public partial class UIEventBindInputFieldTMP
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var inputField = YIUIEventVisionTextHelper.ResolveComponent(this, m_InputField);
            builder.Append("触发：TMP_InputField.onValueChanged；参数=string 当前输入值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatTMPInputField(inputField));
        }
    }

    public partial class UIEventBindInputFieldEndTMP
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var inputField = YIUIEventVisionTextHelper.ResolveComponent(this, m_InputField);
            builder.Append("触发：TMP_InputField.onEndEdit；参数=string 结束编辑值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatTMPInputField(inputField));
        }
    }

    public partial class UITaskEventBindInputFieldTMP
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var inputField = YIUIEventVisionTextHelper.ResolveComponent(this, m_InputField);
            builder.Append("触发：TMP_InputField.onValueChanged 异步；参数=string 当前输入值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatTMPInputField(inputField));
        }
    }

    public partial class UITaskEventBindInputFieldEndTMP
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var inputField = YIUIEventVisionTextHelper.ResolveComponent(this, m_InputField);
            builder.Append("触发：TMP_InputField.onEndEdit 异步；参数=string 结束编辑值；target=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatTMPInputField(inputField));
        }
    }
}

#endif

#endif
