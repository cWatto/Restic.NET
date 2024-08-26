using Restic.Commands.Results;
using System.IO;


namespace Restic
{
    public class BackupContext
    {
        private Repository _repository;
        private ResticCommunicator _communicator;

        public BackupContext(Repository repository, ResticCommunicator communicator, string backupPath)
        {
            _repository = repository;
            _communicator = communicator;
            BackupPath = backupPath;
        }

        public string BackupPath { get; private set; }
        public List<KeyValuePair<string, string>> Parameters { get; private set; } = new();
        
        #region File Exclusions
        /*
         * File Exclusion Functionality
         * See: https://restic.readthedocs.io/en/stable/040_backup.html#excluding-files
         */
        public BackupContext ExcludeFile(string path)
        {
            AddParameter("--exclude", path);
            return this;
        }

        public BackupContext ExcludeLargerThan(string size) {
            AddParameter("--exclude-larger-than", size);
            return this;
        }
        #endregion

        public async Task<BackupResult> Execute()
        {
            return await _communicator.ExecuteAsync<BackupResult>(new Commands.Backup(_repository, this), _repository);
        }

        private void AddParameter(string key, string value)
        {
            Parameters.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}
