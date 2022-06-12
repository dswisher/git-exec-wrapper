// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    public class FetchParser : IFetchParser
    {
        public FetchResult ParseOutput(string stdout)
        {
            // TODO - parse fetch output
            return new FetchResult();
        }


        // TODO - looks like some info is written to stderr, even for a successful fetch
#if false
Exit Code: 0
--- stdout ---

--- stderr ---
From github.com:Example/fun-repo
 * [new branch]        feature/cool-stuff -> origin/feature/cool-stuff
#endif

#if false
Exit Code: 0
--- stdout ---

--- stderr ---
From github.com:Example/fun-repo
 = [up to date]        develop                -> origin/develop
 = [up to date]        master                 -> origin/master
#endif

#if false
Exit Code: 0
--- stdout ---

--- stderr ---
From github.com:Example/fun-repo
   7b2d11b5..8f5acfa8  develop                 -> origin/develop
 = [up to date]        bugfix/stuff-is-really-broken -> origin/bugfix/stuff-is-really-broken
 = [up to date]        master                  -> origin/master
   326a8817..215b5531  release                 -> origin/release
#endif

        public void ThrowError(int code, string stderr)
        {
            // TODO - deduce some common errors, and throw a nice exception
            throw new Exception($"Boom! Fetch failed!\nCode:{code}\nStdErr:\n{stderr}");
        }
    }
}
