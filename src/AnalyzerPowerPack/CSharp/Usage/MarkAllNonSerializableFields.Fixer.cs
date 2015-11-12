// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Composition;
using Microsoft.AnalyzerPowerPack.Usage;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;

namespace Microsoft.AnalyzerPowerPack.CSharp.Usage
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = "CA2237 CodeFix provider"), Shared]
    public class CSharpMarkAllNonSerializableFieldsFixer : MarkAllNonSerializableFieldsFixer
    {
        protected override SyntaxNode GetFieldDeclarationNode(SyntaxNode node)
        {
            var fieldNode = node;
            while (fieldNode != null && fieldNode.Kind() != SyntaxKind.FieldDeclaration)
            {
                fieldNode = fieldNode.Parent;
            }

            return fieldNode?.Kind() == SyntaxKind.FieldDeclaration ? fieldNode : null;
        }
    }
}
