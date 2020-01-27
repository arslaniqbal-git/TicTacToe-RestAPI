using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicTacToe.Models
{
    public class Move
    {
        public string gameId { get; set; }
        public int row { get; set; }
        public int col { get; set; }
    }
}