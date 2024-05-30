using Restic.Exceptions;
using System.Diagnostics;
namespace Restic.Extensions
{
    public static class ResticProcessExtensions
    {
        private static Dictionary<string, Type> EXCEPTION_MAP = new()
        {
            { "permission denied", typeof(PermissionException) },
            { "all target directories/files do not exist", typeof(RepositoryDoesNotExistException) }
        };

        public static void ThrowIfRequired(this Process process)
        {
            if (process.ExitCode > 0)
            {
                // Handle some of the top-level errors
                var errorOutput = process.StandardError.ReadToEnd();

                Type? exceptionType = EXCEPTION_MAP
                    .FirstOrDefault(e => errorOutput.Contains(e.Key)).Value;

                if (exceptionType == null)
                    return;
               
                var exceptionInstance = Activator.CreateInstance(exceptionType, errorOutput) as Exception;
                if (exceptionInstance != null)
                {
                    throw exceptionInstance;
                }
            }

        }
    }
}
