using E_CommercialAPI.Application.Abstractions.Token;
using E_CommercialAPI.Application.DTOs;
using E_CommercialAPI.Application.Exceptions;
using E_CommercialAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Commands.User.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if (user is null)
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);

            if(user is null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if(result.Succeeded) // authentication is succeeded
            {
                TokenDto token = _tokenHandler.CreateAccess(5, user);
                await _tokenHandler.UpdateRefreshTokenAsync(token.RefreshToken, user, token.Expiration, 2);
                return new LoginUserCommandResponse()
                {
                    Token = token
                };
                //.... authorization
            }

            throw new AuthenticationErrorException();
        }
    }
}
