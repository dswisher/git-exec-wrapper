using System;

namespace GitExecWrapper.Helpers
{
    public static class DebugHelpers
    {
        public static void Dump(int exitCode, string stdout, string stderr)
        {
            Console.WriteLine("Exit Code: {0}", exitCode);
            Console.WriteLine("--- stdout ---");
            Console.WriteLine(stdout);
            Console.WriteLine("--- stderr ---");
            Console.WriteLine(stderr);
        }
    }
}
