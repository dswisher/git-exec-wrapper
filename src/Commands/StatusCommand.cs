using System.Collections.Generic;

namespace GitExecWrapper.Commands
{
    public class StatusCommand
    {
        public StatusCommand(string repoPath)
        {
            RepoPath = repoPath;
        }


        public StatusCommand IncludeIgnored(bool ignored = true)
        {
            Ignored = ignored;

            return this;
        }


        public string RepoPath { get; }
        public bool Ignored { get; private set; }


        public override string ToString()
        {
            var items = new List<string>
            {
                "status", "--porcelain=2", "--branch", "--untracked-files=all", "--show-stash"
            };

            if (Ignored)
            {
                items.Add("--ignored");
            }

            return string.Join(" ", items);
        }
    }
}
