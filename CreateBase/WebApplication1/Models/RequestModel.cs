using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReserveWebApp.Models
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public EventModel Data { get; set; }
    }

    public class RequestApiModel
    {
        public string Id { get; set; }
        public string Action { get; set; }
        public EventModel Data { get; set; }
    }
}
