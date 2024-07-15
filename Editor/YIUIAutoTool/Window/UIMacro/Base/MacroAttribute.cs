using System;

namespace YIUIFramework.Editor
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = true)]
    public class YIUIEnumMacroAttribute : Attribute
    {
        public YIUIMacroType MacroType { get; set; }

        public YIUIEnumMacroAttribute(YIUIMacroType macroType)
        {
            MacroType = macroType;
        }
    }
}
