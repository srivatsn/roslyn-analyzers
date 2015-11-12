// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Composition;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Simplification;
using Roslyn.Diagnostics.CodeFixes;

namespace Roslyn.Diagnostics.Analyzers.CSharp.Reliability
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = RoslynDiagnosticIds.DirectlyAwaitingTaskAnalyzerRuleId), Shared]
    public sealed class CSharpDirectlyAwaitingTaskFix : DirectlyAwaitingTaskFix<ExpressionSyntax>
    {
        protected override ExpressionSyntax FixExpression(ExpressionSyntax expression, CancellationToken cancellationToken)
        {
            return
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.ParenthesizedExpression(expression).WithAdditionalAnnotations(Simplifier.Annotation),
                        SyntaxFactory.IdentifierName("ConfigureAwait")),
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Argument(
                                SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression)))));
        }

        protected override string FalseLiteralString
        {
            get
            {
                return "false";
            }
        }
    }
}
