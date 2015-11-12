// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;

namespace System.Security.Cryptography.Hashing.Algorithms.Analyzers.Common
{
    public static class SecurityTypes
    {
        public static INamedTypeSymbol MD5(Compilation compilation)
        {
            return compilation.GetTypeByMetadataName("System.Security.Cryptography.MD5");
        }
        public static INamedTypeSymbol SHA1(Compilation compilation)
        {
            return compilation.GetTypeByMetadataName("System.Security.Cryptography.SHA1");
        }
        public static INamedTypeSymbol HMACSHA1(Compilation compilation)
        {
            return compilation.GetTypeByMetadataName("System.Security.Cryptography.HMACSHA1");
        }
    }
}
