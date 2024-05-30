using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restic.Commands
{
    public interface ResticCommand<T>
    {
        bool IsJsonResponse { get; }
        string BuildCommandArgs(ResticClient clientContext);
        public T ParseResult(Process process);
    }
}
