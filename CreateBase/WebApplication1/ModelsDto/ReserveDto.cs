using System;

namespace ReserveWebApp.Controllers
{
    public class ReserveDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public RoomDto Room { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
    }
}
