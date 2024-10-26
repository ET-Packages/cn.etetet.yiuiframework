using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ET;

public static class YIUIEntitySystemAnalyzerRule
{
    private const string Title = "YIUI Entity类存在未生成的事件函数";

    private const string MessageFormat = "YIUI Entity类: {0} 存在未生成的事件函数";

    private const string Description = "YIUI Entity类存在未生成的事件函数.";

    public static readonly DiagnosticDescriptor Rule =
            new(YIUIDiagnosticIds.YIUIEntitySystemAnalyzerRuleId,
                Title,
                MessageFormat,
                YIUIDiagnosticCategories.Model,
                DiagnosticSeverity.Error,
                true,
                Description);
}

public static class YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRule
{
    private const string Title = "YIUI EntitySystem标签只能添加在含有EntitySystemOf标签的静态类中";

    private const string MessageFormat = "YIUI 方法:{0}的{1}标签只能添加在含有{2}标签的静态类中";

    private const string Description = "YIUI EntitySystem标签只能添加在含有EntitySystemOf标签的静态类中.";

    public static readonly DiagnosticDescriptor Rule =
            new(YIUIDiagnosticIds.YIUIEntitySystemAnalyzerRuleId,
                Title,
                MessageFormat,
                YIUIDiagnosticCategories.Model,
                DiagnosticSeverity.Error,
                true,
                Description);
}

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class YIUIEntitySystemAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(YIUIEntitySystemAnalyzerRule.Rule, YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRule.Rule);

    public override void Initialize(AnalysisContext context)
    {
        if (!AnalyzerGlobalSetting.EnableAnalyzer)
        {
            return;
        }

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSymbolAction(this.Analyzer, SymbolKind.NamedType);
        context.RegisterSymbolAction(this.AnalyzeIsSystemMethodValid, SymbolKind.NamedType);
    }

    private static ImmutableArray<ETSystemData> SupportedETSystemDatas => ImmutableArray.Create(new ETSystemData(Definition.EntitySystemOfAttribute, Definition.EntitySystemAttribute, Definition.EntityType, Definition.EntitySystemAttributeMetaName,

        //IDynamicEvent
        new SystemMethodData(YIUIDefinition.IDynamicEventInterface, YIUIDefinition.IDynamicEventMethod),

        //IYIUIBackClose
        new SystemMethodData(YIUIDefinition.IYIUIBackCloseInterface, YIUIDefinition.IYIUIBackCloseMethod),

        //IYIUIBackHomeClose
        new SystemMethodData(YIUIDefinition.IYIUIBackHomeCloseInterface, YIUIDefinition.IYIUIBackHomeCloseMethod),

        //IYIUIBackHomeOpen
        new SystemMethodData(YIUIDefinition.IYIUIBackHomeOpenInterface, YIUIDefinition.IYIUIBackHomeOpenMethod),

        //IYIUIBackOpen
        new SystemMethodData(YIUIDefinition.IYIUIBackOpenInterface, YIUIDefinition.IYIUIBackOpenMethod),

        //IYIUIClose
        new SystemMethodData(YIUIDefinition.IYIUICloseInterface, YIUIDefinition.IYIUICloseMethod),

        //IYIUIWindowClose
        //new SystemMethodData(YIUIDefinition.IYIUIWindowCloseInterface, YIUIDefinition.IYIUIWindowCloseMethod),
        //IYIUIDisable
        new SystemMethodData(YIUIDefinition.IYIUIDisableInterface, YIUIDefinition.IYIUIDisableMethod),

        //IYIUIDisClose
        new SystemMethodData(YIUIDefinition.IYIUIDisCloseInterface, YIUIDefinition.IYIUIDisCloseMethod),

        //IYIUIEnable
        new SystemMethodData(YIUIDefinition.IYIUIEnableInterface, YIUIDefinition.IYIUIEnableMethod),

        //IYIUIInitialize
        new SystemMethodData(YIUIDefinition.IYIUIInitializeInterface, YIUIDefinition.IYIUIInitializeMethod),

        //IYIUIOpen
        new SystemMethodData(YIUIDefinition.IYIUIOpenInterface, YIUIDefinition.IYIUIOpenMethod),

        //IYIUICloseTweenEnd
        new SystemMethodData(YIUIDefinition.IYIUICloseTweenEndInterface, YIUIDefinition.IYIUICloseTweenEndMethod),

        //IYIUICloseTween
        new SystemMethodData(YIUIDefinition.IYIUICloseTweenInterface, YIUIDefinition.IYIUICloseTweenMethod),

        //IYIUIOpenTweenEnd
        new SystemMethodData(YIUIDefinition.IYIUIOpenTweenEndInterface, YIUIDefinition.IYIUIOpenTweenEndMethod),

        //IYIUIOpenTween
        new SystemMethodData(YIUIDefinition.IYIUIOpenTweenInterface, YIUIDefinition.IYIUIOpenTweenMethod),

        //IYIUIBind
        new SystemMethodData(YIUIDefinition.IYIUIBindInterface, YIUIDefinition.IYIUIBindMethod)));

    private class ETSystemData
    {
        public string             EntityTypeName;
        public string             SystemOfAttribute;
        public string             SystemAttributeShowName;
        public string             SystemAttributeMetaName;
        public SystemMethodData[] SystemMethods;

        public ETSystemData(string systemOfAttribute, string systemAttributeShowName, string entityTypeName, string systemAttributeMetaName, params SystemMethodData[] systemMethods)
        {
            this.SystemOfAttribute       = systemOfAttribute;
            this.SystemAttributeShowName = systemAttributeShowName;
            this.EntityTypeName          = entityTypeName;
            this.SystemAttributeMetaName = systemAttributeMetaName;
            this.SystemMethods           = systemMethods;
        }
    }

    private struct SystemMethodData
    {
        public string InterfaceName;
        public string MethodName;

        public SystemMethodData(string interfaceName, string methodName)
        {
            this.InterfaceName = interfaceName;
            this.MethodName    = methodName;
        }
    }

    private void Analyzer(SymbolAnalysisContext context)
    {
        if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
        {
            return;
        }

        ImmutableDictionary<string, string?>.Builder? builder = null;
        foreach (ETSystemData? supportedEtSystemData in SupportedETSystemDatas)
        {
            if (supportedEtSystemData != null)
            {
                this.AnalyzeETSystem(context, supportedEtSystemData, ref builder);
            }
        }

        this.ReportNeedGenerateSystem(context, namedTypeSymbol, ref builder);
    }

    private void AnalyzeETSystem(SymbolAnalysisContext context, ETSystemData etSystemData, ref ImmutableDictionary<string, string?>.Builder? builder)
    {
        if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
        {
            return;
        }

        // 筛选出含有SystemOf标签的类
        AttributeData? attr = namedTypeSymbol.GetFirstAttribute(etSystemData.SystemOfAttribute);
        if (attr == null)
        {
            return;
        }

        // 获取所属的实体类symbol
        if (attr.ConstructorArguments[0].Value is not INamedTypeSymbol entityTypeSymbol)
        {
            return;
        }

        bool ignoreAwake = false;
        if (attr.ConstructorArguments.Length >= 2 && attr.ConstructorArguments[1].Value is bool ignore)
        {
            ignoreAwake = ignore;
        }

        // 排除非Entity子类
        if (entityTypeSymbol.BaseType?.ToString() != etSystemData.EntityTypeName)
        {
            return;
        }

        foreach (INamedTypeSymbol? interfaceTypeSymbol in entityTypeSymbol.AllInterfaces)
        {
            if (ignoreAwake && interfaceTypeSymbol.IsInterface(Definition.IAwakeInterface))
            {
                continue;
            }

            foreach (SystemMethodData systemMethodData in etSystemData.SystemMethods)
            {
                if (interfaceTypeSymbol.IsInterface(systemMethodData.InterfaceName))
                {
                    var methodName = systemMethodData.MethodName.Split('|')[0];

                    if (interfaceTypeSymbol.IsGenericType)
                    {
                        var typeArgs = ImmutableArray.Create<ITypeSymbol>(entityTypeSymbol).AddRange(interfaceTypeSymbol.TypeArguments);
                        if (!namedTypeSymbol.HasMethodWithParams(methodName, typeArgs.ToArray()))
                        {
                            StringBuilder str = new();
                            str.Append(entityTypeSymbol);
                            str.Append("/");
                            str.Append(etSystemData.SystemAttributeShowName);
                            foreach (ITypeSymbol? typeArgument in interfaceTypeSymbol.TypeArguments)
                            {
                                str.Append("/");
                                str.Append(typeArgument);
                            }

                            AddProperty(ref builder, $"{systemMethodData.MethodName}`{interfaceTypeSymbol.TypeArguments.Length}", str.ToString());
                        }
                    }
                    else
                    {
                        if (interfaceTypeSymbol.IsInterface(Definition.IGetComponentInterface))
                        {
                            if (!namedTypeSymbol.HasMethodWithParams(methodName, entityTypeSymbol.ToString(), "System.Type"))
                            {
                                AddProperty(ref builder, systemMethodData.MethodName, $"{entityTypeSymbol}/{etSystemData.SystemAttributeShowName}/System.Type");
                            }
                        }
                        else if (!namedTypeSymbol.HasMethodWithParams(methodName, entityTypeSymbol))
                        {
                            AddProperty(ref builder, systemMethodData.MethodName, $"{entityTypeSymbol}/{etSystemData.SystemAttributeShowName}");
                        }
                    }

                    break;
                }
            }
        }
    }

    private void AddProperty(ref ImmutableDictionary<string, string?>.Builder? builder, string methodMetaName, string methodArgs)
    {
        if (builder == null)
        {
            builder = ImmutableDictionary.CreateBuilder<string, string?>();
        }

        if (builder.TryGetValue(Definition.EntitySystemInterfaceSequence, out string? seqValue))
        {
            builder[Definition.EntitySystemInterfaceSequence] = $"{seqValue}/{methodMetaName}";
        }
        else
        {
            builder.Add(Definition.EntitySystemInterfaceSequence, methodMetaName);
        }

        builder.Add(methodMetaName, methodArgs);
    }

    private void ReportNeedGenerateSystem(SymbolAnalysisContext context, INamedTypeSymbol namedTypeSymbol, ref ImmutableDictionary<string, string?>.Builder? builder)
    {
        if (builder == null)
        {
            return;
        }

        foreach (SyntaxReference? reference in namedTypeSymbol.DeclaringSyntaxReferences)
        {
            if (reference.GetSyntax() is ClassDeclarationSyntax classDeclarationSyntax)
            {
                Diagnostic diagnostic = Diagnostic.Create(YIUIEntitySystemAnalyzerRule.Rule, classDeclarationSyntax.Identifier.GetLocation(),
                    builder.ToImmutable(), namedTypeSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private void AnalyzeIsSystemMethodValid(SymbolAnalysisContext context)
    {
        if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
        {
            return;
        }

        foreach (ISymbol? symbol in namedTypeSymbol.GetMembers())
        {
            if (symbol is not IMethodSymbol methodSymbol)
            {
                continue;
            }

            foreach (var etSystemData in SupportedETSystemDatas)
            {
                if (!methodSymbol.HasAttribute(etSystemData.SystemAttributeMetaName))
                {
                    continue;
                }

                if (methodSymbol.Parameters.Length == 0)
                {
                    continue;
                }

                AttributeData? attr = namedTypeSymbol.GetFirstAttribute(etSystemData.SystemOfAttribute);
                if (attr == null || attr.ConstructorArguments[0].Value is not INamedTypeSymbol entityTypeSymbol
                 || entityTypeSymbol.ToString() != methodSymbol.Parameters[0].Type.ToString())
                {
                    ReportNeedSystemOfAttr(context, methodSymbol, etSystemData);
                }
            }
        }
    }

    private void ReportNeedSystemOfAttr(SymbolAnalysisContext context, IMethodSymbol methodSymbol, ETSystemData etSystemData)
    {
        foreach (SyntaxReference? reference in methodSymbol.DeclaringSyntaxReferences)
        {
            if (reference.GetSyntax() is MethodDeclarationSyntax methodDeclarationSyntax)
            {
                Diagnostic diagnostic = Diagnostic.Create(YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRule.Rule, methodDeclarationSyntax.Identifier.GetLocation()
                  , methodSymbol.Name, etSystemData.SystemAttributeShowName, etSystemData.SystemOfAttribute);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}