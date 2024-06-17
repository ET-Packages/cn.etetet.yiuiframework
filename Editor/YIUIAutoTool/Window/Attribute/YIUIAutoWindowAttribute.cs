using System;

namespace YIUIFramework.Editor
{
    public class YIUIAutoMenuAttribute : Attribute
    {
        public string MenuName;

        public YIUIAutoMenuAttribute(string menuName)
        {
            MenuName = menuName;
        }
    }
}
