// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace GitExecWrapper.Models
{
    public class FetchResult
    {
        public FetchResult()
        {
            Items = new List<FetchItem>();
        }

        public string FromRepo { get; internal set; }
        public List<FetchItem> Items { get; }
    }
}
