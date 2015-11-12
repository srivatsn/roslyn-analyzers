﻿' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports System.Collections.Immutable
Imports System.Threading
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Roslyn.Diagnostics.Analyzers.VisualBasic
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicCodeActionCreateAnalyzer
        Inherits CodeActionCreateAnalyzer(Of SyntaxKind)

        Protected Overrides Function GetCodeBlockStartedAnalyzer(symbols As ImmutableHashSet(Of ISymbol)) As AbstractCodeBlockStartedAnalyzer
            Return New CodeBlockStartedAnalyzer(symbols)
        End Function

        Private NotInheritable Class CodeBlockStartedAnalyzer
            Inherits AbstractCodeBlockStartedAnalyzer

            Public Sub New(symbols As ImmutableHashSet(Of ISymbol))
                MyBase.New(symbols)
            End Sub

            Protected Overrides Sub GetSyntaxAnalyzer(context As CodeBlockStartAnalysisContext(Of SyntaxKind), symbols As ImmutableHashSet(Of ISymbol))
                Dim analyzer = New SyntaxAnalyzer(symbols)
                context.RegisterSyntaxNodeAction(AddressOf analyzer.AnalyzeNode, analyzer.SyntaxKindsOfInterest.ToArray())
            End Sub
        End Class

        Private NotInheritable Class SyntaxAnalyzer
            Inherits AbstractSyntaxAnalyzer

            Public Sub New(symbols As ImmutableHashSet(Of ISymbol))
                MyBase.New(symbols)
            End Sub

            Public ReadOnly Property SyntaxKindsOfInterest As ImmutableArray(Of SyntaxKind)
                Get
                    Return ImmutableArray.Create(SyntaxKind.InvocationExpression)
                End Get
            End Property

            Public Sub AnalyzeNode(context As SyntaxNodeAnalysisContext)
                Dim invocation = TryCast(context.Node, InvocationExpressionSyntax)
                If invocation Is Nothing Then
                    Return
                End If

                AnalyzeInvocationExpression(invocation.Expression, context.SemanticModel, AddressOf context.ReportDiagnostic, context.CancellationToken)
            End Sub
        End Class
    End Class
End Namespace
