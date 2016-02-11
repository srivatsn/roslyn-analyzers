// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Microsoft.Composition.Analyzers
{
    /// <summary>
    /// RS0006: Do not mix attributes from different versions of MEF
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class CSharpDoNotMixAttributesFromDifferentVersionsOfMEFAnalyzer : DoNotMixAttributesFromDifferentVersionsOfMEFAnalyzer
    {
    }
}