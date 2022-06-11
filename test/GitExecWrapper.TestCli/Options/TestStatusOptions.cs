using CommandLine;

namespace GitExecWrapper.TestCli.Options
{
    [Verb("status", HelpText = "Show the working tree status")]
    public class TestStatusOptions
    {
        [Option("repo", HelpText = "The directory of the git repository. Default is the current directory.")]
        public string RepoDir { get; set; }
    }
}
