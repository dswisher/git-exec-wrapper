// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace GitExecWrapper.Commands
{
    public class BranchCommand
    {
        private bool all;

        public BranchCommand(string repoPath)
        {
            RepoPath = repoPath;
        }

        public string RepoPath { get; }

        public BranchCommand All(bool val = true)
        {
            all = val;

            return this;
        }


        public override string ToString()
        {
            var items = new List<string>
            {
                "branch", "--list", "-vv", "--no-color"
            };

            if (all)
            {
                items.Add("--all");
            }

            return string.Join(" ", items);
        }
    }
}
