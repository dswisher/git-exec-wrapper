// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace GitExecWrapper.TestCli.Options
{
    [Verb("branch", HelpText = "Get branch info.")]
    public class TestBranchOptions
    {
        [Option("repo", HelpText = "The directory of the git repository. Default is the current directory.")]
        public string RepoDir { get; set; }

        [Option("all", HelpText = "Get info on all branches.")]
        public bool All { get; set; }
    }
}
