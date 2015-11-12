// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;

namespace Microsoft.CodeAnalysis
{
    internal class DocumentChangeAction : CodeAction
    {
        private readonly string _title;
        private readonly Func<CancellationToken, Task<Document>> _createChangedDocument;

        public DocumentChangeAction(string title, Func<CancellationToken, Task<Document>> createChangedDocument)
        {
            _title = title;
            _createChangedDocument = createChangedDocument;
        }

        public override string Title
        {
            get { return _title; }
        }

        protected override Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
        {
            return _createChangedDocument(cancellationToken);
        }
    }
}