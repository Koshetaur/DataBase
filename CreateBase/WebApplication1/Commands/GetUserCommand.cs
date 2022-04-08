using MediatR;

namespace ReserveWebApp.Controllers
{
    public class GetUserCommand : IRequest<UserDto>
    {
        public int Id { get; set; }
    }
}
