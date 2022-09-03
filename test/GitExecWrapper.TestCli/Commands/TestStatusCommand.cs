// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.TestCli.Options;

namespace GitExecWrapper.TestCli.Commands
{
    public static class TestStatusCommand
    {
        public static async Task RunAsync(TestStatusOptions options, CancellationToken cancellationToken)
        {
            var repoDir = string.IsNullOrEmpty(options.RepoDir) ? Environment.CurrentDirectory : options.RepoDir;
            var results = await Git.StatusAsync(repoDir, cancellationToken);

            const string format = "   {0,-10} {1,-10} {2}";

            Console.WriteLine(format, "Index", "WorkDir", "Path");
            foreach (var item in results.Items)
            {
                Console.WriteLine(format, item.IndexStatus, item.WorkDirStatus, item.Path);
            }
        }
    }
}
