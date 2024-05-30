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

        public string ParseResult(Process process)
        {
            return process.StandardOutput.ReadToEnd();
        }

        
    }
}
