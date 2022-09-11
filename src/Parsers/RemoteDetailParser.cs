// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    internal class RemoteDetailParser : IParser<RemoteItem>
    {
        private static readonly List<PatternHandler<RemoteItem>> LineHandlers = new List<PatternHandler<RemoteItem>>();

        static RemoteDetailParser()
        {
            PatternHandler<RemoteItem>.AddPatternHandler(LineHandlers, @"^\* remote (?<name>.*)$", HandleFirstLine);
            PatternHandler<RemoteItem>.AddPatternHandler(LineHandlers, @"^  HEAD branch: (?<name>.*)$", HandleHeadBranch);

            // TODO - there is lots of additional, useful info that should be parsed out!
        }


        public RemoteItem ParseOutput(string stdout, string stderr)
        {
            var result = new RemoteItem();

            // Parse line-by-line
            var lines = stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                foreach (var item in LineHandlers)
                {
                    var match = item.Pattern.Match(line);

                    if (match.Success)
                    {
                        item.Handler(result, match);
                        break;
                    }
                }
            }

            return result;
        }


        public void ThrowError(int code, string stderr)
        {
            // TODO - deduce some common errors, and throw a nice exception
            throw new Exception($"Boom! Remote details failed!\nCode:{code}\nStdErr:\n{stderr}");
        }


        private static void HandleFirstLine(RemoteItem item, Match match)
        {
            item.Name = match.Groups["name"].Value;
        }


        private static void HandleHeadBranch(RemoteItem item, Match match)
        {
            item.HeadBranch = match.Groups["name"].Value;
        }
    }
}
