// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace GitExecWrapper.Models
{
    public enum RefStatus
    {
        Fetched,
        ForcedUpdate,
        Pruned,
        TagUpdate,
        NewRef,
        Failed,
        UpToDate
    }
}
