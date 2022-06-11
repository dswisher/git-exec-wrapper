using System.Collections.Generic;

namespace GitExecWrapper.Models
{
    public class StatusResult
    {
        public StatusResult()
        {
            Items = new List<StatusItem>();
        }

        public List<StatusItem> Items { get; }
    }
}
