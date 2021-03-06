// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;

namespace Microsoft.Maintainability.Analyzers
{
    /// <summary>
    /// CA1804: Remove unused locals
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class CSharpRemoveUnusedLocalsFixer : RemoveUnusedLocalsFixer
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create("CS0168");
    }
}