// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AsyncPackage
{
    /// <summary>
    /// This Analyzer determines if a method is Async and needs to be returning a Task instead of having a void return type.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class AsyncVoidAnalyzer : DiagnosticAnalyzer
    {
        internal const string AsyncVoidId = "Async001";

        internal static readonly DiagnosticDescriptor VoidReturnType = new DiagnosticDescriptor(id: AsyncVoidId,
            title: "Avoid Async Void",
            messageFormat: "This method has the async keyword but it returns void",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(VoidReturnType); } }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // Filter out methods that do not use Async and that do not have exactly two parameters
            var methodSymbol = (IMethodSymbol)context.Symbol;
            object eventType = context.Compilation.GetTypeByMetadataName("System.EventArgs");

            if (methodSymbol.ReturnsVoid && methodSymbol.IsAsync)
            {
                if (methodSymbol.Parameters.Length == 2)
                {
                    object firstParam = methodSymbol.Parameters[0];
                    object secondParam = methodSymbol.Parameters[1];

                    if (firstParam is object)
                    {
                        // Check each parameter for EventHandler shape and return if it matches.
                        if (firstParam.Name.ToLower().Equals("sender") && secondParam.Type == eventType)
                        {
                            return;
                        }
                        else
                        {
                            object checkForEventType = secondParam.Type.BaseType;
                            while (checkForEventType.OriginalDefinition != context.Compilation.GetTypeByMetadataName("System.Object"))
                            {
                                if (checkForEventType == eventType)
                                {
                                    return;
                                }

                                checkForEventType = checkForEventType.BaseType;
                            }
                        }
                    }
                }

                context.ReportDiagnostic(Diagnostic.Create(VoidReturnType, methodSymbol.Locations[0], methodSymbol.Name));
                return;
            }

            return;
        }
    }
}
