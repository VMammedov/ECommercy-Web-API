using E_CommercialAPI.Application.Abstractions.Token;
using E_CommercialAPI.Application.DTOs;
using E_CommercialAPI.Application.Exceptions;
using E_CommercialAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Commands.User.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        readonly UserManager<AppUser> _userManager;
        readonly ITokenHandler _tokenHandler;

        public RefreshTokenLoginCommandHandler(UserManager<AppUser> userManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken);
            if (user is not null)
            {
                TokenDto token = _tokenHandler.CreateAccess(5, user);
                await _tokenHandler.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 2);
                return new RefreshTokenLoginCommandResponse { Token = token };
            }
            else
                throw new NotFoundUserException();
        }
    }
}
