using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ET
{
    public static class YIUIComponentFieldAccessAnalyzerRule
    {
        private const string Title = "禁止在 YIUI ComponentSystem 外部直接访问组件字段";

        private const string MessageFormat = "YIUI 组件字段: {0}.{1} 只能在对应的 {2} 中访问，请改为通过属性对外暴露";

        private const string Description = "YIUI Component 上的字段只允许在对应的 ComponentSystem 中访问，外部只能访问属性。";

        public static readonly DiagnosticDescriptor Rule =
                new(YIUIDiagnosticIds.YIUIComponentFieldAccessAnalyzerRuleId,
                    Title,
                    MessageFormat,
                    YIUIDiagnosticCategories.Model,
                    DiagnosticSeverity.Error,
                    true,
                    Description);
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class YIUIComponentFieldAccessAnalyzer : DiagnosticAnalyzer
    {
        private const string YIUIAttribute = "YIUIFramework.YIUIAttribute";
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(YIUIComponentFieldAccessAnalyzerRule.Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (!AnalyzerGlobalSetting.EnableAnalyzer)
            {
                return;
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(this.AnalyzeIdentifierName, Microsoft.CodeAnalysis.CSharp.SyntaxKind.IdentifierName);
        }

        private void AnalyzeIdentifierName(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is not IdentifierNameSyntax identifierNameSyntax)
            {
                return;
            }

            if (context.SemanticModel.GetSymbolInfo(identifierNameSyntax).Symbol is not IFieldSymbol fieldSymbol)
            {
                return;
            }

            if (fieldSymbol.IsStatic)
            {
                return;
            }

            INamedTypeSymbol? componentTypeSymbol = fieldSymbol.ContainingType;
            if (componentTypeSymbol == null || !componentTypeSymbol.HasAttribute(YIUIAttribute))
            {
                return;
            }

            if (this.IsAllowedAccess(context, identifierNameSyntax, componentTypeSymbol))
            {
                return;
            }

            var systemTypeName = $"{componentTypeSymbol.Name}System";
            Diagnostic diagnostic = Diagnostic.Create(YIUIComponentFieldAccessAnalyzerRule.Rule, identifierNameSyntax.GetLocation(),
                componentTypeSymbol.Name, fieldSymbol.Name, systemTypeName);
            context.ReportDiagnostic(diagnostic);
        }

        private bool IsAllowedAccess(SyntaxNodeAnalysisContext context, SyntaxNode syntaxNode, INamedTypeSymbol componentTypeSymbol)
        {
            var containingTypeSymbol = context.SemanticModel.GetEnclosingSymbol(syntaxNode.SpanStart)?.ContainingType;
            if (containingTypeSymbol == null)
            {
                return false;
            }

            if (SymbolEqualityComparer.Default.Equals(containingTypeSymbol, componentTypeSymbol))
            {
                return true;
            }

            if (containingTypeSymbol.Name != $"{componentTypeSymbol.Name}System")
            {
                return false;
            }

            AttributeData? entitySystemOfAttribute = containingTypeSymbol.GetFirstAttribute(Definition.EntitySystemOfAttribute);
            if (entitySystemOfAttribute == null)
            {
                return false;
            }

            if (entitySystemOfAttribute.ConstructorArguments.Length == 0)
            {
                return false;
            }

            if (entitySystemOfAttribute.ConstructorArguments[0].Value is not INamedTypeSymbol systemTargetTypeSymbol)
            {
                return false;
            }

            return SymbolEqualityComparer.Default.Equals(systemTargetTypeSymbol, componentTypeSymbol);
        }
    }
}
