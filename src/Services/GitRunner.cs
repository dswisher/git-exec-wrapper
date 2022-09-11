// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Parsers;

namespace GitExecWrapper.Services
{
    internal class GitRunner : IGitRunner
    {
        public async Task<T> ExecAsync<T>(string repoPath, string args, IParser<T> parser, CancellationToken cancellationToken)
        {
            var exitCode = 0;

            var (stdout, stderr) = await SimpleExec.Command.ReadAsync(
                "git",
                args,
                workingDirectory: repoPath,
                handleExitCode: code => (exitCode = code) <= 128,
                cancellationToken: cancellationToken);

            if (exitCode != 0)
            {
                parser.ThrowError(exitCode, stderr);
            }

            return parser.ParseOutput(stdout, stderr);
        }
    }
}
