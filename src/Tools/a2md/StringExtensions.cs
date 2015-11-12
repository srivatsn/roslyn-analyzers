// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace a2md
{
    public static class StringExtensions
    {
        public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);
    }
}
