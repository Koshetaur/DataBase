using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using LibBase;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            var ResList = new List<string>();
            using (Repository db = new Repository())
            {
                var min = new DateTime(2021, 02, 12, 00, 00, 00);
                var max = new DateTime(2023, 02, 12, 12, 00, 00);
                var connections = db.GetReserveList(min, max);
                foreach (Reserve c in connections)
                {
                    ResList.Add(c.Id + ". " + c.User.Name + " " + c.User.Surname + " reserved " + c.Room.Id + " from " + c.TimeStart + " to " + c.TimeEnd + ".");
                }
            }
            ViewBag.Res = ResList;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
