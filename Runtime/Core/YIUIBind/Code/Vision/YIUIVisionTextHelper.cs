#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace YIUIFramework
{
    internal static class YIUIVisionTextHelper
    {
        internal static void AppendHeader(StringBuilder builder, string componentName, int dataCount)
        {
            builder.Append(componentName);
            builder.Append("：绑定数据 ");
            builder.Append(dataCount);
            builder.AppendLine(" 个");
        }

        internal static void AppendDataLine(StringBuilder builder, UIData data, bool includeBindCount)
        {
            builder.Append("- ");
            builder.Append(FormatDataInline(data, includeBindCount));
            builder.AppendLine();
        }

        internal static void AppendDataSelectLines(StringBuilder builder, IReadOnlyDictionary<string, UIDataSelect> dataSelectDic)
        {
            if (dataSelectDic == null || dataSelectDic.Count <= 0)
            {
                builder.AppendLine("- 未绑定 Data");
                return;
            }

            var keys = new List<string>(dataSelectDic.Keys);
            keys.Sort(StringComparer.Ordinal);

            foreach (var key in keys)
            {
                if (!dataSelectDic.TryGetValue(key, out var dataSelect) || dataSelect == null)
                {
                    builder.Append("- ");
                    builder.Append(key);
                    builder.AppendLine("：读取失败，DataSelect=null");
                    continue;
                }

                AppendDataLine(builder, dataSelect.Data, false);
            }
        }

        internal static int GetDataSelectCount(IReadOnlyDictionary<string, UIDataSelect> dataSelectDic)
        {
            return dataSelectDic != null ? dataSelectDic.Count : 0;
        }

        internal static string FormatDataInline(UIData data, bool includeBindCount)
        {
            if (data == null)
            {
                return "null：id=null，type=null，value=null" + (includeBindCount ? "，binds=0" : "");
            }

            var dataValue = data.DataValue;
            var typeText = dataValue != null ? dataValue.UIBindDataType.ToString() : "null";
            var valueText = FormatDataValue(dataValue);

            var builder = new StringBuilder();
            builder.Append(data.Name);
            builder.Append("：id=");
            builder.Append(data.DataGuid);
            builder.Append("，type=");
            builder.Append(typeText);
            builder.Append("，value=");
            builder.Append(valueText);

            if (includeBindCount)
            {
                builder.Append("，binds=");
                builder.Append(data.GetBindCount());
            }

            return builder.ToString();
        }

        internal static string FormatDataValue(UIDataValue dataValue)
        {
            if (dataValue == null)
            {
                return "null";
            }

            var value = dataValue.GetValueToString();
            if (value == null)
            {
                value = "";
            }

            if (dataValue.UIBindDataType == EUIBindDataType.String)
            {
                return Quote(value);
            }

            if (dataValue.UIBindDataType == EUIBindDataType.Bool)
            {
                return FormatBool(dataValue.GetValue<bool>());
            }

            return EscapeInline(value);
        }

        internal static string FormatBool(bool value)
        {
            return value ? "true" : "false";
        }

        internal static string FormatFloat(float value)
        {
            return value.ToString("0.###", CultureInfo.InvariantCulture);
        }

        internal static string FormatVector2(Vector2 value)
        {
            return "(" + FormatFloat(value.x) + ", " + FormatFloat(value.y) + ")";
        }

        internal static string FormatVector3(Vector3 value)
        {
            return "(" + FormatFloat(value.x) + ", " + FormatFloat(value.y) + ", " + FormatFloat(value.z) + ")";
        }

        internal static string FormatColor(Color value)
        {
            return "rgba(" + FormatFloat(value.r) + ", " + FormatFloat(value.g) + ", " + FormatFloat(value.b) + ", " + FormatFloat(value.a) + ")";
        }

        internal static string FormatUnityObject(UnityEngine.Object target)
        {
            if (target == null)
            {
                return "null";
            }

            return target.GetType().Name + "(" + target.name + ")";
        }

        internal static string FormatUnityObjectList<T>(IEnumerable<T> targets) where T : UnityEngine.Object
        {
            if (targets == null)
            {
                return "null";
            }

            var builder = new StringBuilder();
            builder.Append("[");

            var index = 0;
            foreach (var target in targets)
            {
                if (index > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(FormatUnityObject(target));
                index++;
            }

            builder.Append("]");
            return builder.ToString();
        }

        internal static string FormatStringList(IEnumerable<string> values)
        {
            if (values == null)
            {
                return "null";
            }

            var builder = new StringBuilder();
            builder.Append("[");

            var index = 0;
            foreach (var value in values)
            {
                if (index > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(Quote(value));
                index++;
            }

            builder.Append("]");
            return builder.ToString();
        }

        internal static string FormatCompareMode(UICompareModeEnum compareMode)
        {
            switch (compareMode)
            {
                case UICompareModeEnum.Less:
                    return "<";
                case UICompareModeEnum.LessEqual:
                    return "<=";
                case UICompareModeEnum.Equal:
                    return "==";
                case UICompareModeEnum.Great:
                    return ">";
                case UICompareModeEnum.GreatEqual:
                    return ">=";
                default:
                    return compareMode.ToString();
            }
        }

        internal static string FormatEventMode(bool isTaskEvent)
        {
            return isTaskEvent ? "Async(异步)" : "Sync(同步)";
        }

        internal static string FormatEventParamTypes(IReadOnlyList<EUIEventParamType> paramTypes)
        {
            if (paramTypes == null)
            {
                return "null";
            }

            var builder = new StringBuilder();
            builder.Append("[");

            for (var i = 0; i < paramTypes.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append(paramTypes[i].GetParamTypeString());
            }

            builder.Append("]");
            return builder.ToString();
        }

        internal static bool EventParamTypesMatch(IReadOnlyList<EUIEventParamType> expected, IReadOnlyList<EUIEventParamType> actual)
        {
            if (expected == null || actual == null)
            {
                return false;
            }

            if (expected.Count != actual.Count)
            {
                return false;
            }

            for (var i = 0; i < expected.Count; i++)
            {
                if (expected[i] != actual[i])
                {
                    return false;
                }
            }

            return true;
        }

        internal static void AppendEventLine(StringBuilder builder, string eventName, UIEventBase uiEvent)
        {
            builder.Append("- ");
            builder.Append(string.IsNullOrEmpty(eventName) ? "<empty>" : eventName);

            if (uiEvent == null)
            {
                builder.AppendLine("：event=null");
                return;
            }

            builder.Append("：mode=");
            builder.Append(FormatEventMode(uiEvent.IsTaskEvent));
            builder.Append("，type=");
            builder.Append(uiEvent.GetEventType());
            builder.Append("，handle=");
            builder.Append(uiEvent.GetEventHandleType());
            builder.Append("，params=");
            builder.Append(FormatEventParamTypes(uiEvent.AllEventParamType));
            builder.Append("，binds=");
            builder.Append(uiEvent.GetBindCount());
            builder.AppendLine();
        }

        internal static string Quote(string value)
        {
            return "\"" + EscapeInline(value) + "\"";
        }

        internal static string TrimEndLine(StringBuilder builder)
        {
            return builder.ToString().TrimEnd();
        }

        private static string EscapeInline(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            return value.Replace("\\", "\\\\")
                        .Replace("\"", "\\\"")
                        .Replace("\r", "\\r")
                        .Replace("\n", "\\n");
        }
    }
}

#endif
