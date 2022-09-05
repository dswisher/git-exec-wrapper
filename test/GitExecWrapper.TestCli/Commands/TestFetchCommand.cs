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
    public static class TestFetchCommand
    {
        public static async Task RunAsync(TestFetchOptions options, CancellationToken cancellationToken)
        {
            var repoDir = string.IsNullOrEmpty(options.RepoDir) ? Environment.CurrentDirectory : options.RepoDir;

            var command = new FetchCommand(repoDir)
                .DryRun(options.DryRun);

            var fetchResult = await command.ExecAsync(cancellationToken);

            Console.WriteLine("From Repo: {0}", fetchResult.FromRepo);

            Console.WriteLine();
            Console.WriteLine("Items:");

            foreach (var item in fetchResult.Items)
            {
                Console.WriteLine("    {0} {1} {2} -> {3}", item.Status, item.Summary, item.From, item.To);
            }
        }
    }
}
