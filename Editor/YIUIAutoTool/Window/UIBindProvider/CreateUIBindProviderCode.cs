#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class CreateUIBindProviderCode : BaseTemplate
    {
        public override string EventName => "UI反射动态码";

        public override bool Cover => true;

        public override bool AutoRefresh => true;

        public override bool ShowTips => false;

        public CreateUIBindProviderCode(out bool result, string authorName, UIBindProviderData codeData) : base(authorName)
        {
            var path     = $"{string.Format(YIUIConstHelper.Const.UIETComponentGenPath, YIUIConstHelper.Const.UIETCreatePackageName)}/{codeData.Name}.cs";
            var template = $"{YIUIConstHelper.Const.UITemplatePath}/UIBindProviderTemplate.txt";
            CreateVo = new CreateVo(template, path);

            ValueDic["Count"]   = codeData.Count.ToString();
            ValueDic["Content"] = codeData.Content;

            result = CreateNewFile();
        }
    }
}
#endif
