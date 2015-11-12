﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Analyzers.MetaAnalyzers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.CodeAnalysis.CSharp.Analyzers.MetaAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CSharpDiagnosticDescriptorCreationAnalyzer : DiagnosticDescriptorCreationAnalyzer<ClassDeclarationSyntax, ObjectCreationExpressionSyntax, SyntaxKind>
    {
        protected override ImmutableArray<SyntaxKind> SyntaxKindsOfInterest
        {
            get
            {
                return ImmutableArray.Create(SyntaxKind.ObjectCreationExpression);
            }
        }

        protected override CompilationAnalyzer GetAnalyzer(Compilation compilation, INamedTypeSymbol diagnosticDescriptorType)
        {
            return new CSharpCompilationAnalyzer(diagnosticDescriptorType);
        }

        private sealed class CSharpCompilationAnalyzer : CompilationAnalyzer
        {
            public CSharpCompilationAnalyzer(INamedTypeSymbol diagnosticDescriptorType)
                : base(diagnosticDescriptorType)
            {
            }

            protected override SyntaxNode GetObjectCreationType(ObjectCreationExpressionSyntax objectCreation)
            {
                return objectCreation.Type;
            }
        }
    }
}
