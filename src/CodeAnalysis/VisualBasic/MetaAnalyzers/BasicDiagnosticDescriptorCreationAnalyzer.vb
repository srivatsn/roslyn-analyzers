' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports System.Collections.Immutable
Imports Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.Analyzers.MetaAnalyzers
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicDiagnosticDescriptorCreationAnalyzer
        Inherits DiagnosticDescriptorCreationAnalyzer(Of ClassBlockSyntax, ObjectCreationExpressionSyntax, SyntaxKind)

        Protected Overrides ReadOnly Property SyntaxKindsOfInterest As ImmutableArray(Of SyntaxKind)
            Get
                Return ImmutableArray.Create(SyntaxKind.ObjectCreationExpression)
            End Get
        End Property

        Protected Overrides Function GetAnalyzer(compilation As Compilation, diagnosticDescriptorType As INamedTypeSymbol) As CompilationAnalyzer
            Return New BasicCompilationAnalyzer(diagnosticDescriptorType)
        End Function

        Private NotInheritable Class BasicCompilationAnalyzer
            Inherits CompilationAnalyzer

            Public Sub New(diagnosticDescriptorType As INamedTypeSymbol)
                MyBase.New(diagnosticDescriptorType)
            End Sub

            Protected Overrides Function GetObjectCreationType(objectCreation As ObjectCreationExpressionSyntax) As SyntaxNode
                Return objectCreation.Type
            End Function
        End Class
    End Class
End Namespace

