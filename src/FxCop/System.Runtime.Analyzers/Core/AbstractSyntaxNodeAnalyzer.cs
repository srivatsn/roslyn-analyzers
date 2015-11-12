// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Runtime.Analyzers
{
    public abstract class AbstractSyntaxNodeAnalyzer<TLanguageKindEnum> : DiagnosticAnalyzer where TLanguageKindEnum : struct
    {
        public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Descriptor);

        public sealed override void Initialize(AnalysisContext context) => context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKindsOfInterest);

        protected abstract DiagnosticDescriptor Descriptor { get; }
        protected abstract ImmutableArray<TLanguageKindEnum> SyntaxKindsOfInterest { get; }
        protected abstract void AnalyzeNode(SyntaxNodeAnalysisContext context);

        protected void ReportDiagnostic(SyntaxNodeAnalysisContext context, SyntaxNode node, params object[] messageArgs)
        {
            var diagnostic = Diagnostic.Create(Descriptor, node.GetLocation(), messageArgs);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
