using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ReturnModels
{
    public class GameRequestReturnModel
    {
        public int id { get; set; }
        public String titel { get; set; }
        public Account owner { get; set; }
        public Game gameToPlay { get; set; }
        public List<Invite> invites { get; set; } 
        public Invite usersInvite { get; set; }

        public GameRequestReturnModel()
        {
            
        }
    }
}