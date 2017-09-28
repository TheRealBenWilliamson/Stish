using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Program
    {
        static void Main(string[] args)
        {
            StishBoard board = new StishBoard();
            Square s = board.getSquare(1, 1);
            Console.WriteLine("contains: " + s.Con);
            s.Con = "barracks";
            Console.WriteLine("contains: " + s.Con);
        }
    }
}
