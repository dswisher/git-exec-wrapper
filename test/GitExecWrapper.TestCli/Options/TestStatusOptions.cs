// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using CommandLine;

namespace GitExecWrapper.TestCli.Options
{
    [Verb("status", HelpText = "Show the working tree status.")]
    public class TestStatusOptions
    {
        [Option("repo", HelpText = "The directory of the git repository. Default is the current directory.")]
        public string RepoDir { get; set; }

        [Option("ignored", HelpText = "Include ignored files in the list.")]
        public bool IncludeIgnored { get; set; }
    }
}
