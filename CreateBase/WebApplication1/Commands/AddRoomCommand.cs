using MediatR;

namespace ReserveWebApp.Controllers
{
    public class AddRoomCommand : IRequest
    {
        public string Name { get; set; }
    }
}
