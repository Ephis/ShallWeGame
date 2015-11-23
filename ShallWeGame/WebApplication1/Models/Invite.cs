using System;

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
        public int id { get; set; }
        public Account reciver { get; set; }
        public RequestStatus inviteStatus { get; set; }
        public DateTime madeAt { get; set; }

        public Invite()
        {
            madeAt = DateTime.Now;
        }
    }
}