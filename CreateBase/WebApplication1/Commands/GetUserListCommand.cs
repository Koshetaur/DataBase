using System.Collections.Generic;
using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetUserListCommand : IRequest<List<UserDto>>
    {}
}
