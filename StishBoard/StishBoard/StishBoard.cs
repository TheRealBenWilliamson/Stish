using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using StishBoard.Square;

namespace StishBoard
{
    class StishBoard
    {
        Square[,] array = new Square[11,11];

        public StishBoard()
        {
            for (int row = 0; row < 11; row ++)
            {
                for (int col = 0; col < 11; col++)
                {
                    array[row, col] = new Square();
                }
            }
        }

        public Square getSquare(int row, int col)
        {
            return array[row, col];
            
        }
    }
}
