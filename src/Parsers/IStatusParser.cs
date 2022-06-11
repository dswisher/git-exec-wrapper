using GitExecWrapper.Models;

namespace GitExecWrapper.Parsers
{
    public interface IStatusParser
    {
        StatusResult ParseOutput(string stdout);
    }
}
