// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPackage
{
    /// <summary>
    /// Codefix that changes the type of a variable to be Func of Task instead of a void-returning delegate type.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = AsyncLambdaAnalyzer.AsyncLambdaId1), Shared]
    public class AsyncLambdaVariableCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AsyncLambdaAnalyzer.AsyncLambdaId1); }
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

            Debug.Assert(root != null);
            object parent = root.FindToken(diagnosticSpan.Start).Parent;
            if (parent != null)
            {
                object variableDeclaration = parent.FirstAncestorOrSelf<VariableDeclarationSyntax>();

                // Register a code action that will invoke the fix.
                context.RegisterCodeFix(
                    new AsyncLambdaVariableCodeAction("Async lambdas should not be stored in void-returning delegates",
                                                      c => ChangeToFunc(context.Document, variableDeclaration, c)),
                    diagnostic);
            }
        }

        private async Task<Document> ChangeToFunc(Document document, VariableDeclarationSyntax variableDeclaration, CancellationToken cancellationToken)
        {
            object newDeclaration = variableDeclaration.WithType(SyntaxFactory.ParseTypeName("System.Func<System.Threading.Tasks.Task>").WithAdditionalAnnotations(Simplifier.Annotation, Formatter.Annotation)
                .WithLeadingTrivia(variableDeclaration.Type.GetLeadingTrivia()).WithTrailingTrivia(variableDeclaration.Type.GetTrailingTrivia()));
            object oldRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            object newRoot = oldRoot.ReplaceNode(variableDeclaration, newDeclaration);
            object newDocument = document.WithSyntaxRoot(newRoot);

            // Return document with transformed tree.
            return newDocument;
        }

        private class AsyncLambdaVariableCodeAction : CodeAction
        {
            private readonly Func<CancellationToken, Task<Document>> _generateDocument;
            private readonly string _title;

            public AsyncLambdaVariableCodeAction(string title, Func<CancellationToken, Task<Document>> generateDocument)
            {
                _title = title;
                _generateDocument = generateDocument;
            }

            public override string Title { get { return _title; } }

            protected override Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
            {
                return _generateDocument(cancellationToken);
            }

            public override string EquivalenceKey
            {
                get
                {
                    return nameof(AsyncLambdaVariableCodeAction);
                }
            }
        }
    }
}
