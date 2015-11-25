using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class GameRequest
    {
        public int id { get; set; }
        public String titel { get; set; }
        public Account owner { get; set; }
        public virtual ICollection<Invite> invites{get; set;} 
        public Game gameToPlay { get; set; }
        public DateTime requestCreatedAt { get; set; }
        public DateTime timeItBegins { get; set; }
        public DateTime timeItEnds { get; set; }
        public int playersNeeded { get; set; }

        public GameRequest()
        {
            requestCreatedAt = DateTime.Now;
        }
    }
}