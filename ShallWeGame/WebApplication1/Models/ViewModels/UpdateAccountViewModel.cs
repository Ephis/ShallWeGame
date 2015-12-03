using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class UpdateAccountViewModel
    {
        public String id { get; set; }
        public String name { get; set; }
        public String deviceId { get; set; }
    }
}