using Newtonsoft.Json;
using System;

namespace ReserveWebApp.Models
{
    public class DayPilotEventModel
    {
        [JsonProperty(PropertyName = "barColor")]
        public string BarColor { get; set; }
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }
        [JsonProperty(PropertyName = "end")]
        public DateTime End { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
