// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Commands;
using GitExecWrapper.Helpers;
using GitExecWrapper.Models;

namespace GitExecWrapper
{
    public static class Git
    {
        public static async Task<StatusResult> StatusAsync(string repoPath, CancellationToken cancellationToken)
        {
            var command = new StatusCommand(repoPath);

            return await command.ExecAsync(cancellationToken);
        }
    }
}
