﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;

namespace Desktop.Analyzers.UnitTests
{
    public class DoNotUseInsecureCryptographicAlgorithmsTests : DiagnosticAnalyzerTestBase
    {
        [Fact]
        public void CA5350UseHMACMD5CreateInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var md5 = new HMACMD5();
        }
    }
}",
            GetCSharpResultAt(10, 23, CA5351RuleName, _doNotUseMD5Message));

            VerifyBasic(@"
Imports System.Security.Cryptography

Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim md5 = New HMACMD5()
		End Sub
	End Class
End Namespace",
           GetBasicResultAt(7, 14, CA5351RuleName, _doNotUseMD5Message));
        }

        [Fact]
        public void CA5350CreateObjectFromHMACMD5DerivedClass()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyHMACMD5 : HMACMD5 {}

    class TestClass
    {
        private static void TestMethod()
        {
            var md5 = new MyHMACMD5();
        }
    }
}",
            GetCSharpResultAt(12, 23, CA5351RuleName, _doNotUseMD5Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class MyHMACMD5
		Inherits HMACMD5
	End Class

	Class TestClass
		Private Shared Sub TestMethod()
			Dim md5 = New MyHMACMD5()
		End Sub
	End Class
End Namespace",
           GetBasicResultAt(10, 14, CA5351RuleName, _doNotUseMD5Message));
        }

        [Fact]
        public void CA5350UseHMACMD5CreateInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass1
    {
        public HMACMD5 GetHMACMD5
        {
            get { return new HMACMD5(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5351RuleName, _doNotUseMD5Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass1
		Public ReadOnly Property GetHMACMD5() As HMACMD5
			Get
				Return New HMACMD5()
			End Get
		End Property
	End Class
End Namespace",
GetBasicResultAt(7, 12, CA5351RuleName, _doNotUseMD5Message));
        }

        [Fact]
        public void CA5350UseHMACMD5InFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        HMACMD5 privateMd5 = new HMACMD5();
    }
}",
            GetCSharpResultAt(7, 30, CA5351RuleName, _doNotUseMD5Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateMd5 As New HMACMD5()
	End Class
End Namespace",
GetBasicResultAt(5, 25, CA5351RuleName, _doNotUseMD5Message));
        }

        [Fact]
        public void CA5350UseHMACMD5InLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { new HMACMD5(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5351RuleName, _doNotUseMD5Message));

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass
    Public Async Sub TestMethod()
        Await Task.Run(Function()
                           Return New HMACMD5()
                       End Function)
    End Sub
End Module",
            GetBasicResultAt(7, 35, CA5351RuleName, _doNotUseMD5Message));
        }

        [Fact]
        public void CA5350UseHMACMD5InAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { new HMACMD5(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5351RuleName, _doNotUseMD5Message));

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass 
    Delegate Function Del() As HashAlgorithm
    Dim d As Del = Function() New HMACMD5()
