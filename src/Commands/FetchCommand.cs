// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace GitExecWrapper.Commands
{
    public class FetchCommand
    {
        private bool dryRun;

        public FetchCommand(string repoPath)
        {
            RepoPath = repoPath;
        }

        public string RepoPath { get; }

        public FetchCommand DryRun(bool val = true)
        {
            dryRun = val;

            return this;
        }


        public override string ToString()
        {
            var items = new List<string>
            {
                "fetch", "--verbose"
            };

            if (dryRun)
            {
                items.Add("--dry-run");
            }

            return string.Join(" ", items);
        }
    }
}
