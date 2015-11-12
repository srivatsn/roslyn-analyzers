' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports System.Composition
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers.CodeFixes
Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.CodeAnalysis.Simplification

Namespace Microsoft.CodeAnalysis.VisualBasic.Analyzers.MetaAnalyzers.CodeFixes
    <ExportCodeFixProviderAttribute(LanguageNames.VisualBasic, Name:=NameOf(BasicApplyDiagnosticAnalyzerAttributeFix)), [Shared]>
    Public Class BasicApplyDiagnosticAnalyzerAttributeFix
        Inherits ApplyDiagnosticAnalyzerAttributeFix

        Protected Overrides Function ParseExpression(expression As String) As SyntaxNode
            Return SyntaxFactory.ParseExpression(expression).WithAdditionalAnnotations(Simplifier.Annotation)
        End Function
    End Class
End Namespace
