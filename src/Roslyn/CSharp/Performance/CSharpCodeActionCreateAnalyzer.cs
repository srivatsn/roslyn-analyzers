﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslyn.Diagnostics.Analyzers.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CSharpCodeActionCreateAnalyzer : CodeActionCreateAnalyzer<SyntaxKind>
    {
        protected override AbstractCodeBlockStartedAnalyzer GetCodeBlockStartedAnalyzer(ImmutableHashSet<ISymbol> symbols)
        {
            return new CodeBlockStartedAnalyzer(symbols);
        }

        private sealed class CodeBlockStartedAnalyzer : AbstractCodeBlockStartedAnalyzer
        {
            public CodeBlockStartedAnalyzer(ImmutableHashSet<ISymbol> symbols) : base(symbols)
            {
            }

            protected override void GetSyntaxAnalyzer(CodeBlockStartAnalysisContext<SyntaxKind> context, ImmutableHashSet<ISymbol> symbols)
            {
                var analyzer = new SyntaxAnalyzer(symbols);
                context.RegisterSyntaxNodeAction(analyzer.AnalyzeNode, analyzer.SyntaxKindsOfInterest.ToArray());
            }
        }

        private sealed class SyntaxAnalyzer : AbstractSyntaxAnalyzer
        {
            public SyntaxAnalyzer(ImmutableHashSet<ISymbol> symbols) : base(symbols)
            {
            }

            public ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
            {
                get { return ImmutableArray.Create(SyntaxKind.InvocationExpression); }
            }

            public void AnalyzeNode(SyntaxNodeAnalysisContext context)
            {
                var invocation = context.Node as InvocationExpressionSyntax;
                if (invocation == null)
                {
                    return;
                }

                AnalyzeInvocationExpression(invocation.Expression, context.SemanticModel, context.ReportDiagnostic, context.CancellationToken);
            }
        }
    }
}
