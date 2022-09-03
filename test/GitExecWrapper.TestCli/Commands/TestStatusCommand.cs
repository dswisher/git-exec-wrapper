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
    public static class TestStatusCommand
    {
        public static async Task RunAsync(TestStatusOptions options, CancellationToken cancellationToken)
        {
            var repoDir = string.IsNullOrEmpty(options.RepoDir) ? Environment.CurrentDirectory : options.RepoDir;

            var command = new StatusCommand(repoDir)
                .IncludeIgnored(options.IncludeIgnored);

            var results = await command.ExecAsync(cancellationToken);

            Console.WriteLine("Current Commit: {0}", results.CurrentCommit);
            Console.WriteLine("Current Branch: {0}", results.CurrentBranch);
            Console.WriteLine("Upstream:       {0}", results.Upstream);
            Console.WriteLine("Commits Ahead:  {0}", results.CommitsAhead);
            Console.WriteLine("Commits Behind: {0}", results.CommitsBehind);
            Console.WriteLine();

            const string format = "   {0,-10} {1,-10} {2}";

            Console.WriteLine(format, "Index", "WorkDir", "Path");
            foreach (var item in results.Items)
            {
                Console.WriteLine(format, item.IndexStatus, item.WorkDirStatus, item.Path);
            }
        }
    }
}
