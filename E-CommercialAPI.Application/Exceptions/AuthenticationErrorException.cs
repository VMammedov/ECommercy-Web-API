using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Exceptions
{
    public class AuthenticationErrorException : Exception
    {
        public AuthenticationErrorException() : base("Username or password is not valid...")
        {
        }

        public AuthenticationErrorException(string? message) : base(message)
        {
        }
    }
}
