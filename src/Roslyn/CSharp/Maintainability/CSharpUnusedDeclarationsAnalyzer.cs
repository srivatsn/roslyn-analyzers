﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslyn.Diagnostics.Analyzers.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class CSharpUnusedDeclarationsAnalyzer : UnusedDeclarationsAnalyzer<SyntaxKind>
    {
        protected override SyntaxKind IdentifierSyntaxKind
        {
            get { return SyntaxKind.IdentifierName; }
        }

        protected override SyntaxKind LocalDeclarationStatementSyntaxKind
        {
            get { return SyntaxKind.LocalDeclarationStatement; }
        }

        protected override IEnumerable<SyntaxNode> GetLocalDeclarationNodes(SyntaxNode node, CancellationToken cancellationToken)
        {
            var locals = node as LocalDeclarationStatementSyntax;
            if (locals == null)
            {
                yield break;
            }

            var variables = (locals.Declaration == null) ? (SeparatedSyntaxList<VariableDeclaratorSyntax>?)null : locals.Declaration.Variables;
            if (variables == null)
            {
                yield break;
            }

            foreach (var variable in variables)
            {
                yield return variable;
            }
        }
    }
}
