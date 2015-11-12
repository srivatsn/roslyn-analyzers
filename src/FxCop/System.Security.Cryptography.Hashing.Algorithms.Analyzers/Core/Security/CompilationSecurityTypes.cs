// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;

namespace System.Security.Cryptography.Hashing.Algorithms.Analyzers.Common
{
    public class CompilationSecurityTypes
    {
        public INamedTypeSymbol MD5 { get; private set; }
        public INamedTypeSymbol SHA1 { get; private set; }
        public INamedTypeSymbol HMACSHA1 { get; private set; }

        public CompilationSecurityTypes(Compilation compilation)
        {
            MD5 = SecurityTypes.MD5(compilation);
            SHA1 = SecurityTypes.SHA1(compilation);
            HMACSHA1 = SecurityTypes.HMACSHA1(compilation);
        }
    }
}
