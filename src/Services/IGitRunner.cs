// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Parsers;

namespace GitExecWrapper.Services
{
    internal interface IGitRunner
    {
        Task<T> ExecAsync<T>(string repoPath, string args, IParser<T> parser, CancellationToken cancellationToken);
    }
}
