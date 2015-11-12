// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;

namespace Microsoft.CodeAnalysis
{
    internal class SolutionChangeAction : CodeAction
    {
        private readonly string _title;
        private readonly Func<CancellationToken, Task<Solution>> _createChangedSolution;

        public SolutionChangeAction(string title, Func<CancellationToken, Task<Solution>> createChangedSolution)
        {
            _title = title;
            _createChangedSolution = createChangedSolution;
        }

        public override string Title
        {
            get { return _title; }
        }

        protected override Task<Solution> GetChangedSolutionAsync(CancellationToken cancellationToken)
        {
            return _createChangedSolution(cancellationToken);
        }
    }
}
