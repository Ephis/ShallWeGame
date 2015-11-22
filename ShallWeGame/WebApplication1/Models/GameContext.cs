using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication1.Models
{
    public class GameContext : IdentityDbContext<ApplicationUser>
    {

        public GameContext() : base("GameContext")
        {
            
        }



    }
}