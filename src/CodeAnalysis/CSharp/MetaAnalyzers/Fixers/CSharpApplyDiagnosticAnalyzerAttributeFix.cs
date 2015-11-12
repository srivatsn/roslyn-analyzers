// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Composition;
using Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers.CodeFixes;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Simplification;

namespace Microsoft.CodeAnalysis.CSharp.Analyzers.MetaAnalyzers.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CSharpApplyDiagnosticAnalyzerAttributeFix)), Shared]
    public sealed class CSharpApplyDiagnosticAnalyzerAttributeFix : ApplyDiagnosticAnalyzerAttributeFix
    {
        protected override SyntaxNode ParseExpression(string expression)
        {
            return SyntaxFactory.ParseExpression(expression).WithAdditionalAnnotations(Simplifier.Annotation);
        }
    }
}
