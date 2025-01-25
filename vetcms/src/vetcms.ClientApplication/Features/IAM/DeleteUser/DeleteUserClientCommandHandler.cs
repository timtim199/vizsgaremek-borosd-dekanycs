using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.IAM.RegisterUser;
using vetcms.ClientApplication.Features.IAM.UserList;

namespace vetcms.ClientApplication.Features.IAM.DeleteUser
{
    internal class DeleteUserClientCommandHandler : IRequestHandler<DeleteUserClientCommand, bool>
    {
        public async Task<bool> Handle(DeleteUserClientCommand request, CancellationToken cancellationToken)
        {
            request.UserIds.ForEach(userId =>
            {
                var list = UserListClientQueryHandler.Users.ToList();
                list.RemoveAll(x => x.Id == userId);
                UserListClientQueryHandler.Users = list.AsQueryable();
            });

            await Task.Delay(1000);

            return true;
        }
    }
}
