using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vetcms.ClientApplication.Features.IAM.ResetPassword;
using vetcms.SharedModels.Common.Dto;

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
            UserListClientQueryResponse response = new UserListClientQueryResponse();
            response.UserQueryResult = Users.Skip(request.Skip).Take(request.Take).ToList();
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

                peopleList.Add(new UserDto
                {
                    Id = i,
                    Email = $"{visibleName.Replace(" ", ".").ToLower()}@example.com",
                    VisibleName = visibleName,
                    FirstName = visibleName.Split(' ')[0],
                    LastName = visibleName.Split(' ')[1],
                    Address = Guid.NewGuid().ToString(),
                    DateOfBirth = new DateTime(random.Next(1950, 2000), random.Next(1, 12), random.Next(1, 28)),
                });
            }

            Users = peopleList.AsQueryable();
        }
    }
}
