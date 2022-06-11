using System.Threading;
using System.Threading.Tasks;
using GitExecWrapper.Commands;
using GitExecWrapper.Models;
using GitExecWrapper.Parsers;

namespace GitExecWrapper.Helpers
{
    public static class ExecCommandExtensions
    {
        public static async Task<StatusResult> ExecAsync(this StatusCommand command, CancellationToken cancellationToken)
        {
            var exitCode = 0;
            var args = command.ToString();

            var (stdout, stderr) = await SimpleExec.Command.ReadAsync("git", args,
                workingDirectory: command.RepoPath,
                handleExitCode: code => (exitCode = code) <= 128,
                cancellationToken: cancellationToken);

            DebugHelpers.Dump(exitCode, stdout, stderr);    // TODO - remove this

            var parser = new StatusParser();

            if (exitCode != 0)
            {
                parser.ThrowError(exitCode, stderr);
            }

            return parser.ParseOutput(stdout);
        }
    }
}
