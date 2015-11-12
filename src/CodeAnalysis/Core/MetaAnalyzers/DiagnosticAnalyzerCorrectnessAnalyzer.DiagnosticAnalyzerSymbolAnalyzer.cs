// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers
{
    public abstract partial class DiagnosticAnalyzerCorrectnessAnalyzer : DiagnosticAnalyzer
    {
        protected abstract class DiagnosticAnalyzerSymbolAnalyzer
        {
            private readonly INamedTypeSymbol _diagnosticAnalyzer;
            private readonly INamedTypeSymbol _diagnosticAnalyzerAttribute;

            protected DiagnosticAnalyzerSymbolAnalyzer(INamedTypeSymbol diagnosticAnalyzer, INamedTypeSymbol diagnosticAnalyzerAttribute)
            {
                _diagnosticAnalyzer = diagnosticAnalyzer;
                _diagnosticAnalyzerAttribute = diagnosticAnalyzerAttribute;
            }

            protected INamedTypeSymbol DiagnosticAnalyzer { get { return _diagnosticAnalyzer; } }
            protected INamedTypeSymbol DiagnosticAnalyzerAttribute { get { return _diagnosticAnalyzerAttribute; } }

            protected bool IsDiagnosticAnalyzer(INamedTypeSymbol type)
            {
                return type.Equals(_diagnosticAnalyzer);
            }

            internal void AnalyzeSymbol(SymbolAnalysisContext symbolContext)
            {
                var namedType = (INamedTypeSymbol)symbolContext.Symbol;
                if (namedType.GetBaseTypes().Any(IsDiagnosticAnalyzer))
                {
                    AnalyzeDiagnosticAnalyzer(symbolContext);
                }
            }

            protected abstract void AnalyzeDiagnosticAnalyzer(SymbolAnalysisContext symbolContext);

            protected bool HasDiagnosticAnalyzerAttribute(INamedTypeSymbol namedType)
            {
                foreach (var attribute in AttributeHelpers.GetApplicableAttributes(namedType))
                {
                    if (AttributeHelpers.DerivesFrom(attribute.AttributeClass, DiagnosticAnalyzerAttribute))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
