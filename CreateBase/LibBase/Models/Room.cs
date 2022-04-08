using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace LibBase
{
    [Index("Name", IsUnique = true, Name = "RoomName_Index")]
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator Room(ValueTask<Room> v)
        {
            throw new NotImplementedException();
        }
    }
}
