using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Account
    {
        [Key]
        public int id { get; set; }
        public String name { get; set; }
        public String userId { get; set; }
        public String deviceId { get; set; }
        public Boolean isUsersAccount { get; set; }

        public Account()
        {
            
        }

    }
}