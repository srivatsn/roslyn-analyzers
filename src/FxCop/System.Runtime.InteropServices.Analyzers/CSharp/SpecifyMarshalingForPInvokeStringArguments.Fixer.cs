// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.InteropServices.Analyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = PInvokeDiagnosticAnalyzer.CA2101), Shared]
    public class CSharpSpecifyMarshalingForPInvokeStringArgumentsFixer : SpecifyMarshalingForPInvokeStringArgumentsFixer
    {
        protected override bool IsAttribute(SyntaxNode node)
        {
            return node.IsKind(SyntaxKind.Attribute);
        }

        protected override SyntaxNode FindNamedArgument(IReadOnlyList<SyntaxNode> arguments, string argumentName)
        {
            return arguments.OfType<AttributeArgumentSyntax>().FirstOrDefault(arg => arg.NameEquals != null && arg.NameEquals.Name.Identifier.Text == argumentName);
        }

        protected override bool IsDeclareStatement(SyntaxNode node)
        {
            return false;
        }

        protected override Task<Document> FixDeclareStatement(Document document, SyntaxNode node, CancellationToken cancellationToken)
        {
            return Task.FromResult(document);
        }
    }
}
