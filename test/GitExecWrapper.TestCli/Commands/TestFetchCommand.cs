using System;
using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.TestCli.Options;

namespace GitExecWrapper.TestCli.Commands
{
    public static class TestFetchCommand
    {
        public static async Task RunAsync(TestFetchOptions options, CancellationToken cancellationToken)
        {
            var repoDir = string.IsNullOrEmpty(options.RepoDir) ? Environment.CurrentDirectory : options.RepoDir;
            var results = await Git.FetchAsync(repoDir, cancellationToken);

            // TODO - print fetch results
            Console.WriteLine("Fetch complete.");
        }
    }
}
