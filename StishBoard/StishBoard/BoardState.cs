using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class BoardState
    {
        protected Square[,] m_BoardState;
        //boardsize gives the very bottom right point.
        protected Coordinate boardSize = new Coordinate();
        protected Player player1;
        protected Player player2;

        protected BoardState()
        {

        }

        //StishBoard
        public BoardState(StishBoard CurrentBoard)
        {
            //ONLY WORKS WITH HUMANS
            this.boardSize.X = CurrentBoard.BoardSizeX;
            this.boardSize.Y = CurrentBoard.BoardSizeY;
            this.player1 = new Human(CurrentBoard.Player1);
            this.player2 = new Human(CurrentBoard.Player2);
            m_BoardState = new Square[CurrentBoard.BoardSizeX, CurrentBoard.BoardSizeY];
            Coordinate Here = new Coordinate();
            for (uint y = 0; y < CurrentBoard.BoardSizeY; y++)
            {
                for (uint x = 0; x < CurrentBoard.BoardSizeX; x++)
                {
                    Here.Y = y;
                    Here.X = x;
                    m_BoardState[x, y] = new Square(CurrentBoard.getSquare(Here), (Human)this.player1, (Human)this.player2);
                }
                    
            }
            
        }

        //BoardState
        public BoardState(BoardState CurrentBoard)
        {
            //ONLY WORKS WITH HUMANS
            this.boardSize.X = CurrentBoard.BoardSizeX;
            this.boardSize.Y = CurrentBoard.BoardSizeY;
            this.player1 = new Human(CurrentBoard.Player1);
            this.player2 = new Human(CurrentBoard.Player2);
            m_BoardState = new Square[CurrentBoard.BoardSizeX, CurrentBoard.BoardSizeY];
            Coordinate Here = new Coordinate();
            for (uint y = 0; y < CurrentBoard.BoardSizeY; y++)
            {
                for (uint x = 0; x < CurrentBoard.BoardSizeX; x++)
                {
                    Here.Y = y;
                    Here.X = x;
                    m_BoardState[x, y] = new Square(CurrentBoard.getSquare(Here), (Human)this.player1, (Human)this.player2);
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

        public uint BoardSizeX
        {
            get
            {
                return boardSize.X;
            }
            set
            {
                boardSize.X = value;
            }
        }

        public uint BoardSizeY
        {
            get
            {
                return boardSize.Y;
            }
            set
            {
                boardSize.Y = value;
            }
        }

        //creates a public function called "getSquare", it will return the reference to the square object that sits in the position in "array" that is asked for in the arguments.
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
            for (uint y = 0; y < this.BoardSizeY; y++)
            {
                for (uint x = 0; x < this.BoardSizeX; x++)
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
