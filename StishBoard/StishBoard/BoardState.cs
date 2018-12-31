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
        private uint boardSize;
        private Player player1;
        private Player player2;


        public BoardState(StishBoard CurrentBoard)
        {
            //ONLY WORKS WITH HUMANS
            player1 = new Human(CurrentBoard.Player1);
            player2 = new Human(CurrentBoard.Player2);
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

        public BoardState(BoardState CurrentBoard)
        {
            player1 = new Human(CurrentBoard.Player1);
            player2 = new Human(CurrentBoard.Player2);
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

        public Player Player1
        {
            get
            {
                return player1;
            }
            set
            {
                player1 = value;
            }
        }

        public Player Player2
        {
            get
            {
                return player2;
            }
            set
            {
                player2 = value;
            }
        }

        public uint BoardSize
        {
            get
            {
                return boardSize;
            }
            set
            {
                boardSize = value;
            }
        }

        public Square getSquare(Coordinate Find)
        {
            //TO DO: add error handling of arguments that are out of range of the array.
            try
            {
                return m_BoardState[Find.X, Find.Y];
            }
            catch
            {
                return null;
            }

        }

        //barracks no/health, base health, units no/health.  both players

        public uint Counting(String Type, Player ThisPlayer, bool CheckNumber)
        {
            //check number switches the function to wither count the total health of deployment or just the amount of the deployment
            uint counted = 0;
            Coordinate Here = new Coordinate();
            for (uint y = 0; y < this.BoardSize; y++)
            {
                for (uint x = 0; x < this.BoardSize; x++)
                {
                    Here.Y = y;
                    Here.X = x;
                    if((this.getSquare(Here).Dep.OwnedBy == ThisPlayer) && (this.getSquare(Here).Dep.DepType == Type))
                    {
                        if (CheckNumber == true)
                        {
                            counted++;
                        }
                        else
                        {
                            counted += this.getSquare(Here).Dep.Health;
                        }
                        
                    }
                }

            }

            return counted;
        }


    }
}
