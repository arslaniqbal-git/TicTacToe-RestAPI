using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{
    public class Response
    {
        public Move GameMove { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsGameOver { get; set; }
    }
}