using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Restic.Commands
{
    public class Init : ResticCommand<bool>
    {
        public bool IsJsonResponse => true;

        private Repository _repository;

        public Init(Repository repository)
        {
            _repository = repository;
        }

        public string BuildCommandArgs(ResticClient clientContext)
        {
            return string.Format($"init -r {_repository.RepositoryPath}");
        }

        public async Task<bool> ParseResult(Process process)
        {
            if (process.ExitCode == 0)
                return true;

            if (process.ExitCode == 1)
                return false;

            throw new Exception("Error occurred while trying to initialize repository");
        }
    }
}
