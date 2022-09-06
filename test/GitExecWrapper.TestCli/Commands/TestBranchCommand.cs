// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Commands;
using GitExecWrapper.Helpers;
using GitExecWrapper.TestCli.Options;

namespace GitExecWrapper.TestCli.Commands
{
    public static class TestBranchCommand
    {
        public static async Task RunAsync(TestBranchOptions options, CancellationToken cancellationToken)
        {
            var repoDir = string.IsNullOrEmpty(options.RepoDir) ? Environment.CurrentDirectory : options.RepoDir;

            var command = new BranchCommand(repoDir)
                .All(options.All);

            var branchResult = await command.ExecAsync(cancellationToken);

            foreach (var item in branchResult.Items)
            {
                Console.WriteLine(
                    "{0} {1} {2}",
                    item.IsCurrent ? "*" : " ",
                    item.BranchName,
                    item.UpstreamBranch);
            }
        }
    }
}
