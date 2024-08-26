using System.Diagnostics;

namespace Restic.Commands
{
    public class GetVersion : ResticCommand<string>
    {
        public bool IsJsonResponse => false;

        public string BuildCommandArgs(ResticClient clientContext)
        {
            return "version";
        }

        public async Task<string> ParseResult(Process process)
        {
            return await process.StandardOutput.ReadToEndAsync();
        }

        
    }
}
