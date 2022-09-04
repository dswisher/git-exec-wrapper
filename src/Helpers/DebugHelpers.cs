// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace GitExecWrapper.Helpers
{
    internal static class DebugHelpers
    {
        internal static void Dump(int exitCode, string stdout, string stderr)
        {
            Console.WriteLine("Exit Code: {0}", exitCode);
            Console.WriteLine("--- stdout ---");
            Console.WriteLine(stdout);
            Console.WriteLine("--- stderr ---");
            Console.WriteLine(stderr);
        }
    }
}
