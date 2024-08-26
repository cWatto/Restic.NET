using Restic.Commands.Results;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

namespace Restic.Commands
{
    public class Backup : ResticCommand<BackupResult>
    {
        public Backup(Repository repository, BackupContext backupContext)
        {
            _repository = repository;
            _backupContext = backupContext;
        }
        private Repository _repository;
        private BackupContext _backupContext;

        public bool IsJsonResponse => false;

        public string BuildCommandArgs(ResticClient clientContext)
        {
            StringBuilder commandBuilder = new StringBuilder($"-r {_repository.RepositoryPath} backup");

            if (_backupContext.Parameters.Any())
            {
                foreach (var parameter in _backupContext.Parameters)
                {
                    commandBuilder.Append($" {parameter.Key} {parameter.Value}");
                }
            }
            
            commandBuilder.Append($" {_backupContext.BackupPath}");

            return commandBuilder.ToString();
        }

        public async Task<BackupResult> ParseResult(Process process)
        {
            bool success = process.ExitCode == 0;
            string message = success ? await process.StandardOutput.ReadToEndAsync() : await process.StandardError.ReadToEndAsync();
            return new BackupResult(success, message);
        }

        
    }
}
