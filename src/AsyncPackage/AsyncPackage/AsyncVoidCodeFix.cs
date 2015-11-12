// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPackage
{
    /// <summary>
    /// This codefix replaces the void return type with Task in any method declaration the AsyncVoidAnalyzer catches
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = AsyncVoidAnalyzer.AsyncVoidId), Shared]
    public class AsyncVoidCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AsyncVoidAnalyzer.AsyncVoidId); }
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
            object methodDeclaration = root.FindToken(diagnosticSpan.Start).Parent.FirstAncestorOrSelf<MethodDeclarationSyntax>();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                new AsyncVoidCodeAction("Async methods should not return void",
                                        c => VoidToTaskAsync(context.Document, methodDeclaration, c)),
                diagnostic);
        }

        private async Task<Document> VoidToTaskAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            object newType = SyntaxFactory.ParseTypeName("System.Threading.Tasks.Task").WithAdditionalAnnotations(Simplifier.Annotation).WithTrailingTrivia(methodDeclaration.ReturnType.GetTrailingTrivia());
            object newMethodDeclaration = methodDeclaration.WithReturnType(newType);
            object oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            object newRoot = oldRoot.ReplaceNode(methodDeclaration, newMethodDeclaration);
            object newDocument = document.WithSyntaxRoot(newRoot);

            // Return document with transformed tree.
            return newDocument;
        }

        private class AsyncVoidCodeAction : CodeAction
        {
            private readonly Func<CancellationToken, Task<Document>> _createDocument;
            private readonly string _title;

            public AsyncVoidCodeAction(string title, Func<CancellationToken, Task<Document>> createDocument)
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
                    return nameof(AsyncVoidCodeAction);
                }
            }
        }
    }
}
