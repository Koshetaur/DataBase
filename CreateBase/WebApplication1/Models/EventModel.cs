using Newtonsoft.Json;
using System;

namespace ReserveWebApp.Models
{
    public class EventModel
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        public string holder { get; set; }
        [JsonProperty(PropertyName = "room")]
        public string Room { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}