using MediatR;

namespace DomainLayer
{
    public class AddRoomCommand : IRequest
    {
        public string Name { get; set; }
    }
}