End Module",
            GetBasicResultAt(6, 31, CA5351RuleName, _doNotUseMD5Message));
        }

        [Fact]
        public void CA5351UseDESCreateInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var aes = DES.Create();  
        }
    }
}",
            GetCSharpResultAt(10, 23, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass
    Sub TestMethod()
        Dim desalg As DES = DES.Create()
    End Sub
End Module",
GetBasicResultAt(6, 29, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351UseDESCreateInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass
    {
        public DES GetDES
        {
            get { return DES.Create(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Public ReadOnly Property GetDES() As DES
			Get
				Return DES.Create()
			End Get
		End Property
	End Class
End Namespace
",
GetBasicResultAt(7, 12, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351UseDESCreateInFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        DES privateDES = DES.Create();
    }
}",
            GetCSharpResultAt(7, 26, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateDES As DES = DES.Create()
	End Class
End Namespace",
GetBasicResultAt(5, 31, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351UseDESCreateInLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { DES.Create(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Imports System.Threading.Tasks
Namespace TestNamespace
	Class TestClass
		Private Function TestMethod() As Task
			Await Task.Run(Function() 
			DES.Create()
End Function)
		End Function
	End Class
End Namespace",
GetBasicResultAt(8, 4, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351UseDESCreateInAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { DES.Create(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Delegate Sub Del()
		Private d As Del = Sub() DES.Create()
	End Class
End Namespace",
GetBasicResultAt(6, 28, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351UseDESCryptoServiceProviderCreateInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            DES des = new DESCryptoServiceProvider();
        }
    }
}",
            GetCSharpResultAt(10, 23, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim des As DES = New DESCryptoServiceProvider()
		End Sub
	End Class
End Namespace",
            GetBasicResultAt(6, 21, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351UseDESCryptoServiceProviderCreateInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass
    {
        public DESCryptoServiceProvider GetDES
        {
            get { return new DESCryptoServiceProvider(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Public ReadOnly Property GetDES() As DESCryptoServiceProvider
			Get
				Return New DESCryptoServiceProvider()
			End Get
		End Property
	End Class
End Namespace",
           GetBasicResultAt(7, 12, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351UseDESCryptoServiceProviderCreateInFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        DESCryptoServiceProvider privateDES = new DESCryptoServiceProvider();
    }
}",
            GetCSharpResultAt(7, 47, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateDES As New DESCryptoServiceProvider()
	End Class
End Namespace",
GetBasicResultAt(5, 25, CA5351RuleName, _doNotUseDESMessage));
        }
        //No VB        
        [Fact]
        public void CA5351UseDESCryptoServiceProviderInLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { new DESCryptoServiceProvider(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5351RuleName, _doNotUseDESMessage));
        }
        //No VB        
        [Fact]
        public void CA5351UseDESCryptoServiceProviderInAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { new DESCryptoServiceProvider(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5351CreateObjectFromDESDerivedClass()
        {
            VerifyCSharp(new[] {
//Test0
                @"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            MyDES des = new MyDES();
            des.GenerateKey();
        }
    }
}",
//Test1
                @"
using System;
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyDES : DES
    {
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override void GenerateIV()
        {
            throw new NotImplementedException();
        }

        public override void GenerateKey()
        {
            throw new NotImplementedException();
        }
    }
}" },
            GetCSharpResultAt(10, 25, CA5351RuleName, _doNotUseDESMessage),
            GetCSharpResultAt(11, 13, CA5351RuleName, _doNotUseDESMessage));

            VerifyBasic(new[] {
//Test0
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim des As New MyDES()
			des.GenerateKey()
		End Sub
	End Class
End Namespace",
//Test1
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class MyDES
		Inherits DES
		Public Overrides Function CreateDecryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Function CreateEncryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Sub GenerateIV()
			Throw New NotImplementedException()
		End Sub

		Public Overrides Sub GenerateKey()
			Throw New NotImplementedException()
		End Sub
	End Class
End Namespace
" },
           GetBasicResultAt(6, 15, CA5351RuleName, _doNotUseDESMessage),
           GetBasicResultAt(7, 4, CA5351RuleName, _doNotUseDESMessage));
        }

        [Fact]
        public void CA5352UseRC2CryptoServiceProviderInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var rc2 = new RC2CryptoServiceProvider();
        }
    }
}",
            GetCSharpResultAt(10, 23, CA5351RuleName, _doNotUseRC2Message));

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass
    Sub TestMethod()
        Dim rc2alg As New RC2CryptoServiceProvider
    End Sub
End Module",
GetBasicResultAt(6, 23, CA5351RuleName, _doNotUseRC2Message));
        }

        [Fact]
        public void CA5352UseRC2CryptoServiceProviderInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass
    {
        public RC2CryptoServiceProvider GetRC2
        {
            get { return new RC2CryptoServiceProvider(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5351RuleName, _doNotUseRC2Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Public ReadOnly Property GetRC2() As RC2CryptoServiceProvider
			Get
				Return New RC2CryptoServiceProvider()
			End Get
		End Property
	End Class
End Namespace",
GetBasicResultAt(7, 12, CA5351RuleName, _doNotUseRC2Message));
        }

        [Fact]
        public void CA5352UseRC2CryptoServiceProviderInFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        RC2CryptoServiceProvider privateRC2 = new RC2CryptoServiceProvider();
    }
}",
            GetCSharpResultAt(7, 47, CA5351RuleName, _doNotUseRC2Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateRC2 As New RC2CryptoServiceProvider()
	End Class
End Namespace
",
GetBasicResultAt(5, 25, CA5351RuleName, _doNotUseRC2Message));
        }
        //No VB            
        [Fact]
        public void CA5352UseRC2CryptoServiceProviderInLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { new RC2CryptoServiceProvider(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5351RuleName, _doNotUseRC2Message));
        }
        //No VB        
        [Fact]
        public void CA5352UseRC2CryptoServiceProviderInAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { new RC2CryptoServiceProvider(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5351RuleName, _doNotUseRC2Message));
        }

        [Fact]
        public void CA5352CreateObjectFromRC2DerivedClass()
        {
            VerifyCSharp(new[] {
//Test0
@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var rc2 = new MyRC2();
        }
    }
}",
//Test1
@"
using System;
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyRC2 : RC2
    {
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override void GenerateIV()
        {
            throw new NotImplementedException();
        }

        public override void GenerateKey()
        {
            throw new NotImplementedException();
        }
    }
}" },
            GetCSharpResultAt(10, 23, CA5351RuleName, _doNotUseRC2Message));

            VerifyBasic(new[] {
//Test0
@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim rc2 = New MyRC2()
		End Sub
	End Class
End Namespace",
//Test1
@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class MyRC2
		Inherits RC2
		Public Overrides Function CreateDecryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Function CreateEncryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Sub GenerateIV()
			Throw New NotImplementedException()
		End Sub

		Public Overrides Sub GenerateKey()
			Throw New NotImplementedException()
		End Sub
	End Class
End Namespace
" },
           GetBasicResultAt(6, 14, CA5351RuleName, _doNotUseRC2Message));
        }

        [Fact]
        public void CA5353TripleDESCreateInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var tripleDES = TripleDES.Create(""TripleDES"");  
        }
    }
}",
            GetCSharpResultAt(10, 29, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim tripleDES__1 = TripleDES.Create(""TripleDES"")
        End Sub
    End Class
End Namespace",
            GetBasicResultAt(6, 23, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5353TripleDESCreateInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass
    {
        public TripleDES GetTripleDES
        {
            get { return TripleDES.Create(""TripleDES""); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Public ReadOnly Property GetTripleDES() As TripleDES
			Get
				Return TripleDES.Create(""TripleDES"")
            End Get
        End Property
    End Class
End Namespace",
           GetBasicResultAt(7, 12, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5353TripleDESCreateInFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        TripleDES privateDES = TripleDES.Create(""TripleDES"");
    }
}",
            GetCSharpResultAt(7, 32, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateDES As TripleDES = TripleDES.Create(""TripleDES"")
    End Class
End Namespace",
           GetBasicResultAt(5, 37, CA5350RuleName, _doNotUseTripleDESMessage));
        }
        //No VB
        [Fact]
        public void CA5353TripleDESCreateInLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { TripleDES.Create(""TripleDES""); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5353TripleDESCreateInAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { TripleDES.Create(""TripleDES""); };
    }
}",
            GetCSharpResultAt(8, 31, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Delegate Sub Del()
		Private d As Del = Sub() TripleDES.Create(""TripleDES"")
    End Class
End Namespace",
GetBasicResultAt(6, 28, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5353TripleDESCryptoServiceProviderInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
        }
    }
}",
            GetCSharpResultAt(10, 56, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass
    Sub TestMethod()
        Dim tDESalg As New TripleDESCryptoServiceProvider
    End Sub
End Module",
GetBasicResultAt(6, 24, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5353TripleDESCryptoServiceProviderInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass
    {
        public TripleDESCryptoServiceProvider GetDES
        {
            get { return new TripleDESCryptoServiceProvider(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Public ReadOnly Property GetDES() As TripleDESCryptoServiceProvider
			Get
				Return New TripleDESCryptoServiceProvider()
			End Get
		End Property
	End Class
End Namespace",
            GetBasicResultAt(7, 12, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5353TripleDESCryptoServiceProviderInFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        TripleDESCryptoServiceProvider privateDES = new TripleDESCryptoServiceProvider();
    }
}",
            GetCSharpResultAt(7, 53, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateDES As New TripleDESCryptoServiceProvider()
	End Class
End Namespace",
GetBasicResultAt(5, 25, CA5350RuleName, _doNotUseTripleDESMessage));
        }
        //No VB       
        [Fact]
        public void CA5353TripleDESCryptoServiceProviderInLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { new TripleDESCryptoServiceProvider(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5350RuleName, _doNotUseTripleDESMessage));
        }
        //No VB        
        [Fact]
        public void CA5353TripleDESCryptoServiceProviderInAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { new TripleDESCryptoServiceProvider(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5353CreateObjectFromTripleDESDerivedClass()
        {
            VerifyCSharp(new[] {
//Test0
                @"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var my3DES = new My3DES();
            my3DES.GenerateKey();
        }
    }
}",
//Test1
                @"
using System;
using System.Security.Cryptography;

namespace TestNamespace
{
    class My3DES : TripleDES
    {
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override void GenerateIV()
        {
            throw new NotImplementedException();
        }

        public override void GenerateKey()
        {
            throw new NotImplementedException();
        }
    }
}" },
            GetCSharpResultAt(10, 26, CA5350RuleName, _doNotUseTripleDESMessage),
            GetCSharpResultAt(11, 13, CA5350RuleName, _doNotUseTripleDESMessage));

            VerifyBasic(new[] {
//Test0
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim my3DES = New My3DES()
			my3DES.GenerateKey()
		End Sub
	End Class
End Namespace",

//Test1
                @"
Imports System.Security.Cryptography

Namespace TestNamespace
	Class My3DES
		Inherits TripleDES
		Public Overrides Function CreateDecryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Function CreateEncryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Sub GenerateIV()
			Throw New NotImplementedException()
		End Sub

		Public Overrides Sub GenerateKey()
			Throw New NotImplementedException()
		End Sub
	End Class
End Namespace
" },
            GetBasicResultAt(6, 17, CA5350RuleName, _doNotUseTripleDESMessage),
            GetBasicResultAt(7, 4, CA5350RuleName, _doNotUseTripleDESMessage));
        }

        [Fact]
        public void CA5355RIPEMD160ManagedInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var md160 = new RIPEMD160Managed();
        }
    }
}",
            GetCSharpResultAt(10, 25, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass
    Sub TestMethod()
        Dim md1601alg As New RIPEMD160Managed
    End Sub
End Module",
GetBasicResultAt(6, 26, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355RIPEMD160ManagedInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass1
    {
        public RIPEMD160Managed GetRIPEMD160
        {
            get { return new RIPEMD160Managed(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass1
		Public ReadOnly Property GetRIPEMD160() As RIPEMD160Managed
			Get
				Return New RIPEMD160Managed()
			End Get
		End Property
	End Class
End Namespace",
            GetBasicResultAt(7, 12, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355RIPEMD160ManagedInFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        RIPEMD160Managed privateRIPEMD160 = new RIPEMD160Managed();
    }
}",
            GetCSharpResultAt(7, 45, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateRIPEMD160 As New RIPEMD160Managed()
	End Class
End Namespace
",
        GetBasicResultAt(5, 31, CA5350RuleName, _doNotUseRIPEMD160Message));
        }
        //No VB               
        [Fact]
        public void CA5355RIPEMD160ManagedInLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { new RIPEMD160Managed(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5350RuleName, _doNotUseRIPEMD160Message));
        }
        //No VB        
        [Fact]
        public void CA5355RIPEMD160ManagedInAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { new RIPEMD160Managed(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355RIPEMD160CreateInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            RIPEMD160 md160 = RIPEMD160.Create();
        }
    }
}",
            GetCSharpResultAt(10, 31, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim md160 As RIPEMD160 = RIPEMD160.Create()
		End Sub
	End Class
End Namespace",
            GetBasicResultAt(6, 29, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355RIPEMD160CreateInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass1
    {
        public RIPEMD160 GetRIPEMD160
        {
            get { return RIPEMD160.Create(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass1
		Public ReadOnly Property GetRIPEMD160() As RIPEMD160
			Get
				Return RIPEMD160.Create()
			End Get
		End Property
	End Class
End Namespace",
GetBasicResultAt(7, 12, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355RIPEMD160CreateInFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        RIPEMD160 privateRIPEMD160 = RIPEMD160.Create();
    }
}",
            GetCSharpResultAt(7, 38, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateRIPEMD160 As RIPEMD160 = RIPEMD160.Create()
	End Class
End Namespace",
            GetBasicResultAt(5, 43, CA5350RuleName, _doNotUseRIPEMD160Message));
        }
        //No VB                
        [Fact]
        public void CA5355RIPEMD160CreateInLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { RIPEMD160.Create(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355RIPEMD160CreateInAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { RIPEMD160.Create(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
    Class TestClass
        Private Delegate Sub Del()
        Private d As Del = Sub() RIPEMD160.Create()
    End Class
End Namespace",
          GetBasicResultAt(6, 34, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355HMACRIPEMD160InMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var md160 = new HMACRIPEMD160();
        }
    }
}",
            GetCSharpResultAt(10, 25, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim md160 = New HMACRIPEMD160()
		End Sub
	End Class
End Namespace",
            GetBasicResultAt(6, 16, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355HMACRIPEMD160InGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass1
    {
        public HMACRIPEMD160 GetHMARIPEMD160
        {
            get { return new HMACRIPEMD160(); }
        }
    }
}",
            GetCSharpResultAt(9, 26, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass1
		Public ReadOnly Property GetHMARIPEMD160() As HMACRIPEMD160
			Get
				Return New HMACRIPEMD160()
			End Get
		End Property
	End Class
End Namespace",
            GetBasicResultAt(7, 12, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355HMACRIPEMD160InFieldDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        HMACRIPEMD160 privateHMARIPEMD160 = new HMACRIPEMD160();
    }
}",
            GetCSharpResultAt(7, 45, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateHMARIPEMD160 As New HMACRIPEMD160()
	End Class
End Namespace",
           GetBasicResultAt(5, 34, CA5350RuleName, _doNotUseRIPEMD160Message));
        }
        //No VB        
        [Fact]
        public void CA5355HMACRIPEMD160InLambdaExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { new HMACRIPEMD160(); });
        }
    }
}",
            GetCSharpResultAt(10, 36, CA5350RuleName, _doNotUseRIPEMD160Message));
        }
        //No VB        
        [Fact]
        public void CA5355HMACRIPEMD160InAnonymousMethodExpression()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { new HMACRIPEMD160(); };
    }
}",
            GetCSharpResultAt(8, 31, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355CreateObjectFromRIPEMD160DerivedClass()
        {
            VerifyCSharp(new[] {
//Test0
                @"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod(byte[] inBytes)
        {
            var md160 = new MyRIPEMD160();
        }
    }
}",
//Test1
                @"
using System;
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyRIPEMD160 : RIPEMD160
    {
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }
    }
}" },
            GetCSharpResultAt(10, 25, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(new[] {
//Test0
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod(inBytes As Byte())
			Dim md160 = New MyRIPEMD160()
		End Sub
	End Class
End Namespace",
//Test1
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class MyRIPEMD160
		Inherits RIPEMD160
		Public Overrides Sub Initialize()
			Throw New NotImplementedException()
		End Sub

		Protected Overrides Sub HashCore(array As Byte(), ibStart As Integer, cbSize As Integer)
			Throw New NotImplementedException()
		End Sub

		Protected Overrides Function HashFinal() As Byte()
			Throw New NotImplementedException()
		End Function
	End Class
End Namespace" },
            GetBasicResultAt(6, 16, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355CreateObjectFromRIPEMD160ManagedDerivedClass()
        {
            VerifyCSharp(new[] {
//Test0
                @"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod(byte[] inBytes)
        {
            var md160 = new MyRIPEMD160();
        }
    }
}",
//Test1
                @"
using System;
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyRIPEMD160 : RIPEMD160Managed
    {
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new NotImplementedException();
        }

        protected override byte[] HashFinal()
        {
            throw new NotImplementedException();
        }
    }
}" },
            GetCSharpResultAt(10, 25, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(new[] {
//Test0
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod(inBytes As Byte())
			Dim md160 = New MyRIPEMD160()
		End Sub
	End Class
End Namespace",
//Test1
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class MyRIPEMD160
		Inherits RIPEMD160Managed
		Public Overrides Sub Initialize()
			Throw New NotImplementedException()
		End Sub

		Protected Overrides Sub HashCore(array As Byte(), ibStart As Integer, cbSize As Integer)
			Throw New NotImplementedException()
		End Sub

		Protected Overrides Function HashFinal() As Byte()
			Throw New NotImplementedException()
		End Function
	End Class
End Namespace
" },
            GetBasicResultAt(6, 16, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5355CreateObjectFromHMACRIPEMD160DerivedClass()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyHMACRIPEMD160 : HMACRIPEMD160 {}

    class TestClass
    {
        private static void TestMethod()
        {
            var md160 = new MyHMACRIPEMD160();
        }
    }
}",
            GetCSharpResultAt(12, 25, CA5350RuleName, _doNotUseRIPEMD160Message));

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class MyHMACRIPEMD160
		Inherits HMACRIPEMD160
	End Class

	Class TestClass
		Private Shared Sub TestMethod()
			Dim md160 = New MyHMACRIPEMD160()
		End Sub
	End Class
End Namespace",
            GetBasicResultAt(10, 16, CA5350RuleName, _doNotUseRIPEMD160Message));
        }

        [Fact]
        public void CA5356DSACreateSignatureInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod(DSA dsa, byte[] inBytes)
        {
            var sig = dsa.CreateSignature(inBytes);
        }
    }
}",
            GetCSharpResultAt(10, 23, CA5351RuleName, _doNotUseDSAMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass
    Function TestMethod(ByVal bytes As Byte())
        Dim dsa As New DSACryptoServiceProvider
        Return dsa.CreateSignature(bytes)
    End Function
End Module",
GetBasicResultAt(7, 16, CA5351RuleName, _doNotUseDSAMessage));
        }

        [Fact]
        public void CA5356UseDSACreateSignatureInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

class TestClass
{
    DSA dsa1 = null;
    public byte[] MyProperty
    {
        get
        {
            byte[] inBytes = null;
            return dsa1.CreateSignature(inBytes);
        }
    }
}",
            GetCSharpResultAt(12, 20, CA5351RuleName, _doNotUseDSAMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography

Class TestClass
	Private dsa1 As DSA = Nothing
	Public ReadOnly Property MyProperty() As Byte()
		Get
			Dim inBytes As Byte() = Nothing
			Return dsa1.CreateSignature(inBytes)
		End Get
	End Property
End Class",
            GetBasicResultAt(9, 11, CA5351RuleName, _doNotUseDSAMessage));
        }

        [Fact]
        public void CA5356DSASignatureFormatterInMethodDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var sf1 = new DSASignatureFormatter();
            var sf2 = new DSASignatureFormatter(new DSACryptoServiceProvider());
        }
    }
}",
            GetCSharpResultAt(10, 23, CA5351RuleName, _doNotUseDSAMessage),
            GetCSharpResultAt(11, 23, CA5351RuleName, _doNotUseDSAMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography

Namespace TestNamespace
    Class TestClass
        Private Shared Sub TestMethod()
            Dim sf1 = New DSASignatureFormatter()
            Dim sf2 = New DSASignatureFormatter(New DSACryptoServiceProvider())
        End Sub
    End Class
End Namespace",
           GetBasicResultAt(7, 23, CA5351RuleName, _doNotUseDSAMessage),
           GetBasicResultAt(8, 23, CA5351RuleName, _doNotUseDSAMessage));
        }

        [Fact]
        public void CA5356UseDSACreateSignatureFormatterInGetDeclaration()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

