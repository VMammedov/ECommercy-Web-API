using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Exceptions
{
    public class UserCreatedException : Exception
    {
        public UserCreatedException() : base("Unexpected error!")
        {
        }

        public UserCreatedException(string? message) : base(message)
        {
        }

        public UserCreatedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
