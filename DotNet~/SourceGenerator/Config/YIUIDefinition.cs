namespace ET
{
    public static class YIUIDiagnosticCategories
    {
        public const string Model = "YIUIETModelProjectAnalyzers";
    }

    public static class YIUIDiagnosticIds
    {
        public const string YIUIEntitySystemAnalyzerRuleId = "YIUI0001";
    }

    public static class YIUIDefinition
    {
        //IDynamicEvent
        public const string IDynamicEventInterface = "ET.IDynamicEvent";
        public const string IDynamicEventMethod    = "DynamicEvent|async ETTask";

        //IYIUIBackClose
        public const string IYIUIBackCloseInterface = "ET.Client.IYIUIBackClose";
        public const string IYIUIBackCloseMethod    = "YIUIBackClose|async ETTask";

        //IYIUIBackHomeClose
        public const string IYIUIBackHomeCloseInterface = "ET.Client.IYIUIBackHomeClose";
        public const string IYIUIBackHomeCloseMethod    = "YIUIBackHomeClose|async ETTask";

        //IYIUIBackHomeOpen
        public const string IYIUIBackHomeOpenInterface = "ET.Client.IYIUIBackHomeOpen";
        public const string IYIUIBackHomeOpenMethod    = "YIUIBackHomeOpen|async ETTask";

        //IYIUIBackOpen
        public const string IYIUIBackOpenInterface = "ET.Client.IYIUIBackOpen";
        public const string IYIUIBackOpenMethod    = "YIUIBackOpen|async ETTask";

        //IYIUIClose
        public const string IYIUICloseInterface = "ET.Client.IYIUIClose";
        public const string IYIUICloseMethod    = "YIUIClose|async ETTask<bool>";

        //IYIUIWindowClose
        //public const string IYIUIWindowCloseInterface = "ET.Client.IYIUIWindowClose";
        //public const string IYIUIWindowCloseMethod    = "YIUIWindowClose|async ETTask";

        //IYIUIDisable
        public const string IYIUIDisableInterface = "ET.Client.IYIUIDisable";
        public const string IYIUIDisableMethod    = "YIUIDisable";

        //IYIUIDisClose
        public const string IYIUIDisCloseInterface = "ET.Client.IYIUIDisClose";
        public const string IYIUIDisCloseMethod    = "YIUIDisClose|async ETTask<bool>";

        //IYIUIEnable
        public const string IYIUIEnableInterface = "ET.Client.IYIUIEnable";
        public const string IYIUIEnableMethod    = "YIUIEnable";

        //IYIUIInitialize
        public const string IYIUIInitializeInterface = "ET.Client.IYIUIInitialize";
        public const string IYIUIInitializeMethod    = "YIUIInitialize";

        //IYIUIOpen
        public const string IYIUIOpenInterface = "ET.Client.IYIUIOpen";
        public const string IYIUIOpenMethod    = "YIUIOpen|async ETTask<bool>";

        //IYIUICloseTweenEnd
        public const string IYIUICloseTweenEndInterface = "ET.Client.IYIUICloseTweenEnd";
        public const string IYIUICloseTweenEndMethod    = "YIUICloseTweenEnd";

        //IYIUICloseTween
        public const string IYIUICloseTweenInterface = "ET.Client.IYIUICloseTween";
        public const string IYIUICloseTweenMethod    = "YIUICloseTween|async ETTask";

        //IYIUIOpenTweenEnd
        public const string IYIUIOpenTweenEndInterface = "ET.Client.IYIUIOpenTweenEnd";
        public const string IYIUIOpenTweenEndMethod    = "YIUIOpenTweenEnd";

        //IYIUIOpenTween
        public const string IYIUIOpenTweenInterface = "ET.Client.IYIUIOpenTween";
        public const string IYIUIOpenTweenMethod    = "YIUIOpenTween|async ETTask";

        //IYIUIBind
        public const string IYIUIBindInterface = "ET.Client.IYIUIBind";
        public const string IYIUIBindMethod    = "YIUIBind";
    }
}