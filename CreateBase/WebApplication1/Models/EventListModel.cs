using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReserveWebApp.Models
{
    public class EventListModel
    {
        public List<EventModel> Data { get; set; }
        public Collections Collections { get; set; }
    }
    public class Pair
    {
        public int Value { get; set; }
        public string Label { get; set; }
    }
    public class Collections
    {
        public List<Pair> Rooms { get; set; }
    }
}
