// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.CodeAnalysis.Diagnostics
{
    internal static class DiagnosticCustomTags
    {
        /// <summary>
        /// it is string[] because DiagnosticDescriptor expects string[]. 
        /// </summary>
        private static readonly string[] s_microsoftCustomTags = new string[] { WellKnownDiagnosticTags.Telemetry };

        public static string[] Microsoft
        {
            get
            {
                return s_microsoftCustomTags;
            }
        }
    }
}
