using System;

namespace FTTW.Exceptions
{
    class InvalidLoginPasswordCombinationException : Exception
    {
        public InvalidLoginPasswordCombinationException() : base("Você inseriu uma combinação inválida de login e senha.")
        {}       
    }
}
