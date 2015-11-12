' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports System.Composition
Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.AnalyzerPowerPack.Design
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.VisualBasic

Namespace Design
    ' <summary>
    ' CA1008: Enums should have zero value
    ' </summary>
    <ExportCodeFixProvider(LanguageNames.VisualBasic, Name:="CA1008"), [Shared]>
    Public Class CA1008BasicCodeFixProvider
        Inherits CA1008CodeFixProviderBase

        Protected Overrides Function GetParentNodeOrSelfToFix(nodeToFix As SyntaxNode) As SyntaxNode
            If nodeToFix.IsKind(SyntaxKind.EnumStatement) And nodeToFix.Parent IsNot Nothing Then
                Return nodeToFix.Parent
            End If

            Return nodeToFix
        End Function
    End Class
End Namespace
