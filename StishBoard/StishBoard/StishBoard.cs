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
        private static StishBoard instance;

        Square[,] array = new Square[11,11];

        private StishBoard()
        {
            for (int row = 0; row < 11; row ++)
            {
                for (int col = 0; col < 11; col++)
                {
                    array[row, col] = new Square();
                }
            }
        }
        public static StishBoard Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StishBoard();
                }
                return instance;
            }
        }

        public Square getSquare(int row, int col)
        {
            return array[row, col];
            
        }

        public void Render(int x, int y)
        {
            for (y = 0; y < 11; y++)
            {
                for (x = 0; x < 11; x++)
                {
                    array[x, y].Render(x, y);
                }
            }
        }




    }
}
