// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace a2md
{
    public sealed class DescriptorEqualityComparer : IEqualityComparer<DiagnosticDescriptor>
    {
        public bool Equals(DiagnosticDescriptor x, DiagnosticDescriptor y) => x.Id.Equals(y.Id);

        public int GetHashCode(DiagnosticDescriptor obj) => obj.Id.GetHashCode();
    }
}
