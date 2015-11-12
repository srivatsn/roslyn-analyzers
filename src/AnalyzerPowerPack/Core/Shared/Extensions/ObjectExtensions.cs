// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.AnalyzerPowerPack.Utilities
{
    internal static class ObjectExtensions
    {
        public static TResult TypeSwitch<TBaseType, TDerivedType1, TDerivedType2, TResult>(this TBaseType obj, Func<TDerivedType1, TResult> matchFunc1, Func<TDerivedType2, TResult> matchFunc2, Func<TBaseType, TResult> defaultFunc = null)
            where TDerivedType1 : TBaseType
            where TDerivedType2 : TBaseType
        {
            if (obj is TDerivedType1)
            {
                return matchFunc1((TDerivedType1)obj);
            }
            else if (obj is TDerivedType2)
            {
                return matchFunc2((TDerivedType2)obj);
            }
            else if (defaultFunc != null)
            {
                return defaultFunc(obj);
            }
            else
            {
                return default(TResult);
            }
        }
    }
}
