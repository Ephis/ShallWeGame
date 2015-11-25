using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class FriendRequest
    {
        [Key]
        public int id { get; set; }
        public Account sender { get; set; }
        public Account reciver { get; set; }
        public RequestStatus requestStatus { get; set; }
        public DateTime requestMadeAt { get; set; }

        public FriendRequest()
        {
            requestMadeAt = DateTime.Now;
        }
    }
}       