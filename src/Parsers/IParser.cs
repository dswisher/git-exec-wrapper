// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace GitExecWrapper.Parsers
{
    internal interface IParser<T>
    {
        T ParseOutput(string stdout, string stderr);
        void ThrowError(int code, string stderr);
    }
}
