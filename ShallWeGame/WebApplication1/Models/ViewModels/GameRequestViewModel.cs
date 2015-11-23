using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class GameRequestViewModel
    {
        [Required]
        public String invites { get; set; }
        [Required]
        public int gameId { get; set; }
        public String timeItBegins { get; set; }
        public String timeItEnds { get; set; }
        public int playersNeeded { get; set; }
    }
}