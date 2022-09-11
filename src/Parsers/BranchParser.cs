// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    internal class BranchParser
    {
        private static readonly List<PatternHandler<BranchResult>> BranchHandlers = new List<PatternHandler<BranchResult>>();

        static BranchParser()
        {
            var branch = @"[\w\d/_.-]+";
            var prefix = @"^(?<current>.) +(?<name>" + branch + ") +(?<commit>[0-9a-f]+)";
            var suffix = @" +(?<message>.*)$";

            PatternHandler<BranchResult>.AddPatternHandler(BranchHandlers, prefix + @" +\[(?<upstream>" + branch + @")(?<delta>:.*)?\]" + suffix, HandleStuff);
            PatternHandler<BranchResult>.AddPatternHandler(BranchHandlers, prefix + suffix, HandleStuff);
        }


        public BranchResult ParseOutput(string stdout)
        {
            var result = new BranchResult();

            // Parse line-by-line
            var lines = stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var matched = false;
                foreach (var item in BranchHandlers)
                {
                    var match = item.Pattern.Match(line);

                    if (match.Success)
                    {
                        item.Handler(result, match);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    throw new Exception($"Could not parse branch output line: {line}");
                }
            }

            return result;
        }


        public void ThrowError(int code, string stderr)
        {
            // TODO - deduce some common errors, and throw a nice exception
            throw new Exception($"Boom! Branch failed!\nCode:{code}\nStdErr:\n{stderr}");
        }


        private static void HandleStuff(BranchResult result, Match match)
        {
            // Create the core item
            var item = new BranchItem
            {
                IsCurrent = match.Groups["current"].Value == "*",
                BranchName = match.Groups["name"].Value,
                CommitSha = match.Groups["commit"].Value,
                Message = match.Groups["message"].Value
            };

            // Handle special cases
            if (match.Groups["upstream"].Length > 0)
            {
                item.UpstreamBranch = match.Groups["upstream"].Value;
            }

            if (match.Groups["delta"].Length > 0)
            {
                // TODO - pick out ahead and behind counts
            }

            // Add it to the list
            result.Items.Add(item);
        }
    }
}
