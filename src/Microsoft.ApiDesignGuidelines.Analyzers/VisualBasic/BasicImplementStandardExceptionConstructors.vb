' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Diagnostics
Imports Microsoft.CodeAnalysis.VisualBasic
Imports Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace Microsoft.ApiDesignGuidelines.Analyzers
    ''' <summary>
    ''' CA1032: Implement standard exception constructors
    ''' </summary>
    <DiagnosticAnalyzer(LanguageNames.VisualBasic)>
    Public NotInheritable Class BasicImplementStandardExceptionConstructorsAnalyzer
        Inherits ImplementStandardExceptionConstructorsAnalyzer

        Protected Overrides Function GetConstructorSignatureStringAndExceptionTypeParameter(symbol As ISymbol) As String
            Return "Public Sub New(message As String, innerException As Exception)"
        End Function

        Protected Overrides Function GetConstructorSignatureStringTypeParameter(symbol As ISymbol) As String
            Return "Public Sub New(message As String)"
        End Function

        Protected Overrides Function GetConstructorSignatureNoParameter(symbol As ISymbol) As String
            Return "Public Sub New()"
        End Function
    End Class
End Namespace