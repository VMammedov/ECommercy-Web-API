using E_CommercialAPI.Application.DTOs;
using E_CommercialAPI.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        TokenDto CreateAccess(int minute, AppUser user);
        string CreateRefresh();
        public Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int refreshTokenLifeTime);
    }
}
