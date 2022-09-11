// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Models;
using GitExecWrapper.Parsers;
using GitExecWrapper.Services;

namespace GitExecWrapper.Commands
{
    public class RemoteCommand
    {
        private readonly string repoPath;
        private readonly IGitRunner runner;
        private readonly IParser<List<string>> listParser;
        private readonly IParser<RemoteItem> itemParser;

        public RemoteCommand(string repoPath)
            : this(repoPath, null, null, null)
        {
        }


        internal RemoteCommand(string repoPath, IGitRunner runner, IParser<List<string>> listParser, IParser<RemoteItem> itemParser)
        {
            this.repoPath = repoPath;
            this.runner = runner ?? new GitRunner();
            this.listParser = listParser ?? new RemoteListParser();
            this.itemParser = itemParser ?? new RemoteDetailParser();
        }


        public async Task<RemoteResult> ExecAsync(CancellationToken cancellationToken = default)
        {
            // The result that will be build up
            var result = new RemoteResult();

            // Get a list of all the remotes
            var remoteList = await runner.ExecAsync(repoPath, "remote", listParser, cancellationToken);

            // Go through each remote, and get the details...
            foreach (var remote in remoteList)
            {
                // Note that "remote show" does query the remote server
                var item = await runner.ExecAsync(repoPath, $"remote show {remote}", itemParser, cancellationToken);

                result.Items.Add(item);
            }

            // Return what has been found
            return result;
        }
    }
}
