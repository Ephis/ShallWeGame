using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Freinds
    {
        public int id { get; set; }
        public Account sender { get; set; }
        public Account reciver { get; set; }

        public Freinds()
        {
            
        }

        public Freinds(Account sender, Account reciver)
        {
            this.sender = sender;
            this.reciver = reciver;
        }


    }
}