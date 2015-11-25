using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Game
    {
        [Key]
        public int id { get; set; }
        public String name { get; set; }

        public Game()
        {
            
        }


    }
}