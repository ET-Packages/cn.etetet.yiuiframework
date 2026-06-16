#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public partial class UIEventBindClick
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：PointerClick；skipWhenDrag=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SkipWhenDrag));
            builder.Append("，selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UIEventBindClickDown
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：PointerDown；selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UIEventBindClickUp
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：PointerUp；selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UIEventBindClickInt
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.Append("额外参数：int=");
            builder.AppendLine(m_ExtraParam.ToString());
        }
    }

    public partial class UIEventBindClickString
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.Append("额外参数：string=");
            builder.AppendLine(YIUIVisionTextHelper.Quote(m_ExtraParam));
        }
    }

    public partial class UIEventBindClickPointerEventData
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.AppendLine("事件参数：PointerEventData 作为 object 传入");
        }
    }

    public partial class UIEventBindDoubleClick
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：PointerClick 双击；doubleClickInterval=");
            builder.Append(YIUIVisionTextHelper.FormatFloat(m_DoubleClickInterval));
            builder.Append("，skipWhenDrag=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SkipWhenDrag));
            builder.Append("，selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UIEventBindDoubleClickPointerEventData
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.AppendLine("事件参数：PointerEventData 作为 object 传入");
        }
    }

    public partial class UITaskEventBindClick
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：PointerClick 异步；skipWhenDrag=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SkipWhenDrag));
            builder.Append("，banLayerOption=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_BanLayerOption));
            builder.Append("，clickTasking=");
            builder.Append(YIUIVisionTextHelper.FormatBool(ClickTasking));
            builder.Append("，selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UITaskEventBindClickInt
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.Append("额外参数：int=");
            builder.AppendLine(m_ExtraParam.ToString());
        }
    }

    public partial class UITaskEventBindClickString
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.Append("额外参数：string=");
            builder.AppendLine(YIUIVisionTextHelper.Quote(m_ExtraParam));
        }
    }

    public partial class UITaskEventBindClickPointerEventData
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.AppendLine("事件参数：PointerEventData 作为 object 异步传入");
        }
    }

    public partial class UITaskEventBindDoubleClick
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            var selectable = YIUIEventVisionTextHelper.ResolveComponent(this, m_Selectable);
            builder.Append("触发：PointerClick 双击异步；doubleClickInterval=");
            builder.Append(YIUIVisionTextHelper.FormatFloat(m_DoubleClickInterval));
            builder.Append("，skipWhenDrag=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_SkipWhenDrag));
            builder.Append("，banLayerOption=");
            builder.Append(YIUIVisionTextHelper.FormatBool(m_BanLayerOption));
            builder.Append("，clickTasking=");
            builder.Append(YIUIVisionTextHelper.FormatBool(ClickTasking));
            builder.Append("，selectable=");
            builder.AppendLine(YIUIEventVisionTextHelper.FormatSelectable(selectable));
        }
    }

    public partial class UITaskEventBindDoubleClickPointerEventData
    {
        protected override void AppendYIUIVisionTriggerText(StringBuilder builder)
        {
            base.AppendYIUIVisionTriggerText(builder);
            builder.AppendLine("事件参数：PointerEventData 作为 object 异步传入");
        }
    }
}

#endif
