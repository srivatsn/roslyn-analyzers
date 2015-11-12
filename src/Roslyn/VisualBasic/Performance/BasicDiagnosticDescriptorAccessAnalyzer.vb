' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Roslyn.Diagnostics.Analyzers.VisualBasic
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicDiagnosticDescriptorAccessAnalyzer
        Inherits DiagnosticDescriptorAccessAnalyzer(Of SyntaxKind, MemberAccessExpressionSyntax)

        Protected Overrides ReadOnly Property SimpleMemberAccessExpressionKind As SyntaxKind
            Get
                Return SyntaxKind.SimpleMemberAccessExpression
            End Get
        End Property

        Protected Overrides Function GetLeftOfMemberAccess(memberAccess As MemberAccessExpressionSyntax) As SyntaxNode
            Return memberAccess.Expression
        End Function

        Protected Overrides Function GetRightOfMemberAccess(memberAccess As MemberAccessExpressionSyntax) As SyntaxNode
            Return memberAccess.Name
        End Function

        Protected Overrides Function IsThisOrBaseOrMeOrMyBaseExpression(node As SyntaxNode) As Boolean
            Select Case node.Kind
                Case SyntaxKind.MeExpression, SyntaxKind.MyBaseExpression
                    Return True
                Case Else
                    Return False
            End Select
        End Function
    End Class
End Namespace
