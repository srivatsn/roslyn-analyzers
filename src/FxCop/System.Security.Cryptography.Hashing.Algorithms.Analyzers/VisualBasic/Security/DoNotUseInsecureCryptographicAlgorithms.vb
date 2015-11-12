' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic

Imports System.Security.Cryptography.Hashing.Algorithms.Analyzers.Common

Namespace System.Security.Cryptography.Hashing.Algorithms.Analyzers

    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicDoNotUseInsecureCryptographicAlgorithmsAnalyzer
        Inherits DoNotUseInsecureCryptographicAlgorithmsAnalyzer

        Protected Overrides Function GetAnalyzer(context As CompilationStartAnalysisContext, cryptTypes As CompilationSecurityTypes) As Analyzer
            Dim analyzer As Analyzer = New Analyzer(cryptTypes)
            context.RegisterSyntaxNodeAction(AddressOf analyzer.AnalyzeNode,
                                             SyntaxKind.InvocationExpression,
                                             SyntaxKind.ObjectCreationExpression)
            Return analyzer
        End Function
    End Class
End Namespace
