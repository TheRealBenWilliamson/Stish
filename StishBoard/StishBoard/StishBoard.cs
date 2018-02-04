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

        //creates an array called "array" capable of holding square objects in the orientation of 11 by 11. the square objects have not been created.
        public Square[,] array = new Square[11,11];

        //default constructor: creates the square objects in "array", assigning each to a position in the 11 by 11 grid.
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
        public Square getSquare(int row, int col)
        {
            //TO DO: add error handling of arguments that are out of range of the array.
            return array[row, col];           
        }

        //creates a public render method called "Render" to render each of the squares to the console. it calls a Render method on each of the square objects held within 'array'.
        //not sure if this change is right (stolen from the arguments of Render() ) ...   int x, int y    
        public void Render()
        {
            for (int y = 0; y < 11; y++)
            {
                for (int x = 0; x < 11; x++)
                {
                    array[x, y].Render(x, y);
                }
            }
        }




    }
}
