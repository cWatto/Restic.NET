using Restic.Commands;
using Restic.Extensions;
using System.Diagnostics;
using System.Text;

namespace Restic
{
    public class ResticCommunicator
    {
        private readonly ResticClient _resticClient;
        public ResticCommunicator(ResticClient resticClient) {
            _resticClient = resticClient;
        }
        
        public T Execute<T>(ResticCommand<T> command, Repository repositoryContext)
        {
            var commandArgs = command.BuildCommandArgs(_resticClient);
            Process process = BuildAndExecuteProcess(commandArgs, repositoryContext);
            process.WaitForExit();
            process.ThrowIfRequired();
            Console.WriteLine($"Result: {process.StandardError.ReadToEnd()}");
            return command.ParseResult(process);
        }

        private Process BuildAndExecuteProcess(string arguments, Repository repositoryContext)
        {
            Process process = new Process();
            process.StartInfo.FileName = "restic";
            
            StringBuilder argsBuilder = new StringBuilder(arguments);

            // Final chance to modify arguments
            // Some commands don't require a repository, they will pass null so we make sure we have a valid object.
            // TODO: Add support for other environment variables (RESTIC_PASSWORD_FILE, RESTIC_PASSWORD_COMMAND, etc)
            if (repositoryContext != null) {
                switch (repositoryContext.PasswordType)
                {
                    case PasswordType.File:
                    case PasswordType.Command:
                        string commandPrefix = repositoryContext.PasswordType == PasswordType.File ? "--password-file" : "--password-command";
                        argsBuilder.Append($" {commandPrefix} {repositoryContext.ResticPassword}");
                        break;

                    case PasswordType.Environment:
                        process.StartInfo.Environment["RESTIC_PASSWORD"] = Environment.GetEnvironmentVariable("RESTIC_PASSWORD");
                        break;
                }
            }

            process.StartInfo.Arguments = argsBuilder.ToString();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            Console.WriteLine("Final Command: {0} {1}", process.StartInfo.FileName, argsBuilder.ToString());

            process.Start();
            return process;
        }

        
    }
}
