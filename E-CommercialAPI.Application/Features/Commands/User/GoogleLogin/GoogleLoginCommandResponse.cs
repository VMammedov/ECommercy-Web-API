using E_CommercialAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Commands.User.GoogleLogin
{
    public class GoogleLoginCommandResponse
    {
        public TokenDto Token { get; set; }
    }
}
