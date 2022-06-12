// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace GitExecWrapper.Commands
{
    public class FetchCommand
    {
        public FetchCommand(string repoPath)
        {
            RepoPath = repoPath;
        }

        public string RepoPath { get; }


        public override string ToString()
        {
            return "fetch --verbose";
        }
    }
}
