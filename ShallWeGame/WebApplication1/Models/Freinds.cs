﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Freinds
    {
        [Key]
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