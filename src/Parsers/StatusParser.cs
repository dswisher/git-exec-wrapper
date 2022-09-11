// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    internal class StatusParser
    {
        private static readonly Dictionary<char, FileStatus> CodeMap = new Dictionary<char, FileStatus>();
        private static readonly List<PatternHandler<StatusResult>> BranchHandlers = new List<PatternHandler<StatusResult>>();

        private readonly Regex flagAndPath = new Regex(@"^(?<flag>.) (?<path>.+)$", RegexOptions.Compiled);
        private readonly Regex changedTracked = new Regex(
            @"^1 (?<flag>..) (?<sub>....) (?<mode1>\d{6}) (?<mode2>\d{6}) (?<mode3>\d{6}) (?<name1>\S+) (?<name2>\S+) (?<path>.+)$",
            RegexOptions.Compiled);


        static StatusParser()
        {
            CodeMap.Add('.', FileStatus.Unchanged);
            CodeMap.Add('?', FileStatus.Unknown);
            CodeMap.Add('!', FileStatus.Ignored);
            CodeMap.Add('M', FileStatus.Modified);
            CodeMap.Add('A', FileStatus.Added);
            CodeMap.Add('D', FileStatus.Deleted);

            PatternHandler<StatusResult>.AddPatternHandler(BranchHandlers, @"^# branch.ab \+(?<ahead>\d+) -(?<behind>\d+)$", SetAheadBehind);
            PatternHandler<StatusResult>.AddPatternHandler(BranchHandlers, @"^# branch.oid (?<sha>.+)$", SetCurrentCommit);
            PatternHandler<StatusResult>.AddPatternHandler(BranchHandlers, @"^# branch.head (?<name>.+)$", SetCurrentBranch);
            PatternHandler<StatusResult>.AddPatternHandler(BranchHandlers, @"^# branch.upstream (?<name>.+)$", SetUpstream);
            PatternHandler<StatusResult>.AddPatternHandler(BranchHandlers, @"^# stash (?<num>\d+)$", SetStashed);
        }


        public StatusResult ParseOutput(string stdout)
        {
            var result = new StatusResult();

            foreach (var line in stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                // The first character determines the fate of this line
                Match itemMatch = null;
                switch (line[0])
                {
                    case '#':
                        ParsePoundLine(result, line);
                        continue;

                    case '1':
                        itemMatch = changedTracked.Match(line);
                        break;

                    case '!':
                    case '?':
                        itemMatch = flagAndPath.Match(line);
                        break;

                    // TODO - parse rename/copy/unknown/etc
                }

                // If we have an item match, deal with it
                if (itemMatch != null)
                {
                    if (itemMatch.Success)
                    {
                        var indexFlag = itemMatch.Groups["flag"].Value.ToCharArray().First();
                        var workDirFlag = itemMatch.Groups["flag"].Value.ToCharArray().Last();

                        if (!CodeMap.ContainsKey(indexFlag))
                        {
                            // TODO - improved error handling
                            throw new Exception($"Index flag '{indexFlag}' not handled: {line}");
                        }

                        if (!CodeMap.ContainsKey(workDirFlag))
                        {
                            // TODO - improved error handling
                            throw new Exception($"Work dir flag '{workDirFlag}' not handled: {line}");
                        }

                        var item = new StatusItem
                        {
                            Path = itemMatch.Groups["path"].Value,
                            IndexStatus = CodeMap[indexFlag],
                            WorkDirStatus = CodeMap[workDirFlag]
                        };

                        var code = itemMatch.Groups["flag"].Value;
                        if (code.Length == 2)
                        {
                            item.IndexStatus = CodeMap[code[0]];
                            item.WorkDirStatus = CodeMap[code[1]];
                        }
                        else
                        {
                            item.IndexStatus = FileStatus.Unknown;
                            item.WorkDirStatus = CodeMap[code[0]];
                        }

                        result.Items.Add(item);
                    }
                    else
                    {
                        // TODO - improved error handling
                        throw new Exception($"Could not parse item line: {line}");
                    }
                }
            }

            return result;
        }


        public void ThrowError(int code, string stderr)
        {
            // TODO - deduce some common errors, and throw a nice exception
            throw new Exception($"Boom! Status failed!\nCode:{code}\nStdErr:\n{stderr}");
        }


        private static void SetAheadBehind(StatusResult result, Match match)
        {
            result.CommitsAhead = int.Parse(match.Groups["ahead"].Value);
            result.CommitsBehind = int.Parse(match.Groups["behind"].Value);
        }


        private static void SetCurrentCommit(StatusResult result, Match match)
        {
            var sha = match.Groups["sha"].Value;

            result.CurrentCommit = sha == "(initial)" ? null : sha;
        }


        private static void SetCurrentBranch(StatusResult result, Match match)
        {
            var sha = match.Groups["name"].Value;

            result.CurrentBranch = sha == "(detached)" ? null : sha;
        }


        private static void SetUpstream(StatusResult result, Match match)
        {
            result.Upstream = match.Groups["name"].Value;
        }


        private static void SetStashed(StatusResult result, Match match)
        {
            result.StashCount = int.Parse(match.Groups["num"].Value);
        }


        // TODO - is there a better name for this?
        private void ParsePoundLine(StatusResult result, string line)
        {
            foreach (var item in BranchHandlers)
            {
                var match = item.Pattern.Match(line);

                if (match.Success)
                {
                    item.Handler(result, match);
                    return;
                }
            }

            // TODO - for now, during initial development, throw if we could not decode the line.
            throw new Exception($"Could not handle status pound line: {line}");
        }
    }
}
