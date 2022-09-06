// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace GitExecWrapper.Models
{
    public class BranchItem
    {
        public bool IsCurrent { get; internal set; }
        public string BranchName { get; internal set; }
        public string CommitSha { get; internal set; }
        public string UpstreamBranch { get; internal set; }
        public string Message { get; internal set; }
    }
}
