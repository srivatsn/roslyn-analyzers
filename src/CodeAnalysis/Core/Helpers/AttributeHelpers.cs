// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Analyzers
{
    internal static class AttributeHelpers
    {
        internal static IEnumerable<AttributeData> GetApplicableAttributes(INamedTypeSymbol type)
        {
            var attributes = new List<AttributeData>();

            while (type != null)
            {
                attributes.AddRange(type.GetAttributes());

                type = type.BaseType;
            }

            return attributes;
        }

        internal static bool DerivesFrom(this INamedTypeSymbol symbol, INamedTypeSymbol candidateBaseType)
        {
            while (symbol != null)
            {
                if (symbol.Equals(candidateBaseType))
                {
                    return true;
                }

                symbol = symbol.BaseType;
            }

            return false;
        }
    }
}
