﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Roslyn.Diagnostics.Analyzers
{
    public abstract class AbstractSyntaxNodeAnalyzer<TLanguageKindEnum> : DiagnosticAnalyzer where TLanguageKindEnum : struct
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return ImmutableArray.Create(Descriptor);
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKindsOfInterest);
        }

        protected abstract DiagnosticDescriptor Descriptor { get; }
        protected abstract TLanguageKindEnum[] SyntaxKindsOfInterest { get; }
        protected abstract void AnalyzeNode(SyntaxNodeAnalysisContext context);

        protected void ReportDiagnostic(SyntaxNodeAnalysisContext context, SyntaxNode node, params object[] messageArgs)
        {
            var diagnostic = Diagnostic.Create(Descriptor, node.GetLocation(), messageArgs);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
