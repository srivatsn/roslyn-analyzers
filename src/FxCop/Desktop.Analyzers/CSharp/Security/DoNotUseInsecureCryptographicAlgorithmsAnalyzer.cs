// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

using Desktop.Analyzers.Common;

namespace Desktop.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CSharpDoNotUseInsecureCryptographicAlgorithmsAnalyzer : DoNotUseInsecureCryptographicAlgorithmsAnalyzer
    {
        protected override Analyzer GetAnalyzer(CompilationStartAnalysisContext context, CompilationSecurityTypes cryptTypes)
        {
            Analyzer analyzer = new Analyzer(cryptTypes);
            context.RegisterSyntaxNodeAction(analyzer.AnalyzeNode,
                                             SyntaxKind.InvocationExpression,
                                             SyntaxKind.ObjectCreationExpression);
            return analyzer;
        }
    }
}
