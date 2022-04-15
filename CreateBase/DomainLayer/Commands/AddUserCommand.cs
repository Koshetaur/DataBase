using MediatR;

namespace DomainLayer
{
    public class AddUserCommand: IRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
