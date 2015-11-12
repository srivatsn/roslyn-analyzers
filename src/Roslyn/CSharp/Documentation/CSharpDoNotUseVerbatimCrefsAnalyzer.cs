// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Roslyn.Diagnostics.Analyzers.Documentation;

namespace Roslyn.Diagnostics.Analyzers.CSharp.Documentation
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CSharpDoNotUseVerbatimCrefsAnalyzer : DoNotUseVerbatimCrefsAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeXmlAttribute, SyntaxKind.XmlTextAttribute);
        }

        private void AnalyzeXmlAttribute(SyntaxNodeAnalysisContext context)
        {
            var textAttribute = (XmlTextAttributeSyntax)context.Node;

            if (textAttribute.Name.LocalName.Text == "cref")
            {
                ProcessAttribute(context, textAttribute.TextTokens);
            }
        }
    }
}
