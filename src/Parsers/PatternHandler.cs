// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GitExecWrapper.Parsers
{
    internal class PatternHandler<TResult>
    {
        public Regex Pattern { get; set; }
        public Action<TResult, Match> Handler { get; set; }


        public static void AddPatternHandler(List<PatternHandler<TResult>> list, string pattern, Action<TResult, Match> handler)
        {
            list.Add(new PatternHandler<TResult>
            {
                Pattern = new Regex(pattern, RegexOptions.Compiled),
                Handler = handler
            });
        }
    }
}
