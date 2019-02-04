using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using StishBoard.Square;

namespace StishBoard
{
    public class StishBoard : BoardState
    {
        //creates a reference to the single instance of this singleton object of type StishBoard called "instance".
        private static StishBoard instance;

        private uint m_GameMP = 2;  

        public uint GameMP
        {
            get
            {
                return m_GameMP;
            }
            set
            {
                m_GameMP = value;
            }
        }

        //essentially clones the StishBoard in a BoardState object
        public BoardState GetBoardState()
        {
            return new BoardState(this);
        }

        //default constructor: creates the square objects in "array", assigning each to a position in the BoardSize by BoardSize grid.
        public StishBoard() : base()
        {
            //board size may change
            boardSize.X = 5;
            boardSize.Y = 9;
            m_BoardState = new Square[BoardSizeX, BoardSizeY];
            for (int x = 0; x < BoardSizeX; x ++)
            {
                for (int y = 0; y < BoardSizeY; y++)
                {
                    m_BoardState[x, y] = new Square();
                }
            }
        }

        //creates a public function called "Instance" which ensures there is an existing StishBoard object called "instance" and then returns a reference to the single instance. this is done so that nothing external can interfere with the values of "instance".
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

        //creates a public render method called "Render" to render each of the squares to the console. it calls a Render method on each of the square objects held within 'array'.
        //not sure if this change is right (stolen from the arguments of Render() ) ...   int x, int y    
        public void Render()
        {
            for (int look = 0; look < 2; look++)
            {
                Player LookPlayer;
                if(look == 0)
                {
                    LookPlayer = this.Player1;
                }
                else
                {
                    LookPlayer = this.Player2;
                }
                        

                uint Bcost = 0;
                Coordinate ThisCo = new Coordinate();
                for (uint y = 0; y < BoardSizeY; y++)
                {
                    for (uint x = 0; x < BoardSizeX; x++)
                    {
                        ThisCo.X = x;
                        ThisCo.Y = y;
                        if ((this.getSquare(ThisCo).Dep.DepType == "Barracks" || this.getSquare(ThisCo).Dep.DepType == "Base") && this.getSquare(ThisCo).Dep.OwnedBy == LookPlayer)
                        {
                            Bcost++;
                        }
                    }
                }

                Console.WriteLine("{0} has: {1} Coins. Their next Barracks will cost: {2} Coins", LookPlayer.GetPlayerNum ,LookPlayer.Balance, (Bcost * 3));
            }

            for (int y = 0; y < BoardSizeY; y++)
            {
                for (int x = 0; x < BoardSizeX; x++)
                {
                    m_BoardState[x, y].Render(x, y);
                }
            }

        }




    }
}
