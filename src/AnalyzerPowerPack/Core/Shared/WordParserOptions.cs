// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.AnalyzerPowerPack.Utilities
{
    /// <summary>
    ///   Defines the word parsing and delimiting options for use with <see cref="WordParser.Parse(String,WordParserOptions)"/>.
    /// </summary>
    [Flags]
    internal enum WordParserOptions
    {
        /// <summary>
        ///   Indicates the default options for word parsing.
        /// </summary>
        None = 0,

        /// <summary>
        ///   Indicates that <see cref="WordParser.Parse(String,WordParserOptions)"/> should ignore the mnemonic indicator characters (&amp;) embedded within words.
        /// </summary>
        IgnoreMnemonicsIndicators = 1,

        /// <summary>
        ///   Indicates that <see cref="WordParser.Parse(String,WordParserOptions)"/> should split compound words.
        /// </summary>
        SplitCompoundWords = 2,
    }
}
