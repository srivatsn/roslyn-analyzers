// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.UnitTests;
using Roslyn.Test.Utilities;
using Xunit;

namespace System.Runtime.Analyzers.UnitTests
{
    public partial class AvoidUnsealedAttributeFixerTests : CodeFixTestBase
    {
        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new AvoidUnsealedAttributesAnalyzer();
        }

        protected override CodeFixProvider GetBasicCodeFixProvider()
        {
            return new AvoidUnsealedAttributesFixer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AvoidUnsealedAttributesAnalyzer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new AvoidUnsealedAttributesFixer();
        }

        #region CodeFix Tests

        [Fact]
        public void CA1813CSharpCodeFixProviderTestFired()
        {
            VerifyCSharpFix(@"
using System;

public class AttributeClass : Attribute
{
}", @"
using System;

public sealed class AttributeClass : Attribute
{
}");
        }

        [Fact]
        public void CA1813VisualBasicCodeFixProviderTestFired()
        {
            VerifyBasicFix(@"
Imports System

Public Class AttributeClass
    Inherits Attribute
End Class", @"
Imports System

Public NotInheritable Class AttributeClass
    Inherits Attribute
End Class");
        }

        #endregion
    }
}
