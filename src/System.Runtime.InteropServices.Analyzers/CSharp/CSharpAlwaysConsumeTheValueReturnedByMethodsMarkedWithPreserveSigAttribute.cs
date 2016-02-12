﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Runtime.InteropServices.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CSharpAlwaysConsumeTheValueReturnedByMethodsMarkedWithPreserveSigAttributeAnalyzer
        : AlwaysConsumeTheValueReturnedByMethodsMarkedWithPreserveSigAttributeAnalyzer<SyntaxKind>
    {
        protected override SyntaxKind InvocationExpressionSyntaxKind
        {
            get { return SyntaxKind.InvocationExpression; }
        }

        protected override bool IsExpressionStatementSyntaxKind(int rawKind)
        {
            return (SyntaxKind)rawKind == SyntaxKind.ExpressionStatement;
        }
    }
}
