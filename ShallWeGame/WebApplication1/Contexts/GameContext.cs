using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApplication1.Models;

namespace WebApplication1.Contexts
{
    public class GameContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Account> Accounts { get; set; } 
        public DbSet<FriendRequest> FreindRequests { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameRequest> GameRequests { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Freinds> Freinds { get; set; }

        public GameContext() : base("GameContext")
        {

        }

    }
}