using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Friends
    {
        [Key]
        public int id { get; set; }
        public Account sender { get; set; }
        public Account reciver { get; set; }

        public Friends()
        {
            
        }

        public Friends(Account sender, Account reciver)
        {
            this.sender = sender;
            this.reciver = reciver;
        }


    }
}