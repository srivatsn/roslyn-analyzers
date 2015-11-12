' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports System.Composition
Imports Microsoft.CodeAnalysis.CodeFixes
Imports Microsoft.AnalyzerPowerPack.Usage
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.VisualBasic

Namespace Usage
    <ExportCodeFixProvider(LanguageNames.VisualBasic, Name:="CA2229 CodeFix provider"), [Shared]>
    Public Class BasicMarkAllNonSerializableFieldsFixer
        Inherits MarkAllNonSerializableFieldsFixer

        Protected Overrides Function GetFieldDeclarationNode(node As SyntaxNode) As SyntaxNode
            Dim fieldNode = node
            While fieldNode IsNot Nothing AndAlso fieldNode.Kind() <> SyntaxKind.FieldDeclaration
                fieldNode = fieldNode.Parent
            End While

            Return If(fieldNode?.Kind() = SyntaxKind.FieldDeclaration, fieldNode, Nothing)
        End Function
    End Class
End Namespace
