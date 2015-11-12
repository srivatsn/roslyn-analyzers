// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;

namespace System.Runtime.Analyzers.UnitTests
{
    public partial class OverloadOperatorEqualsOnOverridingValueTypeEqualsTests : CodeFixTestBase
    {
        protected override CodeFixProvider GetBasicCodeFixProvider()
        {
            return new BasicOverloadOperatorEqualsOnOverridingValueTypeEqualsFixer();
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new CSharpOverloadOperatorEqualsOnOverridingValueTypeEqualsFixer();
        }

        [Fact]
        public void CA2231CSharpCodeFixNoEqualsOperator()
        {
            VerifyCSharpFix(@"
using System;

// value type without overriding Equals
public struct A
{    
    public override bool Equals(Object obj)
    {
        return true;
    }
}
",
@"
using System;

// value type without overriding Equals
public struct A
{    
    public override bool Equals(Object obj)
    {
        return true;
    }

    public static bool operator ==(A left, A right)
    {
        throw new NotImplementedException();
    }

    public static bool operator !=(A left, A right)
    {
        throw new NotImplementedException();
    }
}
",

                // This fix introduces the compiler warning:
                // Test0.cs(5,15): warning CS0661: 'A' defines operator == or operator != but does not override Object.GetHashCode()
                allowNewCompilerDiagnostics: true);
        }

        [Fact]
        public void CA2231BasicCodeFixNoEqualsOperator()
        {
            VerifyBasicFix(@"
Imports System

Public Structure A
    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        Return True
    End Function
End Structure
",
@"
Imports System

Public Structure A
    Public Overloads Overrides Function Equals(obj As Object) As Boolean
        Return True
    End Function

    Public Shared Operator =(left As A, right As A) As Boolean
        Throw New NotImplementedException()
    End Operator

    Public Shared Operator <>(left As A, right As A) As Boolean
        Throw New NotImplementedException()
    End Operator
End Structure
");
        }
    }
}
