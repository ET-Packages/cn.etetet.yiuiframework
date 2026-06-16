#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public partial class UIDataBindRectPos2
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform,
                "localPosition=" + YIUIVisionTextHelper.FormatBool(m_LocalPosition),
                m_LocalPosition ? "用 Vector2 Data 同步 RectTransform.localPosition" : "用 Vector2 Data 同步 RectTransform.position");
        }
    }

    public partial class UIDataBindRectPos3
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform,
                "localPosition=" + YIUIVisionTextHelper.FormatBool(m_LocalPosition),
                m_LocalPosition ? "用 Vector3 Data 同步 RectTransform.localPosition" : "用 Vector3 Data 同步 RectTransform.position");
        }
    }

    public partial class UIDataBindRectRot1
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform,
                "targetRot=" + YIUIVisionTextHelper.FormatVector3(m_TargetRot),
                "用 Float Data 乘以目标轴后同步 RectTransform.rotation");
        }
    }

    public partial class UIDataBindRectRot3
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform, null,
                "用 Vector3 Data 同步 RectTransform.rotation");
        }
    }

    public partial class UIDataBindRectScale1
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform,
                "target=" + YIUIVisionTextHelper.FormatVector3(m_Target),
                "用 Float Data 乘以目标轴后同步 RectTransform.localScale");
        }
    }

    public partial class UIDataBindRectScale3
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform, null,
                "用 Vector3 Data 同步 RectTransform.localScale");
        }
    }

    public partial class UIDataBindRectSize
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform, null,
                "用 Vector2 Data 同步 RectTransform.sizeDelta");
        }
    }

    public partial class UIDataBindRectSizeHeight
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform, null,
                "用 Float Data 同步 RectTransform.sizeDelta.y，高度单独变化");
        }
    }

    public partial class UIDataBindRectSizeWidth
    {
        public override string GetYIUIVisionText()
        {
            return UIDataBindRectVisionHelper.Build(GetType().Name, DataSelectDic, m_RectTransform, null,
                "用 Float Data 同步 RectTransform.sizeDelta.x，宽度单独变化");
        }
    }

    internal static class UIDataBindRectVisionHelper
    {
        internal static string Build(string componentName,
                                    System.Collections.Generic.IReadOnlyDictionary<string, UIDataSelect> dataSelectDic,
                                    UnityEngine.RectTransform rectTransform,
                                    string parameters,
                                    string actionText)
        {
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, componentName, YIUIVisionTextHelper.GetDataSelectCount(dataSelectDic));
            YIUIVisionTextHelper.AppendDataSelectLines(builder, dataSelectDic);

            if (!string.IsNullOrEmpty(parameters))
            {
                builder.Append("参数：");
                builder.AppendLine(parameters);
            }

            builder.Append("目标：RectTransform=");
            builder.Append(YIUIVisionTextHelper.FormatUnityObject(rectTransform));
            if (rectTransform != null)
            {
                builder.Append("，localPosition=");
                builder.Append(YIUIVisionTextHelper.FormatVector3(rectTransform.localPosition));
                builder.Append("，anchoredPosition=");
                builder.Append(YIUIVisionTextHelper.FormatVector2(rectTransform.anchoredPosition));
                builder.Append("，sizeDelta=");
                builder.Append(YIUIVisionTextHelper.FormatVector2(rectTransform.sizeDelta));
                builder.Append("，localScale=");
                builder.Append(YIUIVisionTextHelper.FormatVector3(rectTransform.localScale));
            }

            builder.AppendLine();
            builder.Append("作用：");
            builder.Append(actionText);
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
