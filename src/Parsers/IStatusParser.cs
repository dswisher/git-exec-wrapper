// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    public interface IStatusParser
    {
        StatusResult ParseOutput(string stdout);
    }
}
