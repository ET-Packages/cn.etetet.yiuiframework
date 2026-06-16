#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public partial class UIEventBindActive
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            builder.Append("触发：GameObject OnEnable/OnDisable；参数=(gameObject, activeState)；target=");
            builder.AppendLine(YIUIVisionTextHelper.FormatUnityObject(gameObject));
        }
    }

    public partial class UITaskEventBindActive
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            builder.Append("触发：GameObject OnEnable/OnDisable 异步；参数=(gameObject, activeState)；target=");
            builder.AppendLine(YIUIVisionTextHelper.FormatUnityObject(gameObject));
        }
    }

    public partial class UIEventBindChangeDataValue
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var dataBindChange = YIUIEventVisionTextHelper.ResolveComponent(this, m_UIDataBindChange);
            builder.Append("触发：UIDataBindChange 改值后回调；source=");
            builder.AppendLine(YIUIVisionTextHelper.FormatUnityObject(dataBindChange));

            if (dataBindChange != null)
            {
                builder.AppendLine("触发来源详情：");
                builder.AppendLine(dataBindChange.GetYIUIVisionText());
            }
        }
    }

    public partial class UIEventBindBeginDrag
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：BeginDrag；参数=PointerEventData object；selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UIEventBindDrag
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：Drag；参数=PointerEventData object；selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UIEventBindEndDrag
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：EndDrag；参数=PointerEventData object；selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UIEventBindPress
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：长按一次；pressTime=");
            builder.Append(YIUIVisionTextHelper.FormatFloat(m_PressTime));
            builder.Append("，skipExit=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SkipExit));
            builder.Append("，effectiveRange=");
            builder.Append(YIUIVisionTextHelper.FormatVector2(m_EffectiveRange));
            builder.Append("，selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
            builder.AppendLine("事件参数：PointerEventData 作为 object 传入");
        }
    }

    public partial class UIEventBindKeepPress
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：持续长按；interval=");
            builder.Append(YIUIVisionTextHelper.FormatFloat(m_PressTime));
            builder.Append("，skipExit=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SkipExit));
            builder.Append("，effectiveRange=");
            builder.Append(YIUIVisionTextHelper.FormatVector2(m_EffectiveRange));
            builder.Append("，selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
            builder.AppendLine("事件参数：PointerEventData 作为 object 传入");
        }
    }
}

#endif
