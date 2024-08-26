using Restic.Commands;
using Restic.Extensions;
using System;
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

        public async Task<T> ExecuteAsync<T>(ResticCommand<T> command, Action<string>? outputHandler = null, Action<string>? errorHandler = null)
        {
            return await ExecuteAsync(command, null, outputHandler, errorHandler);
        }
        
        public async Task<T> ExecuteAsync<T>(ResticCommand<T> command, Repository? repositoryContext, Action<string>? outputHandler = null, Action<string>? errorHandler = null) {
            
            using (Process process = new Process())
            {
                
                process.StartInfo.FileName = "restic";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.EnableRaisingEvents = true; // Allow Exited event to be raised
                /*
                process.OutputDataReceived += (sender, e) =>
                {
                    if(outputHandler != null && !string.IsNullOrWhiteSpace(e.Data))
                    {
                        outputHandler(e.Data);
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (errorHandler != null && !string.IsNullOrWhiteSpace(e.Data))
                    {
                        errorHandler(e.Data);
                    }
                };
                */

                string commandArgs = BuildArgs(command, repositoryContext);
                process.StartInfo.Arguments = commandArgs;

                Console.WriteLine($"Executing: restic {commandArgs}");

                process.Start();
                //process.BeginOutputReadLine();
                //process.BeginErrorReadLine();


                // Async wait for process to complete...
                await process.WaitForExitAsync();

                T result = await command.ParseResult(process);
                process.Dispose();
                return result;
            }
        }


        //TODO: Implement Enviornment based password
        private String BuildArgs<T>(ResticCommand<T> command, Repository? repositoryContext)
        {
            // Build function specific Args
            var commandArgs = command.BuildCommandArgs(_resticClient);

            StringBuilder argsBuilder = new StringBuilder(commandArgs);

            // Final chance to modify arguments
            // Some commands don't require a repository, they will pass null so we make sure we have a valid object.
            // TODO: Add support for other environment variables (RESTIC_PASSWORD_FILE, RESTIC_PASSWORD_COMMAND, etc)
            if (repositoryContext != null)
            {
                if (repositoryContext.PasswordType == PasswordType.Command || repositoryContext.PasswordType == PasswordType.File)
                {
                    string commandPrefix = repositoryContext.PasswordType == PasswordType.File ? "--password-file" : "--password-command";
                    argsBuilder.Append($" {commandPrefix} {repositoryContext.ResticPassword}");
                }

            }

            return argsBuilder.ToString();
        }
    }
}
