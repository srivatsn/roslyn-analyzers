// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.AnalyzerPowerPack
{
    public abstract class AbstractNamedTypeAnalyzer : DiagnosticAnalyzer
    {
        public override abstract ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }

        public override void Initialize(AnalysisContext analysisContext)
        {
            analysisContext.RegisterSymbolAction(
                (context) =>
                    {
                        AnalyzeSymbol((INamedTypeSymbol)context.Symbol, context.Compilation, context.ReportDiagnostic, context.Options, context.CancellationToken);
                    },
                SymbolKind.NamedType);
        }

        protected abstract void AnalyzeSymbol(INamedTypeSymbol symbol, Compilation compilation, Action<Diagnostic> addDiagnostic, AnalyzerOptions options, CancellationToken cancellationToken);
    }
}
