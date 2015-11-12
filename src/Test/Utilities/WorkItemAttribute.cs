// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Roslyn.Diagnostics.Test.Utilities
{
    public class WorkItemAttribute : Attribute
    {
        private int _id;
        private string _source;

        public WorkItemAttribute(int id, string source)
        {
            _id = id;
            _source = source;
        }
    }
}