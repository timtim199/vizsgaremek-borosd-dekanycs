using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ServerApplication.Common.Abstractions.Data;
using vetcms.SharedModels.Features.IAM;

namespace vetcms.ServerApplication.Features.IAM.DeleteUser
{
    internal class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserApiCommand, DeleteUserApiCommandResponse>
    {
        public Task<DeleteUserApiCommandResponse> Handle(DeleteUserApiCommand request, CancellationToken cancellationToken)
        {
            List<int> nonExistentIds = new();
            request.Ids.ForEach(async id =>
            {
                if (!await userRepository.ExistAsync(id))
                {
                    nonExistentIds.Add(id);
                }
            });

            if (nonExistentIds.Any())
            {
                return Task.FromResult(new DeleteUserApiCommandResponse()
                {
                    Success = false,
                    Message = $"Nem létező felhasználó ID(s): {string.Join(",", nonExistentIds)}"
                });
            }


            request.Ids.ForEach(async id =>
            {
                await userRepository.DeleteAsync(id);
            });


            return Task.FromResult(new DeleteUserApiCommandResponse()
            {
                Success = true
            });


        }
    }
}
