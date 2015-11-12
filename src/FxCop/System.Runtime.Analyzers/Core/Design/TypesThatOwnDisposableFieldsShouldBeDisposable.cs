﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Runtime.Analyzers
{
    /// <summary>
    /// CA1001: Types that own disposable fields should be disposable
    /// </summary>
    public abstract class TypesThatOwnDisposableFieldsShouldBeDisposableAnalyzer<TTypeDeclarationSyntax> : DiagnosticAnalyzer
            where TTypeDeclarationSyntax : SyntaxNode
    {
        internal const string RuleId = "CA1001";
        internal const string Dispose = "Dispose";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(RuleId,
                                                                         new LocalizableResourceString(nameof(SystemRuntimeAnalyzersResources.TypesThatOwnDisposableFieldsShouldBeDisposable), SystemRuntimeAnalyzersResources.ResourceManager, typeof(SystemRuntimeAnalyzersResources)),
                                                                         new LocalizableResourceString(nameof(SystemRuntimeAnalyzersResources.TypeOwnsDisposableFieldButIsNotDisposable), SystemRuntimeAnalyzersResources.ResourceManager, typeof(SystemRuntimeAnalyzersResources)),
                                                                         DiagnosticCategory.Design,
                                                                         DiagnosticSeverity.Warning,
                                                                         isEnabledByDefault: true,
                                                                         helpLinkUri: "http://msdn.microsoft.com/library/ms182172.aspx",
                                                                         customTags: WellKnownDiagnosticTags.Telemetry);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext analysisContext)
        {
            analysisContext.RegisterCompilationStartAction(compilationContext =>
            {
                var disposableType = WellKnownTypes.IDisposable(compilationContext.Compilation);

                if (disposableType == null)
                {
                    return;
                }

                var analyzer = GetAnalyzer(disposableType);
                compilationContext.RegisterSymbolAction(context =>
                {
                    analyzer.AnalyzeSymbol(context);
                },
                SymbolKind.NamedType);
            });
        }

        protected abstract DisposableFieldAnalyzer GetAnalyzer(INamedTypeSymbol disposableType);

        protected abstract class DisposableFieldAnalyzer
        {
            private readonly INamedTypeSymbol _disposableTypeSymbol;

            public DisposableFieldAnalyzer(INamedTypeSymbol disposableTypeSymbol)
            {
                _disposableTypeSymbol = disposableTypeSymbol;
            }

            public void AnalyzeSymbol(SymbolAnalysisContext symbolContext)
            {
                INamedTypeSymbol namedType = (INamedTypeSymbol)symbolContext.Symbol;
                if (!namedType.AllInterfaces.Contains(_disposableTypeSymbol))
                {
                    var disposableFields = from member in namedType.GetMembers()
                                           where member.Kind == SymbolKind.Field && !member.IsStatic
                                           let field = member as IFieldSymbol
                                           where field.Type != null && field.Type.AllInterfaces.Contains(_disposableTypeSymbol)
                                           select field;

                    if (disposableFields.Any())
                    {
                        var disposableFieldsHashSet = new HashSet<ISymbol>(disposableFields);
                        var classDecls = GetClassDeclarationNodes(namedType, symbolContext.CancellationToken);
                        foreach (var classDecl in classDecls)
                        {
                            var model = symbolContext.Compilation.GetSemanticModel(classDecl.SyntaxTree);
                            var syntaxNodes = classDecl.DescendantNodes(n => !(n is TTypeDeclarationSyntax) || ReferenceEquals(n, classDecl))
                                .Where(n => IsDisposableFieldCreation(n,
                                                                    model,
                                                                    disposableFieldsHashSet,
                                                                    symbolContext.CancellationToken));
                            if (syntaxNodes.Any())
                            {
                                symbolContext.ReportDiagnostic(namedType.CreateDiagnostic(Rule, namedType.Name));
                                return;
                            }
                        }
                    }
                }
            }

            private IEnumerable<TTypeDeclarationSyntax> GetClassDeclarationNodes(INamedTypeSymbol namedType, CancellationToken cancellationToken)
            {
                foreach (var syntax in namedType.DeclaringSyntaxReferences.Select(s => s.GetSyntax(cancellationToken)))
                {
                    if (syntax != null)
                    {
                        var classDecl = syntax.FirstAncestorOrSelf<TTypeDeclarationSyntax>(ascendOutOfTrivia: false);
                        if (classDecl != null)
                        {
                            yield return classDecl;
                        }
                    }
                }
            }

            protected abstract bool IsDisposableFieldCreation(SyntaxNode node, SemanticModel model, HashSet<ISymbol> disposableFields, CancellationToken cancellationToken);
        }
    }
}
