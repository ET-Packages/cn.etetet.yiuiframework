#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public abstract partial class UIDataBind : IYIUIVisionDescriber
    {
        public virtual string GetYIUIVisionText()
        {
            var componentName = GetType().Name;
            var builder = new StringBuilder();

            if (this is UIDataBindSelectBase selectBase)
            {
                YIUIVisionTextHelper.AppendHeader(builder, componentName, YIUIVisionTextHelper.GetDataSelectCount(selectBase.DataSelectDic));
                YIUIVisionTextHelper.AppendDataSelectLines(builder, selectBase.DataSelectDic);
            }
            else
            {
                YIUIVisionTextHelper.AppendHeader(builder, componentName, 0);
                builder.AppendLine("- 当前组件未暴露通用 DataSelect 列表");
            }

            builder.Append("作用：通用 DataBind 摘要，当前组件尚未定制更细的 YIUIDATA 视觉文本");
            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
