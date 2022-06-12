using System;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Threading.Tasks;
using CommandLine;
using GitExecWrapper.TestCli.Commands;
using GitExecWrapper.TestCli.Options;

namespace GitExecWrapper.TestCli
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var types = LoadVerbs();

            // TODO - need a cancellation token
            CancellationToken foo = default;

            try
            {
                await Parser.Default.ParseArguments(args, types)
                    .WithParsedAsync(x => RunAsync(x, foo));

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }


        private static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();
        }


        private static async Task RunAsync(object obj, CancellationToken cancellationToken)
        {
            switch (obj)
            {
                case TestStatusOptions s:
                    await TestStatusCommand.RunAsync(s, cancellationToken);
                    break;

                case TestFetchOptions f:
                    await TestFetchCommand.RunAsync(f, cancellationToken);
                    break;

                default:
                    Console.WriteLine("Options not handled: {0}", obj.GetType().Name);
                    break;
            }
        }
    }
}
