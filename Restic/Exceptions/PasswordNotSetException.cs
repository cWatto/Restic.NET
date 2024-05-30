using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Restic.Exceptions
{
    public class PasswordNotSetException : Exception
    {
        public PasswordNotSetException()
        {
        }

        public PasswordNotSetException(string? message) : base(message)
        {
        }

        public PasswordNotSetException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PasswordNotSetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
