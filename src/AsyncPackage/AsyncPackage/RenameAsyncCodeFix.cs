// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPackage
{
    /// <summary>
    /// This codefix adds "Async" to the end of the Method Identifier and does a basic spellcheck in case the user had already tried to type Async
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = RenameAsyncAnalyzer.RenameAsyncId), Shared]
    public class RenameAsyncCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(RenameAsyncAnalyzer.RenameAsyncId); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            object root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            object diagnostic = context.Diagnostics.First();
            object diagnosticSpan = diagnostic.Location.SourceSpan;
            object methodDeclaration = root.FindToken(diagnosticSpan.Start).Parent.FirstAncestorOrSelf<MethodDeclarationSyntax>();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                new RenameAsyncCodeAction("Add Async to the end of the method name",
                                          c => RenameMethodAsync(context.Document, methodDeclaration, c)),
                diagnostic);
        }

        private async Task<Solution> RenameMethodAsync(Document document, MethodDeclarationSyntax methodDeclaration, CancellationToken cancellationToken)
        {
            object model = await document.GetSemanticModelAsync().ConfigureAwait(false);
            object oldSolution = document.Project.Solution;
            object symbol = model.GetDeclaredSymbol(methodDeclaration);
            object oldName = methodDeclaration.Identifier.ToString();
            string newName = string.Empty;

            // Check to see if name already contains Async at the end
            if (HasAsyncSuffix(oldName))
            {
                newName = oldName.Substring(0, oldName.Length - 5) + "Async";
            }
            else
            {
                newName = oldName + "Async";
            }

            object newSolution = await Renamer.RenameSymbolAsync(document.Project.Solution, symbol, newName, document.Project.Solution.Workspace.Options).ConfigureAwait(false);
            object syntaxTree = await document.GetSyntaxTreeAsync().ConfigureAwait(false);
            object descendentTokens = syntaxTree.GetRoot().DescendantTokens();
            object usedNames = descendentTokens.Where(token => token.IsKind(SyntaxKind.IdentifierToken)).Select(token => token.Value);

            while (usedNames.Contains(newName))
            {
                // No codefix should be offered if the name conflicts with another method.
                return oldSolution;
            }

            return newSolution;
        }

        /// <summary>
        /// This spellchecker obviously has limitations, but it may be helpful to some.
        /// </summary>
        /// <param name="oldName"></param>
        /// <returns>Returns a boolean of whether or not "Async" may have been in the Method name already but was mispelled.</returns>
        public bool HasAsyncSuffix(string oldName)
        {
            if (oldName.Length >= 5)
            {
                string last5letters = oldName.Substring(oldName.Length - 5);

                // Check case. The A in Async must be capitalized
                if (last5letters.Contains("async") || last5letters.Contains("asinc"))
                {
                    return true;
                }
                else if (((last5letters.Contains("A") || last5letters.Contains("a")) && last5letters.Contains("s") && last5letters.Contains("y")
                    && last5letters.Contains("n") && last5letters.Contains("c")) && !last5letters.ToLower().Equals("scany"))
                {
                    return true; // Basic spellchecker. This is obviously not conclusive, but it may catch a small error if the letters are simply switched around.
                }
            }

            return false;
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            return null;
        }

        private class RenameAsyncCodeAction : CodeAction
        {
            private readonly Func<CancellationToken, Task<Solution>> _generateSolution;
            private readonly string _title;

            public RenameAsyncCodeAction(string title, Func<CancellationToken, Task<Solution>> generateSolution)
            {
                _title = title;
                _generateSolution = generateSolution;
            }

            public override string Title { get { return _title; } }

            protected override Task<Solution> GetChangedSolutionAsync(CancellationToken cancellationToken)
            {
                return _generateSolution(cancellationToken);
            }

            public override string EquivalenceKey
            {
                get
                {
                    return nameof(RenameAsyncCodeAction);
                }
            }
        }
    }
}
