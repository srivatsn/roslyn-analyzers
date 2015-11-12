// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPackage
{
    /// <summary>
    /// Codefix changes the synchronous operations to it's asynchronous equivalent. 
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = BlockingAsyncAnalyzer.BlockingAsyncId), Shared]
    public class BlockingAsyncCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(BlockingAsyncAnalyzer.BlockingAsyncId); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            object root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            object diagnostic = context.Diagnostics.First();
            object diagnosticSpan = diagnostic.Location.SourceSpan;
            object memberAccessNode = root.FindToken(diagnosticSpan.Start).Parent.FirstAncestorOrSelf<MemberAccessExpressionSyntax>();
            object semanticmodel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

            var method = semanticmodel.GetEnclosingSymbol(memberAccessNode.SpanStart) as IMethodSymbol;

            if (method != null && method.IsAsync)
            {
                var invokeMethod = semanticmodel.GetSymbolInfo(memberAccessNode).Symbol as IMethodSymbol;

                if (invokeMethod != null)
                {
                    object invocation = memberAccessNode.FirstAncestorOrSelf<InvocationExpressionSyntax>();

                    if (memberAccessNode.Name.Identifier.Text.Equals("Wait"))
                    {
                        object name = memberAccessNode.Name.Identifier.Text;

                        // Register a code action that will invoke the fix.
                        context.RegisterCodeFix(
                            new CodeActionChangetoAwaitAsync("Change synchronous operation to asynchronous counterpart",
                                                             c => ChangetoAwaitAsync(context.Document, invocation, name, c)),
                            diagnostic);
                        return;
                    }

                    if (memberAccessNode.Name.Identifier.Text.Equals("GetAwaiter"))
                    {
                        // Register a code action that will invoke the fix.
                        context.RegisterCodeFix(
                            new CodeActionChangetoAwaitGetAwaiterAsync("Change synchronous operation to asynchronous counterpart",
                                                                       c => ChangetoAwaitGetAwaiterAsync(context.Document, invocation, c)),
                            diagnostic);
                        return;
                    }

                    if (memberAccessNode.Name.Identifier.Text.Equals("Result"))
                    {
                    }

                    if (memberAccessNode.Name.Identifier.Text.Equals("WaitAny"))
                    {
                        object name = memberAccessNode.Name.Identifier.Text;

                        // Register a code action that will invoke the fix.
                        context.RegisterCodeFix(
                            new CodeActionToDelayWhenAnyWhenAllAsync("Change synchronous operation to asynchronous counterpart",
                                                                     c => ToDelayWhenAnyWhenAllAsync(context.Document, invocation, name, c)),
                            diagnostic);
                        return;
                    }

                    if (memberAccessNode.Name.Identifier.Text.Equals("WaitAll"))
                    {
                        object name = memberAccessNode.Name.Identifier.Text;

                        // Register a code action that will invoke the fix.
                        context.RegisterCodeFix(
                            new CodeActionToDelayWhenAnyWhenAllAsync("Change synchronous operation to asynchronous counterpart",
                                                                     c => ToDelayWhenAnyWhenAllAsync(context.Document, invocation, name, c)),
                            diagnostic);
                        return;
                    }

                    if (memberAccessNode.Name.Identifier.Text.Equals("Sleep"))
                    {
                        object name = memberAccessNode.Name.Identifier.Text;

                        // Register a code action that will invoke the fix.
                        context.RegisterCodeFix(
                            new CodeActionToDelayWhenAnyWhenAllAsync("Change synchronous operation to asynchronous counterpart",
                                                                     c => ToDelayWhenAnyWhenAllAsync(context.Document, invocation, name, c)),
                            diagnostic);
                        return;
                    }
                }

                var property = semanticmodel.GetSymbolInfo(memberAccessNode).Symbol as IPropertySymbol;

                if (property != null && memberAccessNode.Name.Identifier.Text.Equals("Result"))
                {
                    object name = memberAccessNode.Name.Identifier.Text;

                    // Register a code action that will invoke the fix.
                    context.RegisterCodeFix(
                        new CodeActionChangetoAwaitAsync("Change synchronous operation to asynchronous counterpart",
                                                         c => ChangeToAwaitAsync(context.Document, memberAccessNode, name, c)),
                        diagnostic);
                    return;
                }
            }
        }

        private async Task<Document> ToDelayWhenAnyWhenAllAsync(Document document, InvocationExpressionSyntax invocation, string name, CancellationToken cancellationToken)
        {
            object simpleExpression = SyntaxFactory.ParseName("");
            if (name.Equals("WaitAny"))
            {
                simpleExpression = SyntaxFactory.ParseName("System.Threading.Tasks.Task.WhenAny").WithAdditionalAnnotations(Simplifier.Annotation);
            }
            else if (name.Equals("WaitAll"))
            {
                simpleExpression = SyntaxFactory.ParseName("System.Threading.Tasks.Task.WhenAll").WithAdditionalAnnotations(Simplifier.Annotation);
            }
            else if (name.Equals("Sleep"))
            {
                simpleExpression = SyntaxFactory.ParseName("System.Threading.Tasks.Task.Delay").WithAdditionalAnnotations(Simplifier.Annotation);
            }

            SyntaxNode oldExpression = invocation;
            object expression = invocation.WithExpression(simpleExpression).WithLeadingTrivia(invocation.GetLeadingTrivia()).WithTrailingTrivia(invocation.GetTrailingTrivia());
            object newExpression = SyntaxFactory.AwaitExpression(expression.WithLeadingTrivia(SyntaxFactory.Space)).WithTrailingTrivia(invocation.GetTrailingTrivia()).WithLeadingTrivia(invocation.GetLeadingTrivia());
            object oldroot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            object newroot = oldroot.ReplaceNode(oldExpression, newExpression);
            object newDocument = document.WithSyntaxRoot(newroot);

            return newDocument;
        }

        private async Task<Document> ChangetoAwaitAsync(Document document, InvocationExpressionSyntax invocation, string name, CancellationToken cancellationToken)
        {
            SyntaxNode oldExpression = invocation;
            SyntaxNode newExpression = null;

            if (name.Equals("Wait"))
            {
                object expression = (invocation.Expression as MemberAccessExpressionSyntax).Expression;
                newExpression = SyntaxFactory.AwaitExpression(expression).WithAdditionalAnnotations(Formatter.Annotation);
            }

            object oldroot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            object newroot = oldroot.ReplaceNode(oldExpression, newExpression);
            object newDocument = document.WithSyntaxRoot(newroot);

            return newDocument;
        }

        private async Task<Document> ChangeToAwaitAsync(Document document, MemberAccessExpressionSyntax access, string name, CancellationToken cancellationToken)
        {
            SyntaxNode oldExpression = access;
            SyntaxNode newExpression = null;

            if (name.Equals("Result"))
            {
                newExpression = SyntaxFactory.AwaitExpression(access.Expression).WithAdditionalAnnotations(Formatter.Annotation);
            }

            if (newExpression != null)
            {
                object oldroot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
                object newroot = oldroot.ReplaceNode(oldExpression, newExpression);

                document = document.WithSyntaxRoot(newroot);
            }

            return document;
        }

        private async Task<Document> ChangetoAwaitGetAwaiterAsync(Document document, InvocationExpressionSyntax invocation, CancellationToken cancellationTkn)
        {
            SyntaxNode expression = invocation;
            while (!(expression is ExpressionStatementSyntax))
            {
                expression = expression.Parent;
            }

            var oldExpression = expression as ExpressionStatementSyntax;
            object awaitedInvocation = SyntaxFactory.AwaitExpression(invocation.WithLeadingTrivia(SyntaxFactory.Space)).WithLeadingTrivia(invocation.GetLeadingTrivia());
            object newExpression = oldExpression.WithExpression(awaitedInvocation);
            object oldroot = await document.GetSyntaxRootAsync(cancellationTkn).ConfigureAwait(false);
            object newroot = oldroot.ReplaceNode(oldExpression, newExpression);
            object newDocument = document.WithSyntaxRoot(newroot);

            return newDocument;
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return null;
        }

        private class CodeActionToDelayWhenAnyWhenAllAsync : CodeAction
        {
            private readonly Func<CancellationToken, Task<Document>> _generateDocument;
            private readonly string _title;

            public CodeActionToDelayWhenAnyWhenAllAsync(string title, Func<CancellationToken, Task<Document>> generateDocument)
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
                    return nameof(CodeActionToDelayWhenAnyWhenAllAsync);
                }
            }
        }

        private class CodeActionChangetoAwaitAsync : CodeAction
        {
            private readonly Func<CancellationToken, Task<Document>> _generateDocument;
            private readonly string _title;

            public CodeActionChangetoAwaitAsync(string title, Func<CancellationToken, Task<Document>> generateDocument)
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
                    return nameof(CodeActionChangetoAwaitAsync);
                }
            }
        }

        private class CodeActionChangetoAwaitGetAwaiterAsync : CodeAction
        {
            private readonly Func<CancellationToken, Task<Document>> _generateDocument;
            private readonly string _title;

            public CodeActionChangetoAwaitGetAwaiterAsync(string title, Func<CancellationToken, Task<Document>> generateDocument)
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
                    return nameof(CodeActionChangetoAwaitGetAwaiterAsync);
                }
            }
        }
    }
}
