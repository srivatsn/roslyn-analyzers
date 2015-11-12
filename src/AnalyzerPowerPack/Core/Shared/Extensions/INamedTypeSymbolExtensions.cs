// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Microsoft.AnalyzerPowerPack.Utilities
{
    internal static class INamedTypeSymbolExtensions
    {
        public static IEnumerable<INamedTypeSymbol> GetBaseTypesAndThis(this INamedTypeSymbol namedType)
        {
            var current = namedType;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }
    }
}
