// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace GitExecWrapper.Commands
{
    public class StatusCommand
    {
        private bool includeIgnored;

        public StatusCommand(string repoPath)
        {
            RepoPath = repoPath;
        }


        public string RepoPath { get; }


        public StatusCommand IncludeIgnored(bool ignored = true)
        {
            includeIgnored = ignored;

            return this;
        }


        public override string ToString()
        {
            var items = new List<string>
            {
                "status", "--porcelain=2", "--branch", "--untracked-files=all", "--show-stash"
            };

            if (includeIgnored)
            {
                items.Add("--ignored");
            }

            return string.Join(" ", items);
        }
    }
}
