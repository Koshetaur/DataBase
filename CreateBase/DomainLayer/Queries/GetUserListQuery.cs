using System.Collections.Generic;
using MediatR;

namespace DomainLayer
{
    public class GetUserListQuery : IRequest<List<UserDto>>
    {}
}
