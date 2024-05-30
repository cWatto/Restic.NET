
using Restic.Commands.Results;
using Restic.Exceptions;

namespace Restic
{
    public class Repository
    {
        private readonly ResticClient _client;
        private readonly ResticCommunicator _communicator;
        
        public Repository(ResticClient client, string repositoryPath)
        {
            _client = client;
            _communicator = _client.Communicator;
            RepositoryPath = repositoryPath;

            // Default to environment variables
            WithPasswordInEnvironment();
        }

        public string RepositoryPath { get; private set; }
        
        public PasswordType PasswordType { get; set; } = PasswordType.None;
        
        // ResticPassword changes depending on PasswordType,
        // it's either Empty (Environment, None) or the path to a file/executable (Command, File)
        public string ResticPassword { get; set; } = string.Empty;

        public Repository WithPasswordFile(string passwordFilePath)
        {
            ResticPassword = passwordFilePath;
            PasswordType = PasswordType.File;
            return this;
        }

        public Repository WithPasswordInEnvironment()
        {
            string? environmentPassword = Environment.GetEnvironmentVariable("RESTIC_PASSWORD");
            PasswordType = string.IsNullOrWhiteSpace(environmentPassword) ? PasswordType.None : PasswordType.Environment;
            ResticPassword = string.Empty;
            return this;
        }

        public Repository WithPasswordCommand(string passwordCommandFilePath)
        {
            ResticPassword = passwordCommandFilePath;
            PasswordType = PasswordType.Command;
            return this;
        }

        /// <summary>
        /// Initialize the Restic Repository
        /// </summary>
        /// <returns>true if created, false if already exists</returns>
        public bool Initialize()
        {
            ThrowIfNoPassword();
            return _communicator.Execute<bool>(new Commands.Init(this), this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backupPath">The path to be backed up</param>
        /// <returns>BackupContext</returns>
        public BackupContext Backup(string backupPath)
        {
            ThrowIfNoPassword();
            return new BackupContext(this, _communicator, backupPath);
        }

        private void ThrowIfNoPassword()
        {
            if(PasswordType == PasswordType.None) 
                throw new PasswordNotSetException("Password has not been set, manually specify via the Repository class or set the RESTIC_PASSWORD environment variable.");
        }
    }

    public enum PasswordType
    {
        None,
        File,
        Command,
        Environment
    }
}
