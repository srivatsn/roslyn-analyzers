' Copyright (c) Microsoft. All rights reserved.
' Licensed under the MIT license. See LICENSE file in the project root for full license information.

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic

Namespace System.Runtime.Analyzers
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public Class BasicDefineAccessorsForAttributeArgumentsAnalyzer
        Inherits DefineAccessorsForAttributeArgumentsAnalyzer

        Protected Overrides Function IsAssignableTo(fromSymbol As ITypeSymbol, toSymbol As ITypeSymbol, compilation As Compilation) As Boolean
            Return fromSymbol IsNot Nothing AndAlso toSymbol IsNot Nothing AndAlso DirectCast(compilation, VisualBasicCompilation).ClassifyConversion(fromSymbol, toSymbol).IsWidening
        End Function
    End Class
End Namespace
