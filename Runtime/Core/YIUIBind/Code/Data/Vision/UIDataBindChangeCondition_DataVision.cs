#if UNITY_EDITOR

namespace YIUIFramework
{
    public partial class UIDataBindChangeCondition
    {
        public override string GetYIUIVisionText()
        {
            var extraParams = "changeBind=" + YIUIVisionTextHelper.FormatUnityObject(m_UIDataBindChange);
            return GetYIUIDataBoolVisionText(GetType().Name, extraParams, "条件满足时调用 UIDataBindChange.ChangeDataValue 修改目标 Data");
        }
    }
}

#endif
