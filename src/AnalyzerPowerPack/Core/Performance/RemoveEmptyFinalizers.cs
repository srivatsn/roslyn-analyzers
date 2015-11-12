﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.AnalyzerPowerPack.Utilities;
using Microsoft.CodeAnalysis;

namespace Microsoft.AnalyzerPowerPack.Performance
{
    public abstract class RemoveEmptyFinalizers<TLanguageKindEnum> : DiagnosticAnalyzer where TLanguageKindEnum : struct
    {
        public const string RuleId = "CA1821";
        private static readonly LocalizableString s_localizableMessageAndTitle = new LocalizableResourceString(nameof(AnalyzerPowerPackRulesResources.RemoveEmptyFinalizers), AnalyzerPowerPackRulesResources.ResourceManager, typeof(AnalyzerPowerPackRulesResources));
        private static readonly LocalizableString s_localizableDescription = new LocalizableResourceString(nameof(AnalyzerPowerPackRulesResources.RemoveEmptyFinalizersDescription), AnalyzerPowerPackRulesResources.ResourceManager, typeof(AnalyzerPowerPackRulesResources));

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(RuleId,
                                                                         s_localizableMessageAndTitle,
                                                                         s_localizableMessageAndTitle,
                                                                         AnalyzerPowerPackDiagnosticCategory.Performance,
                                                                         DiagnosticSeverity.Warning,
                                                                         isEnabledByDefault: true,
                                                                         description: s_localizableDescription,
                                                                         helpLinkUri: "http://msdn.microsoft.com/library/bb264476.aspx",
                                                                         customTags: DiagnosticCustomTags.Microsoft);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext analysisContext)
        {
            analysisContext.RegisterCodeBlockStartAction<TLanguageKindEnum>(
                // TODO (tmeschter): Fix this analyzer so the suppression is no longer necessary.
#pragma warning disable RS1013 // Start action has no registered non-end actions.
                (context) =>
#pragma warning restore RS1013 // Start action has no registered non-end actions.
                {
                    var method = context.OwningSymbol as IMethodSymbol;
                    if (method == null)
                    {
                        return;
                    }

                    if (!IsFinalizer(method))
                    {
                        return;
                    }

                    context.RegisterCodeBlockEndAction(codeBlockContext =>
                    {
                        if (IsEmptyFinalizer(codeBlockContext.CodeBlock, context.SemanticModel))
                        {
                            codeBlockContext.ReportDiagnostic(codeBlockContext.OwningSymbol.CreateDiagnostic(Rule));
                        }
                    });
                });
        }


        private static bool IsFinalizer(IMethodSymbol method)
        {
            if (method.MethodKind == MethodKind.Destructor)
            {
                return true; // for C#
            }

            if (method.Name != "Finalize" || method.Parameters.Length != 0 || !method.ReturnsVoid)
            {
                return false;
            }

            var overridden = method.OverriddenMethod;
            if (overridden == null)
            {
                return false;
            }

            for (var o = overridden.OverriddenMethod; o != null; o = o.OverriddenMethod)
            {
                overridden = o;
            }

            return overridden.ContainingType.SpecialType == SpecialType.System_Object; // it is object.Finalize
        }

        protected abstract bool IsEmptyFinalizer(SyntaxNode node, SemanticModel model);
    }
}
