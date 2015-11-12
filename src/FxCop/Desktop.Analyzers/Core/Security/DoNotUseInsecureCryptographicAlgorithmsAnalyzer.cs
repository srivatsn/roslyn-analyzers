﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using Desktop.Analyzers.Common;


namespace Desktop.Analyzers
{
    public abstract class DoNotUseInsecureCryptographicAlgorithmsAnalyzer : DiagnosticAnalyzer
    {
        internal const string DoNotUseWeakCryptographicRuleId = "CA5350";
        internal const string DoNotUseBrokenCryptographicRuleId = "CA5351";

        private static readonly LocalizableString s_localizableDoNotUseMD5Title = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseMD5));
        private static readonly LocalizableString s_localizableDoNotUseMD5Description = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseMD5Description));
        private static readonly LocalizableString s_localizableDoNotUseDESTitle = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseDES));
        private static readonly LocalizableString s_localizableDoNotUseDESDescription = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseDESDescription));
        private static readonly LocalizableString s_localizableDoNotUseRC2Title = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseRC2));
        private static readonly LocalizableString s_localizableDoNotUseRC2Description = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseRC2Description));
        private static readonly LocalizableString s_localizableDoNotUseTripleDESTitle = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseTripleDES));
        private static readonly LocalizableString s_localizableDoNotUseTripleDESDescription = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseTripleDESDescription));
        private static readonly LocalizableString s_localizableDoNotUseRIPEMD160Title = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseRIPEMD160));
        private static readonly LocalizableString s_localizableDoNotUseRIPEMD160Description = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseRIPEMD160Description));
        private static readonly LocalizableString s_localizableDoNotUseDSATitle = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseDSA));
        private static readonly LocalizableString s_localizableDoNotUseDSADescription = DiagnosticHelpers.GetLocalizableResourceString(nameof(DesktopAnalyzersResources.DoNotUseDSADescription));

        internal static DiagnosticDescriptor DoNotUseMD5SpecificRule = CreateDiagnosticDescriptor(DoNotUseBrokenCryptographicRuleId,
                                                                                          s_localizableDoNotUseMD5Title,
                                                                                          s_localizableDoNotUseMD5Description);

        internal static DiagnosticDescriptor DoNotUseDESSpecificRule = CreateDiagnosticDescriptor(DoNotUseBrokenCryptographicRuleId,
                                                                                          s_localizableDoNotUseDESTitle,
                                                                                          s_localizableDoNotUseDESDescription);

        internal static DiagnosticDescriptor DoNotUseRC2SpecificRule = CreateDiagnosticDescriptor(DoNotUseBrokenCryptographicRuleId,
                                                                                          s_localizableDoNotUseRC2Title,
                                                                                          s_localizableDoNotUseRC2Description);

        internal static DiagnosticDescriptor DoNotUseTripleDESSpecificRule = CreateDiagnosticDescriptor(DoNotUseWeakCryptographicRuleId,
                                                                                                s_localizableDoNotUseTripleDESTitle,
                                                                                                s_localizableDoNotUseTripleDESDescription);

        internal static DiagnosticDescriptor DoNotUseRIPEMD160SpecificRule = CreateDiagnosticDescriptor(DoNotUseWeakCryptographicRuleId,
                                                                                                s_localizableDoNotUseRIPEMD160Title,
                                                                                                s_localizableDoNotUseRIPEMD160Description);

        internal static DiagnosticDescriptor DoNotUseDSASpecificRule = CreateDiagnosticDescriptor(DoNotUseBrokenCryptographicRuleId,
                                                                                          s_localizableDoNotUseDSATitle,
                                                                                          s_localizableDoNotUseDSADescription);

        protected abstract Analyzer GetAnalyzer(CompilationStartAnalysisContext context, CompilationSecurityTypes cryptTypes);

        private static readonly ImmutableArray<DiagnosticDescriptor> s_supportedDiagnostics = ImmutableArray.Create(DoNotUseMD5SpecificRule,
                                                                                                                  DoNotUseDESSpecificRule,
                                                                                                                  DoNotUseRC2SpecificRule,
                                                                                                                  DoNotUseTripleDESSpecificRule,
                                                                                                                  DoNotUseRIPEMD160SpecificRule,
                                                                                                                  DoNotUseDSASpecificRule);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => s_supportedDiagnostics;

        private static DiagnosticDescriptor CreateDiagnosticDescriptor(string ruleId, LocalizableString title, LocalizableString description, string uri = null)
        {
            return new DiagnosticDescriptor(ruleId,
                                            title,
                                            title,
                                            DiagnosticCategory.Security,
                                            DiagnosticSeverity.Warning,
                                            isEnabledByDefault: true,
                                            description: description,
                                            helpLinkUri: uri,
                                            customTags: WellKnownDiagnosticTags.Telemetry);
        }

        public override void Initialize(AnalysisContext analysisContext)
        {
            analysisContext.RegisterCompilationStartAction(
                (context) =>
                {
                    var cryptTypes = new CompilationSecurityTypes(context.Compilation);
                    if (ReferencesAnyTargetType(cryptTypes))
                    {
                        GetAnalyzer(context, cryptTypes);
                    }
                });
        }

        private static bool ReferencesAnyTargetType(CompilationSecurityTypes types)
        {
            return types.DES != null
                || types.DSA != null
                || types.DSASignatureFormatter != null
                || types.HMACMD5 != null
                || types.RC2 != null
                || types.TripleDES != null
                || types.RIPEMD160 != null
                || types.HMACRIPEMD160 != null;
        }

        protected class Analyzer
        {
            private readonly CompilationSecurityTypes _cryptTypes;

            public Analyzer(CompilationSecurityTypes cryptTypes)
            {
                _cryptTypes = cryptTypes;
            }

            public void AnalyzeNode(SyntaxNodeAnalysisContext context)
            {
                SyntaxNode node = context.Node;
                SemanticModel model = context.SemanticModel;

                ISymbol symbol = SyntaxNodeHelper.GetSymbol(node, model);
                IMethodSymbol method = symbol as IMethodSymbol;
                if (method == null)
                {
                    return;
                }

                INamedTypeSymbol type = method.ContainingType;
                DiagnosticDescriptor rule = null;

                if (type.IsDerivedFrom(_cryptTypes.DES, baseTypesOnly: true))
                {
                    rule = DoNotUseDESSpecificRule;
                }
                else if (method.MatchMethodDerived(_cryptTypes.DSA, SecurityMemberNames.CreateSignature) ||
                         (type == _cryptTypes.DSASignatureFormatter &&
                          method.MatchMethodDerived(_cryptTypes.DSASignatureFormatter, WellKnownMemberNames.InstanceConstructorName)))
                {
                    rule = DoNotUseDSASpecificRule;
                }
                else if (type.IsDerivedFrom(_cryptTypes.HMACMD5, baseTypesOnly: true))
                {
                    rule = DoNotUseMD5SpecificRule;
                }
                else if (type.IsDerivedFrom(_cryptTypes.RC2, baseTypesOnly: true))
                {
                    rule = DoNotUseRC2SpecificRule;
                }
                else if (type.IsDerivedFrom(_cryptTypes.TripleDES, baseTypesOnly: true))
                {
                    rule = DoNotUseTripleDESSpecificRule;
                }
                else if (type.IsDerivedFrom(_cryptTypes.RIPEMD160, baseTypesOnly: true) ||
                         type.IsDerivedFrom(_cryptTypes.HMACRIPEMD160, baseTypesOnly: true))
                {
                    rule = DoNotUseRIPEMD160SpecificRule;
                }

                if (rule != null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(rule, node.GetLocation()));
                }
            }
        }
    }
}

