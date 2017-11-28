using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTTW.Exceptions
{
    class RedmineServerInaccessibleException : Exception
    {
        public RedmineServerInaccessibleException() : base("Não foi possível conectar-se ao Redmine.")
        {}
    }
}
