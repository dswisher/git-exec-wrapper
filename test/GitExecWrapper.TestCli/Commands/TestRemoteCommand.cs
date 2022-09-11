// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Commands;
using GitExecWrapper.TestCli.Options;

namespace GitExecWrapper.TestCli.Commands
{
    public class TestRemoteCommand
    {
        public static async Task RunAsync(TestRemoteOptions options, CancellationToken cancellationToken)
        {
            var repoDir = string.IsNullOrEmpty(options.RepoDir) ? Environment.CurrentDirectory : options.RepoDir;

            var command = new RemoteCommand(repoDir);

            var result = await command.ExecAsync(cancellationToken);

            Console.WriteLine("Remotes:");

            foreach (var item in result.Items)
            {
                Console.WriteLine("   - {0}", item.Name);
            }
        }
    }
}
