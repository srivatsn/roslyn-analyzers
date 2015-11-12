' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports System.Collections.Immutable
Imports System.Threading
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Roslyn.Diagnostics.Analyzers.VisualBasic

    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicDirectlyAwaitingTaskAnalyzer
        Inherits DirectlyAwaitingTaskAnalyzer(Of SyntaxKind)

        Protected Overrides ReadOnly Property AwaitSyntaxKind As SyntaxKind
            Get
                Return SyntaxKind.AwaitExpression
            End Get
        End Property

        Protected Overrides Function GetAwaitedExpression(awaitNode As SyntaxNode) As SyntaxNode
            Return DirectCast(awaitNode, AwaitExpressionSyntax).Expression
        End Function
    End Class
End Namespace
