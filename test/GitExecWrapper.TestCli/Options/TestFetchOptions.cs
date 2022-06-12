using CommandLine;

namespace GitExecWrapper.TestCli.Options
{
    [Verb("fetch", HelpText = "Fetch changes from a remote.")]
    public class TestFetchOptions
    {
        [Option("repo", HelpText = "The directory of the git repository. Default is the current directory.")]
        public string RepoDir { get; set; }
    }
}
