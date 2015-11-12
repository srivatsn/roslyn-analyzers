﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AsyncPackage
{
    /// <summary>
    /// This analyzer will run a codefix on any method that qualifies as async that renames it to follow naming conventions
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    public class RenameAsyncAnalyzer : DiagnosticAnalyzer
    {
        internal const string RenameAsyncId = "Async002";

        internal static readonly DiagnosticDescriptor RenameAsyncMethod = new DiagnosticDescriptor(id: RenameAsyncId,
            title: "Async Method Names Should End in Async",
            messageFormat: "This method is async but the method name does not end in Async",
            category: "Naming",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.Method);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(RenameAsyncMethod); } }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            // Filter out methods that do not use Async and make sure to include methods that return a Task
            var methodSymbol = (IMethodSymbol)context.Symbol;

            // Check if method name is an override or virtual class. If it is disregard it.
            // (This assumes if a method is virtual the programmer will not want to change the name)
            // Check if the method returns a Task or Task<TResult>
            if ((methodSymbol.ReturnType == context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task")
                || methodSymbol.ReturnType.OriginalDefinition == context.Compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1").OriginalDefinition)
                && !methodSymbol.Name.EndsWith("Async") && !methodSymbol.IsOverride && !methodSymbol.IsVirtual)
            {
                context.ReportDiagnostic(Diagnostic.Create(RenameAsyncMethod, methodSymbol.Locations[0], methodSymbol.Name));
                return;
            }

            return;
        }
    }
}
