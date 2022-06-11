// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace GitExecWrapper.Models
{
    // TODO - is there a better name for this enum? Seems like it could conflict...
    public enum FileStatus
    {
        Unknown,
        Added,
        Modified,
        Deleted,
        Renamed,
        TypeChange,
        Ignored,
        Unchanged
    }
}
