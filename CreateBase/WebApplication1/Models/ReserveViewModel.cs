using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReserveWebApp.Models
{
    public class ReserveViewModel
    {
        [Display(Name = "Employee")]
        public int SelectedUserId { get; set; }
        public SelectList Users { get; set; }

        [Display(Name = "Office")]
        public int SelectedRoomId { get; set; }
        public SelectList Rooms { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [HiddenInput]
        public int Id { get; set; }
    }
}