class TestClass
{
    DSA dsa1 = null;
    public DSASignatureFormatter MyProperty
    {
        get
        {
            DSASignatureFormatter inBytes = null;
            if (inBytes == null) { return new DSASignatureFormatter(); }
            else return new DSASignatureFormatter(new DSACryptoServiceProvider());
        }
    }
}",
            GetCSharpResultAt(12, 43, CA5351RuleName, _doNotUseDSAMessage),
            GetCSharpResultAt(13, 25, CA5351RuleName, _doNotUseDSAMessage));

            VerifyBasic(@"
Imports System.Security.Cryptography
Class TestClass
	Private dsa1 As DSA = Nothing
	Public ReadOnly Property MyProperty() As DSASignatureFormatter
		Get
			Dim inBytes As DSASignatureFormatter = Nothing
			If inBytes Is Nothing Then
				Return New DSASignatureFormatter()
			Else
				Return New DSASignatureFormatter(New DSACryptoServiceProvider())
			End If
		End Get
	End Property
End Class",
            GetBasicResultAt(9, 12, CA5351RuleName, _doNotUseDSAMessage),
            GetBasicResultAt(11, 12, CA5351RuleName, _doNotUseDSAMessage));
        }

        [Fact]
        public void CA5356CreateSignatureFromDSADerivedClass()
        {
            VerifyCSharp(new[] {
                //Test0
                @"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod(byte[] inBytes)
        {
            var myDsa = new MyDsa();
            myDsa.CreateSignature(inBytes);
        }
    }
}",
                //Test1
                @"
using System;
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyDsa : DSA
    {
        public override string KeyExchangeAlgorithm
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string SignatureAlgorithm
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override byte[] CreateSignature(byte[] rgbHash)
        {
            throw new NotImplementedException();
        }

        public override DSAParameters ExportParameters(bool includePrivateParameters)
        {
            throw new NotImplementedException();
        }

        public override void ImportParameters(DSAParameters parameters)
        {
            throw new NotImplementedException();
        }

        public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
        {
            throw new NotImplementedException();
        }
    }
}" },
            GetCSharpResultAt(11, 13, CA5351RuleName, _doNotUseDSAMessage));

            VerifyBasic(new[] {
//Test0
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod(inBytes As Byte())
			Dim myDsa = New MyDsa()
			myDsa.CreateSignature(inBytes)
		End Sub
	End Class
End Namespace",
//Test1
                @"
Imports System.Security.Cryptography

Namespace TestNamespace
	Class MyDsa
		Inherits DSA
		Public Overrides ReadOnly Property KeyExchangeAlgorithm() As String
			Get
				Throw New NotImplementedException()
			End Get
		End Property

		Public Overrides ReadOnly Property SignatureAlgorithm() As String
			Get
				Throw New NotImplementedException()
			End Get
		End Property

		Public Overrides Function CreateSignature(rgbHash As Byte()) As Byte()
			Throw New NotImplementedException()
		End Function

		Public Overrides Function ExportParameters(includePrivateParameters As Boolean) As DSAParameters
			Throw New NotImplementedException()
		End Function

		Public Overrides Sub ImportParameters(parameters As DSAParameters)
			Throw New NotImplementedException()
		End Sub

		Public Overrides Function VerifySignature(rgbHash As Byte(), rgbSignature As Byte()) As Boolean
			Throw New NotImplementedException()
		End Function
	End Class
End Namespace" },
           GetBasicResultAt(7, 4, CA5351RuleName, _doNotUseDSAMessage));
        }

        [Fact]
        public void CA5357RijndaelManagedInMethodDeclarationShouldNotGenerateDiagnostics()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var rc2 = new RijndaelManaged();
        }
    }
}"
            );

            VerifyBasic(@"
Imports System.Security.Cryptography

Module TestClass
    Sub TestMethod()
        Dim rijndaelalg As New RijndaelManaged
    End Sub
End Module"
            );
        }

        [Fact]
        public void CA5357RijndaelManagedInGetDeclarationShouldNotGenerateDiagnostics()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
   class TestClass1
    {
        public RijndaelManaged GetRijndael
        {
            get { return new RijndaelManaged(); }
        }
    }
}"
            );

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass1
		Public ReadOnly Property GetRijndael() As RijndaelManaged
			Get
				Return New RijndaelManaged()
			End Get
		End Property
	End Class
