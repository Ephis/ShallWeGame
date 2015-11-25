using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class InviteRequest
    {
        [Key]
        public int id { get; set; }
        public Invite invite { get; set; }
        public GameRequest gameRequest { get; set; }

        public InviteRequest()
        {
            
        }


    }
}