﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class IdViewModel
    {
        [Required]
        public int id { get; set; }
    }
}