End Namespace"
            );
        }

        [Fact]
        public void CA5357RijndaelManagedInFieldDeclarationShouldNotGenerateDiagnostics()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        RijndaelManaged privateRijndael = new RijndaelManaged();
    }
}"
            );

            VerifyBasic(@"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private privateRijndael As New RijndaelManaged()
	End Class
End Namespace"
            );
        }
        //No VB                    
        [Fact]
        public void CA5357RijndaelManagedInLambdaExpressionShouldNotGenerateDiagnostics()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace TestNamespace
{
    class TestClass
    {
        private async Task TestMethod()
        {
            await Task.Run(() => { new RijndaelManaged(); });
        }
    }
}"
            );
        }
        //No VB        
        [Fact]
        public void CA5357RijndaelManagedInAnonymousMethodExpressionShouldNotGenerateDiagnostics()
        {
            VerifyCSharp(@"
using System.Security.Cryptography;
namespace TestNamespace
{
    class TestClass
    {
        delegate void Del();
        Del d = delegate () { new RijndaelManaged(); };
    }
}"
            );
        }

        [Fact]
        public void CA5357CreateObjectFromRijndaelDerivedClassShouldNotGenerateDiagnostics()
        {
            VerifyCSharp(new[] {
//Test0
                @"
using System.Security.Cryptography;

namespace TestNamespace
{
    class TestClass
    {
        private static void TestMethod()
        {
            var rc2 = new MyRijndael();
        }
    }
}",
//Test1
                @"
using System;
using System.Security.Cryptography;

namespace TestNamespace
{
    class MyRijndael : Rijndael
    {
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            throw new NotImplementedException();
        }

        public override void GenerateIV()
        {
            throw new NotImplementedException();
        }

        public override void GenerateKey()
        {
            throw new NotImplementedException();
        }
    }
}" }
            );

            VerifyBasic(new[] {
//Test0
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class TestClass
		Private Shared Sub TestMethod()
			Dim rc2 = New MyRijndael()
		End Sub
	End Class
End Namespace",
//Test1
                @"
Imports System.Security.Cryptography
Namespace TestNamespace
	Class MyRijndael
		Inherits Rijndael
		Public Overrides Function CreateDecryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Function CreateEncryptor(rgbKey As Byte(), rgbIV As Byte()) As ICryptoTransform
			Throw New NotImplementedException()
		End Function

		Public Overrides Sub GenerateIV()
			Throw New NotImplementedException()
		End Sub

		Public Overrides Sub GenerateKey()
			Throw New NotImplementedException()
		End Sub
	End Class
End Namespace" }
            );
        }

        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new BasicDoNotUseInsecureCryptographicAlgorithmsAnalyzer();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CSharpDoNotUseInsecureCryptographicAlgorithmsAnalyzer();
        }

        private const string CA5350RuleName = DoNotUseInsecureCryptographicAlgorithmsAnalyzer.DoNotUseWeakCryptographicRuleId;
        private const string CA5351RuleName = DoNotUseInsecureCryptographicAlgorithmsAnalyzer.DoNotUseBrokenCryptographicRuleId;

        private readonly string _doNotUseMD5Message = DesktopAnalyzersResources.DoNotUseMD5;
        private readonly string _doNotUseDESMessage = DesktopAnalyzersResources.DoNotUseDES;
        private readonly string _doNotUseRC2Message = DesktopAnalyzersResources.DoNotUseRC2;
        private readonly string _doNotUseTripleDESMessage = DesktopAnalyzersResources.DoNotUseTripleDES;
        private readonly string _doNotUseRIPEMD160Message = DesktopAnalyzersResources.DoNotUseRIPEMD160;
        private readonly string _doNotUseDSAMessage = DesktopAnalyzersResources.DoNotUseDSA;
    }
}
