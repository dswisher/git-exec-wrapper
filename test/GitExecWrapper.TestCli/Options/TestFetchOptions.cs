// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace GitExecWrapper.TestCli.Options
{
    [Verb("fetch", HelpText = "Fetch changes from a remote.")]
    public class TestFetchOptions
    {
        [Option("repo", HelpText = "The directory of the git repository. Default is the current directory.")]
        public string RepoDir { get; set; }

        [Option("dry-run", HelpText = "Show what would be done, without making any changes.")]
        public bool DryRun { get; set; }
    }
}
