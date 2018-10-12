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
        //creates a reference to the single instance of this singleton object of type StishBoard called "instance".
        private static StishBoard instance;

        private Player player1;
        private Player player2;

        private uint boardSize;

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

        //creates an array called "array" capable of holding square objects in the orientation of BoardSize by BoardSize. the square objects have not been created.
        public Square[,] array;

        //default constructor: creates the square objects in "array", assigning each to a position in the BoardSize by BoardSize grid.
        private StishBoard()
        {
            //board size may change
            BoardSize = 9;
            array = new Square[BoardSize, BoardSize];
            for (int row = 0; row < BoardSize; row ++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    array[row, col] = new Square();
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

        //creates a public function called "getSquare", it will return the reference to the square object that sits in the position in "array" that is asked for in the arguments.
        public Square getSquare(uint row, uint col)
        {
            //TO DO: add error handling of arguments that are out of range of the array.
            try
            {
                return array[row, col];
            }
            catch
            {
                return null;
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
                    LookPlayer = Player1;
                }
                else
                {
                    LookPlayer = Player2;
                }
                        

                uint Bcost = 0;

                for (uint y = 0; y < BoardSize; y++)
                {
                    for (uint x = 0; x < BoardSize; x++)
                    {
                        if ((this.getSquare(x, y).Dep.DepType == "Barracks" || this.getSquare(x, y).Dep.DepType == "Base") && this.getSquare(x, y).Dep.OwnedBy == LookPlayer)
                        {
                            Bcost++;
                        }
                    }
                }

                Console.WriteLine("{0} has: {1} Coins. Their next Barracks will cost: {2} Coins", LookPlayer.GetPlayerNum ,LookPlayer.Balance, (Bcost * 3));
            }

            for (int y = 0; y < BoardSize; y++)
            {
                for (int x = 0; x < BoardSize; x++)
                {
                    array[x, y].Render(x, y);
                }
            }

        }




    }
}
