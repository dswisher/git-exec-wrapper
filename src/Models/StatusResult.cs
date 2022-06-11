// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace GitExecWrapper.Models
{
    public class StatusResult
    {
        public StatusResult()
        {
            Items = new List<StatusItem>();
        }

        public List<StatusItem> Items { get; }
    }
}
