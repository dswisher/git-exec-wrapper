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

        /// <summary>
        /// Gets the number of commits that exist in the local branch but do not exist in the upstream branch.
        /// </summary>
        /// <remarks>
        /// May be null, if the upstream branch does not exist.
        /// </remarks>
        public int? CommitsAhead { get; internal set; }

        /// <summary>
        /// Gets the number of commits that exist in the upstream branch but do not exist in the local branch.
        /// </summary>
        /// <remarks>
        /// May be null, if the upstream branch does not exist.
        /// </remarks>
        public int? CommitsBehind { get; internal set; }

        /// <summary>
        /// Gets the SHA of the current commit.
        /// </summary>
        /// <remarks>
        /// May be null, if there is not yet a commit.
        /// </remarks>
        public string CurrentCommit { get; internal set; }

        /// <summary>
        /// Gets the name of the current branch.
        /// </summary>
        /// <remarks>
        /// May be null, if the current branch is detached.
        /// </remarks>
        public string CurrentBranch { get; internal set; }

        /// <summary>
        /// Gets the upstream.
        /// </summary>
        public string Upstream { get; internal set; }

        /// <summary>
        /// Gets the number of stash entries.
        /// </summary>
        public int StashCount { get; internal set; }

        /// <summary>
        /// Gets the list of changed tracked or untracked entries.
        /// </summary>
        public List<StatusItem> Items { get; }
    }
}
