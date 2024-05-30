using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Restic.Exceptions
{
    public class PermissionException : Exception
    {
        public PermissionException()
        {
        }

        public PermissionException(string? message) : base(message)
        {
        }

        public PermissionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PermissionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
