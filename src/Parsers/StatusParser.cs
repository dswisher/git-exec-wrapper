// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    public class StatusParser : IStatusParser
    {
        private static readonly Dictionary<char, FileStatus> CodeMap = new Dictionary<char, FileStatus>();

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
                        // TODO - parse status header lines
                        continue;

                    case '1':
                        itemMatch = changedTracked.Match(line);
                        break;

                    case '!':
                    case '?':
                        itemMatch = flagAndPath.Match(line);
                        break;

                    default:
                        // TODO - parse rename/copy/unknown/etc
                        break;
                }

                // If we have an item match, deal with it
                if (itemMatch != null)
                {
                    if (itemMatch.Success)
                    {
                        var item = new StatusItem
                        {
                            Path = itemMatch.Groups["path"].Value,
                            IndexStatus = CodeMap[itemMatch.Groups["flag"].Value.ToCharArray().First()],
                            WorkDirStatus = CodeMap[itemMatch.Groups["flag"].Value.ToCharArray().Last()]
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
    }
}
