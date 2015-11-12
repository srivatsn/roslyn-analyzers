﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslyn.Diagnostics.Analyzers.CSharp
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CSharpDiagnosticDescriptorAccessAnalyzer : DiagnosticDescriptorAccessAnalyzer<SyntaxKind, MemberAccessExpressionSyntax>
    {
        protected override SyntaxKind SimpleMemberAccessExpressionKind
        {
            get
            {
                return SyntaxKind.SimpleMemberAccessExpression;
            }
        }

        protected override SyntaxNode GetLeftOfMemberAccess(MemberAccessExpressionSyntax memberAccess)
        {
            return memberAccess.Expression;
        }

        protected override SyntaxNode GetRightOfMemberAccess(MemberAccessExpressionSyntax memberAccess)
        {
            return memberAccess.Name;
        }

        protected override bool IsThisOrBaseOrMeOrMyBaseExpression(SyntaxNode node)
        {
            switch (node.Kind())
            {
                case SyntaxKind.ThisExpression:
                case SyntaxKind.BaseExpression:
                    return true;

                default:
                    return false;
            }
        }
    }
}
