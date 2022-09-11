// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace GitExecWrapper.TestCli.Options
{
    [Verb("remote-list", HelpText = "List the remotes for the specified repository.")]
    public class TestRemoteOptions
    {
        [Option("repo", HelpText = "The directory of the git repository. Default is the current directory.")]
        public string RepoDir { get; set; }
    }
}
