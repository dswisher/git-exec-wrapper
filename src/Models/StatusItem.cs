namespace GitExecWrapper.Models
{
    public class StatusItem
    {
        public FileStatus IndexStatus { get; set; }
        public FileStatus WorkDirStatus { get; set; }
        public string Path { get; set; }
    }
}
