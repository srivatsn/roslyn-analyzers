// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslyn.Diagnostics.Analyzers.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CSharpDoNotCallContinueWithWithoutTaskSchedulerAnalyzer : DoNotCreateTasksWithoutTaskSchedulerAnalyzer<SyntaxKind>
    {
        protected override SyntaxKind InvocationExpressionSyntaxKind
        {
            get { return SyntaxKind.InvocationExpression; }
        }
    }
}
