using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.IAM.ResetPassword;
using vetcms.SharedModels.Common.Dto;
using vetcms.SharedModels.Common.IAM.Authorization;

namespace vetcms.ClientApplication.Features.IAM.UserList
{
    internal class UserListClientQueryHandler(IMediator mediator) : IRequestHandler<UserListClientQuery, UserListClientQueryResponse>
    {
        static UserListClientQueryHandler()
        {
            GenerateUsers(100);
        }

        public async Task<UserListClientQueryResponse> Handle(UserListClientQuery request, CancellationToken cancellationToken)
        {

            if(request.UserId.HasValue)
            {
                return new UserListClientQueryResponse
                {
                    UserQueryResult = new List<UserDto> { Users.FirstOrDefault(x => x.Id == request.UserId.Value) },
                    ResultCount = 1
                };
            }
            UserListClientQueryResponse response = new UserListClientQueryResponse();
            response.UserQueryResult = Users.Skip(request.Skip.HasValue ? request.Skip.Value : 0).Take(request.Take).ToList();
            response.ResultCount = Users.Count();

            await Task.Delay(5000);
            return response;
        }


        //TODO: Csak tesztelésmiatt van itt, törölni kell
        private static IQueryable<UserDto> Users;
        private static void GenerateUsers(int count)
        {
            var random = new Random();
            var names = new[] { "Jean Martin", "António Langa", "Julie Smith", "Nur Sari", "Jose Hernandez", "Kenji Sato" };
            var peopleList = new List<UserDto>();

            for (int i = 0; i < count; i++)
            {
                string visibleName = names[random.Next(names.Length)];
                var userObject = new UserDto
                {
                    Id = i,
                    Email = $"{visibleName.Replace(" ", ".").ToLower()}@example.com",
                    VisibleName = visibleName,
                    FirstName = visibleName.Split(' ')[0],
                    LastName = visibleName.Split(' ')[1],
                    Address = Guid.NewGuid().ToString(),
                    DateOfBirth = new DateTime(random.Next(1950, 2000), random.Next(1, 12), random.Next(1, 28)),
                };
                userObject.OverwritePermissions(new EntityPermissions().AddFlag(PermissionFlags.CAN_LOGIN, PermissionFlags.CAN_ADD_NEW_USERS));
                peopleList.Add(userObject);
            }

            Users = peopleList.AsQueryable();
        }
    }
}
