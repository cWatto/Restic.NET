using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Restic.Exceptions
{
    public class RepositoryDoesNotExistException : Exception
    {
        public RepositoryDoesNotExistException()
        {
        }

        public RepositoryDoesNotExistException(string? message) : base(message)
        {
        }

        public RepositoryDoesNotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected RepositoryDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
