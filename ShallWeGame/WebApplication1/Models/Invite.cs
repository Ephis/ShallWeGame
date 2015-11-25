using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
   public enum RequestStatus
    {
        Standby = 0,
        Recived = 1,
        Accepted = 2,
        Rejected = 3
    }


    public class Invite
    {   
        [Key]
        public int id { get; set; }
        public Account reciver { get; set; }
        public GameRequest gameRequest { get; set; }
        public RequestStatus inviteStatus { get; set; }
        public DateTime madeAt { get; set; }
        public int priority { get; set; }

        public Invite()
        {
            madeAt = DateTime.Now;
        }
    }
}