﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AnalyzerPowerPack.Usage;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.CodeAnalysis.UnitTests;
using Roslyn.Diagnostics.Test.Utilities;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.AnalyzerPowerPack.UnitTests
{
    public partial class CA2237FixerTests : CodeFixTestBase
    {
        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new SerializationRulesDiagnosticAnalyzer();
        }

        [WorkItem(858655, "DevDiv")]
        protected override CodeFixProvider GetBasicCodeFixProvider()
        {
            return new MarkTypesWithSerializableFixer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SerializationRulesDiagnosticAnalyzer();
        }

        [WorkItem(858655, "DevDiv")]
        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new MarkTypesWithSerializableFixer();
        }

        #region CA2237

        [Fact]
        public void CA2237SerializableMissingAttrFix()
        {
            VerifyCSharpFix(@"
using System;
using System.Runtime.Serialization;
public class CA2237SerializableMissingAttr : ISerializable
{
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}",
@"
using System;
using System.Runtime.Serialization;

[Serializable]
public class CA2237SerializableMissingAttr : ISerializable
{
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
}",
codeFixIndex: 0);

            VerifyBasicFix(@"
Imports System
Imports System.Runtime.Serialization
Public Class CA2237SerializableMissingAttr
    Implements ISerializable

    Protected Sub New(context As StreamingContext, info As SerializationInfo)
    End Sub

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext)
        throw new NotImplementedException()
    End Sub
End Class",
@"
Imports System
Imports System.Runtime.Serialization

<Serializable>
Public Class CA2237SerializableMissingAttr
    Implements ISerializable

    Protected Sub New(context As StreamingContext, info As SerializationInfo)
    End Sub

    Public Sub GetObjectData(info as SerializationInfo, context as StreamingContext)
        throw new NotImplementedException()
    End Sub
End Class",
codeFixIndex: 0);
        }

        #endregion
    }
}
