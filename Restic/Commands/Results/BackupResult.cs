using System;
using System.Collections.Generic;namespace Restic.Commands.Results
{
    public class BackupResult
    {
        public BackupResult(bool success, string message)
        {
            Success = success;

            if (success) 
                Output = message; 
            else 
                ErrorOutput = message;
            
        }
        public bool Success { get; private set; }

        public string ErrorOutput { get; private set; }

        public string Output { get; private set; }
    }
}
