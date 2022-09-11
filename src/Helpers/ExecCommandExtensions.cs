// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Commands;
using GitExecWrapper.Models;
using GitExecWrapper.Parsers;

namespace GitExecWrapper.Helpers
{
    public static class ExecCommandExtensions
    {
        public static async Task<StatusResult> ExecAsync(this StatusCommand command, CancellationToken cancellationToken = default)
        {
            var exitCode = 0;
            var args = command.ToString();

            var (stdout, stderr) = await SimpleExec.Command.ReadAsync(
                "git",
                args,
                workingDirectory: command.RepoPath,
                handleExitCode: code => (exitCode = code) <= 128,
                cancellationToken: cancellationToken);

            var parser = new StatusParser();

            if (exitCode != 0)
            {
                parser.ThrowError(exitCode, stderr);
            }

            return parser.ParseOutput(stdout);
        }


        public static async Task<FetchResult> ExecAsync(this FetchCommand command, CancellationToken cancellationToken = default)
        {
            var exitCode = 0;
            var args = command.ToString();

            var (stdout, stderr) = await SimpleExec.Command.ReadAsync(
                "git",
                args,
                workingDirectory: command.RepoPath,
                handleExitCode: code => (exitCode = code) <= 128,
                cancellationToken: cancellationToken);

            var parser = new FetchParser();

            if (exitCode != 0)
            {
                parser.ThrowError(exitCode, stderr);
            }

            return parser.ParseOutput(stdout, stderr);
        }


        public static async Task<BranchResult> ExecAsync(this BranchCommand command, CancellationToken cancellationToken = default)
        {
            var exitCode = 0;
            var args = command.ToString();

            var (stdout, stderr) = await SimpleExec.Command.ReadAsync(
                "git",
                args,
                workingDirectory: command.RepoPath,
                handleExitCode: code => (exitCode = code) <= 128,
                cancellationToken: cancellationToken);

            var parser = new BranchParser();

            if (exitCode != 0)
            {
                parser.ThrowError(exitCode, stderr);
            }

            return parser.ParseOutput(stdout);
        }
    }
}
