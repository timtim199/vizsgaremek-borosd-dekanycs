using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.ServerApplication.Common.Exceptions;
using vetcms.ServerApplication.Common.IAM;
using vetcms.ServerApplication.Domain.Entity;
using vetcms.ServerApplication.Infrastructure.Presistence.Repository;
using vetcms.SharedModels.Common.ApiLogicExceptionHandling;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.IAM.RegisterUser
{
    internal class AssignUserPermissionCommandHandler(IUserRepository userRepository) : IRequestHandler<AssignUserPermissionApiCommand, AssignUserPermissionApiCommandResponse>
    {
        public async Task<AssignUserPermissionApiCommandResponse> Handle(AssignUserPermissionApiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await userRepository.GetByIdAsync(request.Id);

                // Overwrite permissions for the user
                user.OverwritePermissions(request.GetPermissions());

                // Save changes to the repository
                await userRepository.UpdateAsync(user);

                return new AssignUserPermissionApiCommandResponse(true)
                {
                    Message = "Jogosultság sikeresen delegálva."
                };
            }
            catch (NotFoundException ex)
            {
                return new AssignUserPermissionApiCommandResponse(false)
                {
                    Message = "Nincs ilyen felhasználó."
                };
            }
        }
    }
}
