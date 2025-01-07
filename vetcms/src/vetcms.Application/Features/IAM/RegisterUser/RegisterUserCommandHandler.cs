using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Common.ApiLogicExceptionHandling;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.IAM.RegisterUser
{
    internal class RegisterUserCommandHandler(IUserRepository userRepository) : IRequestHandler<RegisterUserApiCommand, RegisterUserApiCommandResponse>
    {
        public async Task<RegisterUserApiCommandResponse> Handle(RegisterUserApiCommand request, CancellationToken cancellationToken)
        {
            User newUser = new User();
            newUser.PhoneNumber = request.PhoneNumber;
            newUser.Email = request.Email;
            newUser.Password = PasswordUtility.CreateUserPassword(newUser, request.Password);
            newUser.VisibleName = request.Name;

            if(userRepository.GetByEmail(newUser.Email) != null)
            {
                return new RegisterUserApiCommandResponse()
                {
                    Success = false,
                    Message = "Az E-mail cím már foglalt."
                };
            }

            await userRepository.AddAsync(newUser);

            return new RegisterUserApiCommandResponse(true);

        }
    }
}
