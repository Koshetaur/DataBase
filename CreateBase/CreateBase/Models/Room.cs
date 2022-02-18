using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CreateBase
{
    [Index("Name", IsUnique = true, Name = "RoomName_Index")]
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
