using System;

namespace FTTW.Exceptions
{
    class MissingConfigFileException : Exception
    {
        public MissingConfigFileException() : base("Não foi possível encontrar o arquivo de configuração.")
        { }
    }
}
