using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class BoardState
    {
        private Square[,] m_BoardState;


        public BoardState(StishBoard CurrentBoard)
        {
            m_BoardState = new Square[CurrentBoard.BoardSize, CurrentBoard.BoardSize];
            Coordinate Here = new Coordinate();
            for (uint y = 0; y < CurrentBoard.BoardSize; y++)
            {
                for (uint x = 0; x < CurrentBoard.BoardSize; x++)
                {
                    Here.Y = y;
                    Here.X = x;
                    m_BoardState[x, y] = new Square(CurrentBoard.getSquare(Here));
                }
                    
            }
            
        }

    }
}
