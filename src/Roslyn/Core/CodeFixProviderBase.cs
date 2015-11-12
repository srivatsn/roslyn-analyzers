// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Microsoft.CodeAnalysis
{
    public abstract class CodeFixProviderBase : CodeFixProvider
    {
        protected abstract string GetCodeFixDescription(string ruleId);

        internal abstract Task<Document> GetUpdatedDocumentAsync(Document document, SemanticModel model, SyntaxNode root, SyntaxNode nodeToFix, string diagnosticId, CancellationToken cancellationToken);

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var document = context.Document;
            var cancellationToken = context.CancellationToken;

            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var model = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            foreach (var diagnostic in context.Diagnostics)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var nodeToFix = root.FindNode(diagnostic.Location.SourceSpan);

                var newDocument = await GetUpdatedDocumentAsync(document, model, root, nodeToFix, diagnostic.Id, cancellationToken).ConfigureAwait(false);

                Debug.Assert(newDocument != null);
                if (newDocument != document)
                {
                    var codeFixDescription = GetCodeFixDescription(diagnostic.Id);
                    context.RegisterCodeFix(new MyCodeAction(codeFixDescription, newDocument), diagnostic);
                }
            }
        }

        private class MyCodeAction : DocumentChangeAction
        {
            public MyCodeAction(string title, Document newDocument) :
                base(title, c => Task.FromResult(newDocument))
            {
            }
        }
    }
}
