// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace GitExecWrapper.Parsers
{
    internal class RemoteListParser : IParser<List<string>>
    {
        public List<string> ParseOutput(string stdout, string stderr)
        {
            var lines = stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            return lines.ToList();
        }


        public void ThrowError(int code, string stderr)
        {
            // TODO - deduce some common errors, and throw a nice exception
            throw new Exception($"Boom! Remote list failed!\nCode:{code}\nStdErr:\n{stderr}");
        }
    }
}
