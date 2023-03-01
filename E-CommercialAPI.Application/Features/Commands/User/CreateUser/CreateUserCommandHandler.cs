using E_CommercialAPI.Application.Exceptions;
using E_CommercialAPI.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommercialAPI.Application.Features.Commands.User.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(
                new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = request.Username,
                    Email = request.Email,
                    NameLastname = request.NameSurname
                }, request.Password);

            CreateUserCommandResponse response = new CreateUserCommandResponse() { Succeeded = result.Succeeded, ErrorMessages = new List<string>() };
            foreach (IdentityError item in result.Errors)
            {
                response.ErrorMessages.Add($"{item.Code} - {item.Description}");
            }

            return response;
        }
    }
}
