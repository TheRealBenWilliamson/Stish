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
            StishBoard board = StishBoard.Instance;

            Square s1 = board.getSquare(1, 1);
            s1.Dep = new Barracks();            
            Console.WriteLine("contains: " + s1.DepType);

            Square s2 = board.getSquare(2, 2); 
            s2.Dep = new Unit();                     
            Console.WriteLine("contains: " + s2.DepType);

            Square s3 = board.getSquare(3, 3);
            Console.WriteLine("contains: " + s3.DepType);
        }
    }
}
