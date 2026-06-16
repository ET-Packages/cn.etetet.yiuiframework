#if UNITY_EDITOR

using System.Text;

namespace YIUIFramework
{
    public abstract partial class UIDataBindBool
    {
        public override string GetYIUIVisionText()
        {
            return GetYIUIDataBoolVisionText(GetType().Name, null, "根据条件计算 bool 结果");
        }

        protected string GetYIUIDataBoolVisionText(string componentName, string extraParams, string actionText)
        {
            var dataCount = m_Datas != null ? m_Datas.Count : 0;
            var builder = new StringBuilder();
            YIUIVisionTextHelper.AppendHeader(builder, componentName, dataCount);

            if (m_Datas == null || m_Datas.Count <= 0)
            {
                builder.AppendLine("- 未绑定 Bool 条件 Data");
            }
            else
            {
                foreach (var dataRef in m_Datas)
                {
                    if (dataRef == null)
                    {
                        builder.AppendLine("- null：读取失败，UIDataBoolRef=null");
                        continue;
                    }

                    builder.AppendLine(dataRef.GetYIUIVisionText());
                }
            }

            builder.Append("参数：logic=");
            builder.Append(m_BooleanLogic);
            if (!string.IsNullOrEmpty(extraParams))
            {
                builder.Append("，");
                builder.Append(extraParams);
            }

            if (!string.IsNullOrEmpty(actionText))
            {
                builder.AppendLine();
                builder.Append("作用：");
                builder.Append(actionText);
            }

            return YIUIVisionTextHelper.TrimEndLine(builder);
        }
    }
}

#endif
