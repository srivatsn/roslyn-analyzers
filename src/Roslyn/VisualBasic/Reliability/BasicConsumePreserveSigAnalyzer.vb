' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic

Namespace Roslyn.Diagnostics.Analyzers.VisualBasic
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicConsumePreserveSigAnalyzer
        Inherits ConsumePreserveSigAnalyzer(Of SyntaxKind)

        Protected Overrides ReadOnly Property InvocationExpressionSyntaxKind As SyntaxKind
            Get
                Return SyntaxKind.InvocationExpression
            End Get
        End Property

        Protected Overrides Function IsExpressionStatementSyntaxKind(rawKind As Integer) As Boolean
            Return CType(rawKind, SyntaxKind) = SyntaxKind.ExpressionStatement
        End Function
    End Class
End Namespace
