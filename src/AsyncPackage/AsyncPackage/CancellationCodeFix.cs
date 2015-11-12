// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPackage
{
    /// <summary>
    /// Codefix that changes the type of a variable to be Func of Task instead of a void-returning delegate type.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = CancellationAnalyzer.CancellationId), Shared]
    public class CancellationCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(CancellationAnalyzer.CancellationId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return null;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            object root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            object diagnostic = context.Diagnostics.First();
            object diagnosticSpan = diagnostic.Location.SourceSpan;
            object invocation = root.FindToken(diagnosticSpan.Start).Parent.FirstAncestorOrSelf<InvocationExpressionSyntax>();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                new CancellationCodeAction("Propagate CancellationTokens when possible",
                                           c => AddCancellationTokenAsync(context.Document, invocation, c)),
                diagnostic);
        }

        private async Task<Document> AddCancellationTokenAsync(Document document, InvocationExpressionSyntax invocation, CancellationToken cancellationToken)
        {
            object semanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false);

            ITypeSymbol cancellationTokenType = semanticModel.Compilation.GetTypeByMetadataName("System.Threading.CancellationToken");

            var invocationSymbol = semanticModel.GetSymbolInfo(invocation).Symbol as IMethodSymbol;
            object parent = invocation.Parent;
            parent = parent.FirstAncestorOrSelf<MethodDeclarationSyntax>();

            var containingMethod = semanticModel.GetDeclaredSymbol(parent) as IMethodSymbol;
            object tokens = containingMethod.Parameters.Where(x => x.Type.Equals(cancellationTokenType));
            object firstToken = tokens.FirstOrDefault();
            object cancelSlots = invocationSymbol.Parameters.Where(x => x.Type.Equals(cancellationTokenType));

            if (cancelSlots.FirstOrDefault() == null)
            {
                return document;
            }

            object firstSlotIndex = invocationSymbol.Parameters.IndexOf(cancelSlots.FirstOrDefault());
            object newIdentifier = SyntaxFactory.IdentifierName(firstToken.Name.ToString());
            object newArgs = invocation.ArgumentList.Arguments;

            if (firstSlotIndex == 0)
            {
                newArgs = newArgs.Insert(firstSlotIndex, SyntaxFactory.Argument(newIdentifier).WithLeadingTrivia());
            }
            else
            {
                newArgs = invocation.ArgumentList.Arguments.Insert(firstSlotIndex, SyntaxFactory.Argument(newIdentifier).WithLeadingTrivia(SyntaxFactory.TriviaList(SyntaxFactory.ElasticSpace)));
            }

            object newArgsList = SyntaxFactory.ArgumentList(newArgs);
            object newInvocation = invocation.WithArgumentList(newArgsList);
            object oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            object newRoot = oldRoot.ReplaceNode(invocation, newInvocation);
            object newDocument = document.WithSyntaxRoot(newRoot);

            // Return document with transformed tree.
            return newDocument;
        }

        private class CancellationCodeAction : CodeAction
        {
            private readonly Func<CancellationToken, Task<Document>> _createDocument;
            private readonly string _title;

            public CancellationCodeAction(string title, Func<CancellationToken, Task<Document>> createDocument)
            {
                _title = title;
                _createDocument = createDocument;
            }

            public override string Title { get { return _title; } }

            protected override Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
            {
                return _createDocument(cancellationToken);
            }

            public override string EquivalenceKey
            {
                get
                {
                    return nameof(CancellationCodeAction);
                }
            }
        }
    }
}
