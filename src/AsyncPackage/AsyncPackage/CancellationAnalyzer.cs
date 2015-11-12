// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;

namespace AsyncPackage
{
    /// <summary>
    /// This analyzer check to see if there are Cancellation Tokens that can be propagated through async method calls
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class CancellationAnalyzer : DiagnosticAnalyzer
    {
        internal const string CancellationId = "Async005";

        internal static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(id: CancellationId,
            title: "Propagate CancellationTokens When Possible",
            messageFormat: "This method can take a CancellationToken",
            category: "Library",
            defaultSeverity: DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCodeBlockStartAction<SyntaxKind>(CreateAnalyzerWithinCodeBlock);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        private void CreateAnalyzerWithinCodeBlock(CodeBlockStartAnalysisContext<SyntaxKind> context)
        {
            var methodDeclaration = context.OwningSymbol as IMethodSymbol;

            if (methodDeclaration != null)
            {
                ITypeSymbol cancellationTokenType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Threading.CancellationToken");
                object paramTypes = methodDeclaration.Parameters.Select(x => x.Type);

                if (paramTypes.Contains(cancellationTokenType))
                {
                    // Analyze the inside of the code block for invocationexpressions
                    context.RegisterSyntaxNodeAction(new CancellationAnalyzer_Inner().AnalyzeNode, SyntaxKind.InvocationExpression);
                }
            }
        }

        internal class CancellationAnalyzer_Inner
        {
            public void AnalyzeNode(SyntaxNodeAnalysisContext context)
            {
                var invokeMethod = context.SemanticModel.GetSymbolInfo(context.Node).Symbol as IMethodSymbol;

                if (invokeMethod != null)
                {
                    ITypeSymbol cancellationTokenType = context.SemanticModel.Compilation.GetTypeByMetadataName("System.Threading.CancellationToken");
                    object invokeParams = invokeMethod.Parameters.Select(x => x.Type);

                    if (invokeParams.Contains(cancellationTokenType))
                    {
                        var passedToken = false;

                        foreach (object arg in ((InvocationExpressionSyntax)context.Node).ArgumentList.Arguments)
                        {
                            object thisArgType = context.SemanticModel.GetTypeInfo(arg.Expression).Type;

                            if (thisArgType != null && thisArgType.Equals(cancellationTokenType))
                            {
                                passedToken = true;
                            }
                        }

                        if (!passedToken)
                        {
                            context.ReportDiagnostic(Diagnostic.Create(CancellationAnalyzer.Rule, context.Node.GetLocation()));
                        }
                    }
                }
            }
        }
    }
}
