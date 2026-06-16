namespace ET
{
    public static class YIUIDiagnosticCategories
    {
        public const string Model = "YIUIETModelProjectAnalyzers";
    }

    public static class YIUIDefinition
    {
        public const string IgnoreYIUIEventPublishCheckAttribute = "ET.IgnoreYIUIEventPublishCheckAttribute";
    }

    public static class YIUIDiagnosticIds
    {
        public const string YIUIEntitySystemAnalyzerRuleId                       = "YIUI0001";
        public const string YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRuleId = "YIUI0002";
        public const string YIUIComponentFieldAccessAnalyzerRuleId               = "YIUI0003";
        public const string YIUIETEventWithoutPublisherAnalyzerRuleId            = "YIUI0004";
        public const string YIUIDynamicEventWithoutPublisherAnalyzerRuleId       = "YIUI0005";
    }
}
