// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    internal class FetchParser
    {
        private static readonly Dictionary<string, RefStatus> Flags = new Dictionary<string, RefStatus>();
        private static readonly List<PatternHandler<FetchResult>> BranchHandlers = new List<PatternHandler<FetchResult>>();

        static FetchParser()
        {
            Flags.Add(" ", RefStatus.Fetched);
            Flags.Add("+", RefStatus.ForcedUpdate);
            Flags.Add("-", RefStatus.Pruned);
            Flags.Add("t", RefStatus.TagUpdate);
            Flags.Add("*", RefStatus.NewRef);
            Flags.Add("!", RefStatus.Failed);
            Flags.Add("=", RefStatus.UpToDate);

            PatternHandler<FetchResult>.AddPatternHandler(BranchHandlers, @"^From (?<from>.*)$", HandleFromLine);
            PatternHandler<FetchResult>.AddPatternHandler(BranchHandlers, @"^POST git-upload-pack \(.*\)$", IgnoreLine);
            PatternHandler<FetchResult>.AddPatternHandler(BranchHandlers, @"^Auto packing the repository in background for optimum performance.$", IgnoreLine);
            PatternHandler<FetchResult>.AddPatternHandler(BranchHandlers, @"^See ""git help gc"" for manual housekeeping.$", IgnoreLine);
            PatternHandler<FetchResult>.AddPatternHandler(BranchHandlers, @"^ (?<flag>.) +(?<summary>\[[a-z ]+\]) +(?<from>.+) +-> +(?<to>.+)$", HandleBranchLine);
            PatternHandler<FetchResult>.AddPatternHandler(BranchHandlers, @"^ (?<flag>.) +(?<summary>[a-z0-9.]+) +(?<from>.+) -> (?<to>.+)$", HandleBranchLine);
        }


        public FetchResult ParseOutput(string stdout, string stderr)
        {
            var result = new FetchResult();

            // Parse line-by-line
            var lines = stderr.Split('\n', StringSplitOptions.RemoveEmptyEntries);
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
                    throw new Exception($"Could not parse fetch output line: {line}");
                }
            }

            return result;
        }


        public void ThrowError(int code, string stderr)
        {
            // TODO - deduce some common errors, and throw a nice exception
            throw new Exception($"Boom! Fetch failed!\nCode:{code}\nStdErr:\n{stderr}");
        }


        private static void IgnoreLine(FetchResult result, Match match)
        {
            // nothing to do
        }


        private static void HandleFromLine(FetchResult result, Match match)
        {
            result.FromRepo = match.Groups["from"].Value;
        }


        private static void HandleBranchLine(FetchResult result, Match match)
        {
            var item = new FetchItem
            {
                Status = Flags[match.Groups["flag"].Value],
                Summary = match.Groups["summary"].Value,
                From = match.Groups["from"].Value.Trim(),
                To = match.Groups["to"].Value.Trim()
            };

            result.Items.Add(item);
        }
    }
}
