' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.CodeAnalysis.VisualBasic.Analyzers.MetaAnalyzers
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicDiagnosticAnalyzerFieldsAnalyzer
        Inherits DiagnosticAnalyzerFieldsAnalyzer(Of ClassBlockSyntax, FieldDeclarationSyntax, TypeSyntax, SimpleAsClauseSyntax)
    End Class
End Namespace

