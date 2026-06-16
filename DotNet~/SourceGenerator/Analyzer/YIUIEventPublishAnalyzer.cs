using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace ET
{
    public static class YIUIETEventWithoutPublisherAnalyzerRule
    {
        private const string Title = "ET 事件存在监听但没有任何发送";

        private const string MessageFormat = "ET 事件消息: {0} 被监听于 {1}，但项目中没有任何 EventSystem.Publish/PublishAsync 或 PublishAndDynamicEvent 发送";

        private const string Description = "监听 AEvent<Scene, T> 时，项目中至少要有一个对应的 ET 事件发送入口。";

        public static readonly DiagnosticDescriptor Rule =
                new(YIUIDiagnosticIds.YIUIETEventWithoutPublisherAnalyzerRuleId,
                    Title,
                    MessageFormat,
                    YIUIDiagnosticCategories.Model,
                    DiagnosticSeverity.Error,
                    true,
                    Description);
    }

    public static class YIUIDynamicEventWithoutPublisherAnalyzerRule
    {
        private const string Title = "动态事件存在监听但没有任何发送";

        private const string MessageFormat = "动态事件消息: {0} 被监听于 {1}，但项目中没有任何 DynamicEvent 或 PublishAndDynamicEvent 发送";

        private const string Description = "监听 IDynamicEvent<T> 时，项目中至少要有一个对应的动态事件发送入口。";

        public static readonly DiagnosticDescriptor Rule =
                new(YIUIDiagnosticIds.YIUIDynamicEventWithoutPublisherAnalyzerRuleId,
                    Title,
                    MessageFormat,
                    YIUIDiagnosticCategories.Model,
                    DiagnosticSeverity.Error,
                    true,
                    Description);
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class YIUIEventPublishAnalyzer : DiagnosticAnalyzer
    {
        private const string SceneType = "ET.Scene";
        private const string AEventType = "ET.AEvent<S, A>";
        private const string DynamicEventInterfaceType = "ET.IDynamicEvent<A>";
        private static readonly CSharpParseOptions GlobalParseOptions = new(preprocessorSymbols: new[] { "UNITY_EDITOR" });

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
            ImmutableArray.Create(YIUIETEventWithoutPublisherAnalyzerRule.Rule, YIUIDynamicEventWithoutPublisherAnalyzerRule.Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (!AnalyzerGlobalSetting.EnableAnalyzer)
            {
                return;
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterCompilationStartAction(this.InitializeCompilation);
        }

        private void InitializeCompilation(CompilationStartAnalysisContext context)
        {
            var projectRoot = FindProjectRoot(context.Compilation.SyntaxTrees);
            var globalEventIndex = GetGlobalEventIndex(projectRoot);
            var etPublishedEvents = new ConcurrentDictionary<string, byte>(System.StringComparer.Ordinal);
            var dynamicPublishedEvents = new ConcurrentDictionary<string, byte>(System.StringComparer.Ordinal);
            var etEventListeners = new ConcurrentBag<ListenerRegistration>();
            var dynamicEventListeners = new ConcurrentBag<ListenerRegistration>();

            context.RegisterSyntaxNodeAction(
                syntaxContext => AnalyzeClassDeclarationSyntax(syntaxContext, etEventListeners),
                SyntaxKind.ClassDeclaration);
            context.RegisterSymbolAction(
                symbolContext => AnalyzeDynamicEventMethod(symbolContext, dynamicEventListeners),
                SymbolKind.Method);
            context.RegisterOperationAction(
                operationContext => AnalyzeInvocation(operationContext, etPublishedEvents, dynamicPublishedEvents),
                OperationKind.Invocation);
            context.RegisterCompilationEndAction(
                endContext => AnalyzeCompilationEnd(endContext, globalEventIndex, etPublishedEvents, dynamicPublishedEvents, etEventListeners, dynamicEventListeners));
        }

        private static void AnalyzeClassDeclarationSyntax(
            SyntaxNodeAnalysisContext context,
            ConcurrentBag<ListenerRegistration> listeners)
        {
            if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax)
            {
                return;
            }

            if (context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol namedTypeSymbol)
            {
                return;
            }

            if (namedTypeSymbol.HasAttribute(YIUIDefinition.IgnoreYIUIEventPublishCheckAttribute))
            {
                return;
            }

            AnalyzeETEventListenerSyntax(context, classDeclarationSyntax, namedTypeSymbol, listeners);
        }

        private static void AnalyzeETEventListenerSyntax(
            SyntaxNodeAnalysisContext context,
            ClassDeclarationSyntax classDeclarationSyntax,
            INamedTypeSymbol namedTypeSymbol,
            ConcurrentBag<ListenerRegistration> listeners)
        {
            if (classDeclarationSyntax.BaseList == null)
            {
                return;
            }

            foreach (var baseTypeSyntax in classDeclarationSyntax.BaseList.Types)
            {
                if (baseTypeSyntax.Type is not GenericNameSyntax genericNameSyntax)
                {
                    continue;
                }

                if (genericNameSyntax.Identifier.ValueText != "AEvent" || genericNameSyntax.TypeArgumentList.Arguments.Count != 2)
                {
                    continue;
                }

                var baseTypeSymbol = context.SemanticModel.GetSymbolInfo(genericNameSyntax).Symbol as INamedTypeSymbol;
                if (baseTypeSymbol == null || !baseTypeSymbol.IsGenericType || baseTypeSymbol.OriginalDefinition.ToString() != AEventType)
                {
                    continue;
                }

                if (baseTypeSymbol.TypeArguments[0].ToString() != SceneType)
                {
                    continue;
                }

                if (baseTypeSymbol.TypeArguments[1] is INamedTypeSymbol eventTypeSymbol)
                {
                    listeners.Add(new ListenerRegistration(
                        ListenerKind.ETEvent,
                        eventTypeSymbol.ToString(),
                        eventTypeSymbol.Name,
                        namedTypeSymbol.ToString(),
                        baseTypeSyntax.GetLocation()));
                }
            }
        }

        private static void AnalyzeDynamicEventMethod(
            SymbolAnalysisContext context,
            ConcurrentBag<ListenerRegistration> listeners)
        {
            if (context.Symbol is not IMethodSymbol methodSymbol)
            {
                return;
            }

            if (methodSymbol.Name != "DynamicEvent")
            {
                return;
            }

            if (!methodSymbol.HasAttribute(Definition.EntitySystemAttributeMetaName))
            {
                return;
            }

            if (methodSymbol.HasAttribute(YIUIDefinition.IgnoreYIUIEventPublishCheckAttribute))
            {
                return;
            }

            if (methodSymbol.Parameters.Length != 2)
            {
                return;
            }

            if (methodSymbol.Parameters[0].Type is not INamedTypeSymbol componentTypeSymbol)
            {
                return;
            }

            if (methodSymbol.Parameters[1].Type is not INamedTypeSymbol eventTypeSymbol)
            {
                return;
            }

            if (!ImplementsDynamicEvent(componentTypeSymbol, eventTypeSymbol))
            {
                return;
            }

            listeners.Add(new ListenerRegistration(
                ListenerKind.DynamicEvent,
                eventTypeSymbol.ToString(),
                eventTypeSymbol.Name,
                methodSymbol.ToDisplayString(),
                methodSymbol.Locations.First(),
                ReportTarget.Method));
        }

        private static bool ImplementsDynamicEvent(INamedTypeSymbol componentTypeSymbol, INamedTypeSymbol eventTypeSymbol)
        {
            foreach (var interfaceSymbol in componentTypeSymbol.AllInterfaces)
            {
                if (!interfaceSymbol.IsGenericType || interfaceSymbol.OriginalDefinition.ToString() != DynamicEventInterfaceType)
                {
                    continue;
                }

                if (interfaceSymbol.TypeArguments.Length != 1)
                {
                    continue;
                }

                if (SymbolEqualityComparer.Default.Equals(interfaceSymbol.TypeArguments[0], eventTypeSymbol))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AnalyzeInvocation(
            OperationAnalysisContext context,
            ConcurrentDictionary<string, byte> etPublishedEvents,
            ConcurrentDictionary<string, byte> dynamicPublishedEvents)
        {
            if (context.Operation is not IInvocationOperation invocationOperation)
            {
                return;
            }

            var targetMethod = invocationOperation.TargetMethod.ReducedFrom ?? invocationOperation.TargetMethod;
            switch (targetMethod.Name)
            {
                case "Publish":
                case "PublishAsync":
                    if (invocationOperation.Arguments.Length >= 2)
                    {
                        AddTypeSymbolName(etPublishedEvents, invocationOperation.Arguments[invocationOperation.Arguments.Length - 1].Value.Type);
                    }

                    break;
                case "DynamicEvent":
                    if (targetMethod.IsGenericMethod && targetMethod.TypeArguments.Length == 1)
                    {
                        AddTypeSymbolName(dynamicPublishedEvents, targetMethod.TypeArguments[0]);
                    }
                    else if (invocationOperation.Arguments.Length >= 1)
                    {
                        AddTypeSymbolName(dynamicPublishedEvents, invocationOperation.Arguments[invocationOperation.Arguments.Length - 1].Value.Type);
                    }

                    break;
                case "PublishAndDynamicEvent":
                    if (targetMethod.IsGenericMethod && targetMethod.TypeArguments.Length == 1)
                    {
                        AddTypeSymbolName(etPublishedEvents, targetMethod.TypeArguments[0]);
                        AddTypeSymbolName(dynamicPublishedEvents, targetMethod.TypeArguments[0]);
                    }
                    else if (invocationOperation.Arguments.Length >= 1)
                    {
                        var messageType = invocationOperation.Arguments[invocationOperation.Arguments.Length - 1].Value.Type;
                        AddTypeSymbolName(etPublishedEvents, messageType);
                        AddTypeSymbolName(dynamicPublishedEvents, messageType);
                    }

                    break;
            }
        }

        private static void AnalyzeCompilationEnd(
            CompilationAnalysisContext context,
            GlobalEventIndex globalEventIndex,
            ConcurrentDictionary<string, byte> etPublishedEvents,
            ConcurrentDictionary<string, byte> dynamicPublishedEvents,
            ConcurrentBag<ListenerRegistration> etEventListeners,
            ConcurrentBag<ListenerRegistration> dynamicEventListeners)
        {
            var compilationEventIndex = new GlobalEventIndex(
                new HashSet<string>(etPublishedEvents.Keys, System.StringComparer.Ordinal),
                new HashSet<string>(dynamicPublishedEvents.Keys, System.StringComparer.Ordinal));
            var eventIndex = GlobalEventIndex.Merge(compilationEventIndex, globalEventIndex);

            foreach (var listener in etEventListeners)
            {
                if (eventIndex.ContainsETPublisher(listener.EventFullName, listener.EventShortName))
                {
                    continue;
                }

                context.ReportDiagnostic(Diagnostic.Create(
                    YIUIETEventWithoutPublisherAnalyzerRule.Rule,
                    listener.Location,
                    listener.EventFullName,
                    listener.ListenerTypeName));
            }

            foreach (var listener in dynamicEventListeners)
            {
                if (eventIndex.ContainsDynamicPublisher(listener.EventFullName, listener.EventShortName))
                {
                    continue;
                }

                context.ReportDiagnostic(Diagnostic.Create(
                    YIUIDynamicEventWithoutPublisherAnalyzerRule.Rule,
                    listener.Location,
                    listener.EventFullName,
                    listener.ListenerTypeName));
            }
        }

        private static string? FindProjectRoot(IEnumerable<SyntaxTree> syntaxTrees)
        {
            var samplePath = syntaxTrees.Select(static x => x.FilePath).FirstOrDefault(static x => !string.IsNullOrWhiteSpace(x));
            if (string.IsNullOrWhiteSpace(samplePath))
            {
                return null;
            }

            var current = new DirectoryInfo(Path.GetDirectoryName(samplePath)!);
            while (current != null)
            {
                if (Directory.Exists(Path.Combine(current.FullName, "Packages")))
                {
                    return current.FullName;
                }

                current = current.Parent;
            }

            return null;
        }

        private static GlobalEventIndex GetGlobalEventIndex(string? projectRoot)
        {
            if (string.IsNullOrWhiteSpace(projectRoot) || !Directory.Exists(projectRoot))
            {
                return GlobalEventIndex.Empty;
            }

            return BuildGlobalEventIndex(projectRoot!);
        }

        private static GlobalEventIndex BuildGlobalEventIndex(string projectRoot)
        {
            var etPublishedEvents = new HashSet<string>(System.StringComparer.Ordinal);
            var dynamicPublishedEvents = new HashSet<string>(System.StringComparer.Ordinal);
            var parsedFiles = Directory.EnumerateFiles(projectRoot, "*.cs", SearchOption.AllDirectories)
                .Where(static file => !IsIgnoredPath(file))
                .Select(ParseSourceFile)
                .ToArray();
            var methodReturnTypes = BuildMethodReturnTypeIndex(parsedFiles);
            var walker = new PublisherSyntaxWalker(etPublishedEvents, dynamicPublishedEvents, methodReturnTypes);

            foreach (var parsedFile in parsedFiles)
            {
                walker.Visit(parsedFile.Root);
            }

            return new GlobalEventIndex(etPublishedEvents, dynamicPublishedEvents);
        }

        private static ParsedSourceFile ParseSourceFile(string file)
        {
            var text = File.ReadAllText(file);
            var syntaxTree = CSharpSyntaxTree.ParseText(text, GlobalParseOptions, path: file);
            return new ParsedSourceFile(syntaxTree.GetRoot(), GetNamespaceName(syntaxTree.GetRoot()));
        }

        private static void AddTypeSymbolName(ConcurrentDictionary<string, byte> target, ITypeSymbol? typeSymbol)
        {
            if (typeSymbol == null)
            {
                return;
            }

            AddTypeName(target, typeSymbol.ToString());
        }

        private static Dictionary<string, string> BuildMethodReturnTypeIndex(IEnumerable<ParsedSourceFile> parsedFiles)
        {
            var result = new Dictionary<string, string>(System.StringComparer.Ordinal);

            foreach (var parsedFile in parsedFiles)
            {
                foreach (var typeDeclaration in parsedFile.Root.DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    var typeName = typeDeclaration.Identifier.ValueText;
                    foreach (var methodDeclaration in typeDeclaration.Members.OfType<MethodDeclarationSyntax>())
                    {
                        var returnType = NormalizeTypeName(methodDeclaration.ReturnType.ToString());
                        if (string.IsNullOrEmpty(returnType))
                        {
                            continue;
                        }

                        var methodName = methodDeclaration.Identifier.ValueText;
                        AddMethodReturnType(result, methodName, returnType);
                        AddMethodReturnType(result, $"{typeName}.{methodName}", returnType);

                        if (!string.IsNullOrEmpty(parsedFile.NamespaceName))
                        {
                            AddMethodReturnType(result, $"{parsedFile.NamespaceName}.{typeName}.{methodName}", returnType);
                        }
                    }
                }
            }

            return result;
        }

        private static string GetNamespaceName(SyntaxNode root)
        {
            var namespaceNode = root.DescendantNodes()
                .FirstOrDefault(static node => node.IsKind(SyntaxKind.NamespaceDeclaration) || node.IsKind(SyntaxKind.FileScopedNamespaceDeclaration));
            if (namespaceNode == null)
            {
                return string.Empty;
            }

            switch (namespaceNode)
            {
                case NamespaceDeclarationSyntax namespaceDeclarationSyntax:
                    return namespaceDeclarationSyntax.Name.ToString();
                case FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax:
                    return fileScopedNamespaceDeclarationSyntax.Name.ToString();
                default:
                    return string.Empty;
            }
        }

        private static void AddMethodReturnType(Dictionary<string, string> result, string key, string returnType)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(returnType))
            {
                return;
            }

            result[key] = returnType;
        }

        private static bool IsIgnoredPath(string filePath)
        {
            return filePath.Contains(@"\obj\") ||
                   filePath.Contains(@"\Library\") ||
                   filePath.Contains(@"\Temp\") ||
                   filePath.Contains(@"\Logs\") ||
                   filePath.Contains(@"\Bin\") ||
                   filePath.Contains(@"\HybridCLRData\");
        }

        private sealed class GlobalEventIndex
        {
            public static readonly GlobalEventIndex Empty = new(new HashSet<string>(System.StringComparer.Ordinal), new HashSet<string>(System.StringComparer.Ordinal));

            private readonly HashSet<string> etPublishedEvents;
            private readonly HashSet<string> dynamicPublishedEvents;

            public GlobalEventIndex(HashSet<string> etPublishedEvents, HashSet<string> dynamicPublishedEvents)
            {
                this.etPublishedEvents = etPublishedEvents;
                this.dynamicPublishedEvents = dynamicPublishedEvents;
            }

            public bool ContainsETPublisher(string fullName, string shortName)
            {
                return this.etPublishedEvents.Contains(fullName) || this.etPublishedEvents.Contains(shortName);
            }

            public bool ContainsDynamicPublisher(string fullName, string shortName)
            {
                return this.dynamicPublishedEvents.Contains(fullName) || this.dynamicPublishedEvents.Contains(shortName);
            }

            public static GlobalEventIndex Merge(GlobalEventIndex first, GlobalEventIndex second)
            {
                var etPublishedEvents = new HashSet<string>(first.etPublishedEvents);
                etPublishedEvents.UnionWith(second.etPublishedEvents);

                var dynamicPublishedEvents = new HashSet<string>(first.dynamicPublishedEvents);
                dynamicPublishedEvents.UnionWith(second.dynamicPublishedEvents);

                return new GlobalEventIndex(etPublishedEvents, dynamicPublishedEvents);
            }
        }

        private readonly struct ParsedSourceFile
        {
            public ParsedSourceFile(SyntaxNode root, string namespaceName)
            {
                this.Root = root;
                this.NamespaceName = namespaceName;
            }

            public SyntaxNode Root { get; }

            public string NamespaceName { get; }
        }

        private readonly struct ListenerRegistration
        {
            public ListenerRegistration(ListenerKind kind, string eventFullName, string eventShortName, string listenerTypeName, Location location, ReportTarget reportTarget = ReportTarget.Class)
            {
                this.Kind = kind;
                this.EventFullName = eventFullName;
                this.EventShortName = eventShortName;
                this.ListenerTypeName = listenerTypeName;
                this.Location = location;
                this.ReportTarget = reportTarget;
            }

            public ListenerKind Kind { get; }

            public string EventFullName { get; }

            public string EventShortName { get; }

            public string ListenerTypeName { get; }

            public Location Location { get; }

            public ReportTarget ReportTarget { get; }
        }

        private enum ListenerKind
        {
            ETEvent,
            DynamicEvent,
        }

        private enum ReportTarget
        {
            Class,
            Method,
        }

        private sealed class PublisherSyntaxWalker : CSharpSyntaxWalker
        {
            private readonly HashSet<string> etPublishedEvents;
            private readonly HashSet<string> dynamicPublishedEvents;
            private readonly Dictionary<string, string> methodReturnTypes;

            public PublisherSyntaxWalker(HashSet<string> etPublishedEvents, HashSet<string> dynamicPublishedEvents, Dictionary<string, string> methodReturnTypes)
            {
                this.etPublishedEvents = etPublishedEvents;
                this.dynamicPublishedEvents = dynamicPublishedEvents;
                this.methodReturnTypes = methodReturnTypes;
            }

            public override void VisitInvocationExpression(InvocationExpressionSyntax node)
            {
                var methodName = GetMethodName(node.Expression, out var genericArguments);
                switch (methodName)
                {
                    case "Publish":
                    case "PublishAsync":
                        if (node.ArgumentList.Arguments.Count >= 2)
                        {
                            AddPublishedEvent(this.etPublishedEvents, this.methodReturnTypes, genericArguments, node.ArgumentList.Arguments[node.ArgumentList.Arguments.Count - 1].Expression, node);
                        }

                        break;
                    case "DynamicEvent":
                        if (node.ArgumentList.Arguments.Count >= 1)
                        {
                            AddPublishedEvent(this.dynamicPublishedEvents, this.methodReturnTypes, genericArguments, node.ArgumentList.Arguments[node.ArgumentList.Arguments.Count - 1].Expression, node);
                        }

                        break;
                    case "PublishAndDynamicEvent":
                        if (node.ArgumentList.Arguments.Count >= 1)
                        {
                            var lastArgumentExpression = node.ArgumentList.Arguments[node.ArgumentList.Arguments.Count - 1].Expression;
                            AddPublishedEvent(this.etPublishedEvents, this.methodReturnTypes, genericArguments, lastArgumentExpression, node);
                            AddPublishedEvent(this.dynamicPublishedEvents, this.methodReturnTypes, genericArguments, lastArgumentExpression, node);
                        }

                        break;
                }

                base.VisitInvocationExpression(node);
            }

            private static void AddPublishedEvent(HashSet<string> target, Dictionary<string, string> methodReturnTypes, TypeArgumentListSyntax? genericArguments, ExpressionSyntax expression, InvocationExpressionSyntax invocation)
            {
                if (genericArguments != null && genericArguments.Arguments.Count == 1)
                {
                    AddTypeName(target, genericArguments.Arguments[0].ToString());
                    return;
                }

                if (TryGetTypeNameFromExpression(expression, invocation, methodReturnTypes, out var typeName))
                {
                    AddTypeName(target, typeName);
                }
            }

            private static bool TryGetTypeNameFromExpression(ExpressionSyntax expression, InvocationExpressionSyntax invocation, Dictionary<string, string> methodReturnTypes, out string typeName)
            {
                switch (expression)
                {
                    case ObjectCreationExpressionSyntax objectCreation:
                        typeName = objectCreation.Type.ToString();
                        return true;
                    case ImplicitObjectCreationExpressionSyntax:
                        typeName = string.Empty;
                        return false;
                    case CastExpressionSyntax castExpression:
                        typeName = castExpression.Type.ToString();
                        return true;
                    case IdentifierNameSyntax identifierName:
                        return TryResolveIdentifierType(invocation, identifierName.Identifier.ValueText, methodReturnTypes, out typeName);
                    case InvocationExpressionSyntax invocationExpressionSyntax:
                        return TryResolveInvocationReturnType(invocationExpressionSyntax, methodReturnTypes, out typeName);
                    default:
                        typeName = string.Empty;
                        return false;
                }
            }

            private static bool TryResolveIdentifierType(SyntaxNode node, string identifierName, Dictionary<string, string> methodReturnTypes, out string typeName)
            {
                foreach (var parameterList in node.Ancestors().OfType<BaseMethodDeclarationSyntax>().Select(static x => x.ParameterList))
                {
                    foreach (var parameter in parameterList.Parameters)
                    {
                        if (parameter.Identifier.ValueText != identifierName || parameter.Type == null)
                        {
                            continue;
                        }

                        typeName = parameter.Type.ToString();
                        return true;
                    }
                }

                var methodNode = node.Ancestors().OfType<BaseMethodDeclarationSyntax>().FirstOrDefault();
                if (methodNode != null)
                {
                    foreach (var declarator in methodNode.DescendantNodes().OfType<VariableDeclaratorSyntax>())
                    {
                        if (declarator.Identifier.ValueText != identifierName || declarator.SpanStart >= node.SpanStart)
                        {
                            continue;
                        }

                        if (declarator.Parent?.Parent is VariableDeclarationSyntax variableDeclaration)
                        {
                            var declaredType = NormalizeTypeName(variableDeclaration.Type.ToString());
                            if (declaredType != "var")
                            {
                                typeName = declaredType;
                                return true;
                            }

                            if (declarator.Initializer != null &&
                                TryGetTypeNameFromExpression(declarator.Initializer.Value, node as InvocationExpressionSyntax ?? declarator.Initializer.Value as InvocationExpressionSyntax ?? node.DescendantNodes().OfType<InvocationExpressionSyntax>().FirstOrDefault(), methodReturnTypes, out typeName))
                            {
                                return true;
                            }
                        }
                    }
                }

                typeName = string.Empty;
                return false;
            }

            private static bool TryResolveInvocationReturnType(InvocationExpressionSyntax invocationExpressionSyntax, Dictionary<string, string> methodReturnTypes, out string typeName)
            {
                var methodKey = GetInvocationMethodKey(invocationExpressionSyntax.Expression);
                if (!string.IsNullOrEmpty(methodKey) && methodReturnTypes.TryGetValue(methodKey, out typeName))
                {
                    return true;
                }

                typeName = string.Empty;
                return false;
            }

            private static string GetInvocationMethodKey(ExpressionSyntax expression)
            {
                switch (expression)
                {
                    case MemberAccessExpressionSyntax memberAccessExpressionSyntax:
                        return $"{memberAccessExpressionSyntax.Expression}.{memberAccessExpressionSyntax.Name.Identifier.ValueText}";
                    case IdentifierNameSyntax identifierNameSyntax:
                        return identifierNameSyntax.Identifier.ValueText;
                    case GenericNameSyntax genericNameSyntax:
                        return genericNameSyntax.Identifier.ValueText;
                    default:
                        return string.Empty;
                }
            }

            private static string GetMethodName(ExpressionSyntax expression, out TypeArgumentListSyntax? genericArguments)
            {
                genericArguments = null;
                switch (expression)
                {
                    case GenericNameSyntax genericName:
                        genericArguments = genericName.TypeArgumentList;
                        return genericName.Identifier.ValueText;
                    case IdentifierNameSyntax identifierName:
                        return identifierName.Identifier.ValueText;
                    case MemberAccessExpressionSyntax memberAccess:
                        if (memberAccess.Name is GenericNameSyntax memberGenericName)
                        {
                            genericArguments = memberGenericName.TypeArgumentList;
                            return memberGenericName.Identifier.ValueText;
                        }

                        return memberAccess.Name.Identifier.ValueText;
                    case MemberBindingExpressionSyntax memberBinding:
                        if (memberBinding.Name is GenericNameSyntax bindingGenericName)
                        {
                            genericArguments = bindingGenericName.TypeArgumentList;
                            return bindingGenericName.Identifier.ValueText;
                        }

                        return memberBinding.Name.Identifier.ValueText;
                    default:
                        return string.Empty;
                }
            }

            private static void AddTypeName(HashSet<string> target, string typeName)
            {
                var normalizedTypeName = NormalizeTypeName(typeName);
                if (string.IsNullOrWhiteSpace(normalizedTypeName))
                {
                    return;
                }

                target.Add(normalizedTypeName);
                var lastDotIndex = normalizedTypeName.LastIndexOf('.');
                if (lastDotIndex >= 0 && lastDotIndex < normalizedTypeName.Length - 1)
                {
                    target.Add(normalizedTypeName.Substring(lastDotIndex + 1));
                }
            }
        }

        private static string NormalizeTypeName(string typeName)
        {
            return typeName.Replace("global::", string.Empty).Trim();
        }

        private static void AddTypeName(HashSet<string> target, string typeName)
        {
            var normalizedTypeName = NormalizeTypeName(typeName);
            if (string.IsNullOrWhiteSpace(normalizedTypeName))
            {
                return;
            }

            target.Add(normalizedTypeName);
            var lastDotIndex = normalizedTypeName.LastIndexOf('.');
            if (lastDotIndex >= 0 && lastDotIndex < normalizedTypeName.Length - 1)
            {
                target.Add(normalizedTypeName.Substring(lastDotIndex + 1));
            }
        }

        private static void AddTypeName(ConcurrentDictionary<string, byte> target, string typeName)
        {
            var normalizedTypeName = NormalizeTypeName(typeName);
            if (string.IsNullOrWhiteSpace(normalizedTypeName))
            {
                return;
            }

            target.TryAdd(normalizedTypeName, 0);
            var lastDotIndex = normalizedTypeName.LastIndexOf('.');
            if (lastDotIndex >= 0 && lastDotIndex < normalizedTypeName.Length - 1)
            {
                target.TryAdd(normalizedTypeName.Substring(lastDotIndex + 1), 0);
            }
        }
    }
}
