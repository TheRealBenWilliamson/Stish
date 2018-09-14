using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Cursor
    {
        //the cursor is not a deployment type calss as it has no owner, health, or icon.
        //cursor can be controlled by the player on their turn.
        //the cursor is always in one of two modes, free or locked.
        //the free cursor will be yellow and can be moved about the board freely, it does not change any game elements and is used to "land" the static cursor and show the information of squares beneath it.
        //the locked cursor will be green and is used to depict which square on the board is being munipulated. the static cursor can only be ontop of a friendly unit.
        //the cursor can only be toggled above friendly territory.

        //the locked cursor will detect information about it's surroundings and display them to the user. it will also be the driving force of movement and tell the underlying unit where to go.

        /*functions:
        movement that splits off to the appropriate "locked" or "free" functions
        free movment
        locked movement (uses a black list array to determine where the unit cant 

        */

        private uint Xco = 0;
        private uint Yco = 0;
        public enum Mode { free, locked }
        public Mode mode = Mode.free;


        
        public enum Cardinal { can,cant,attack }

        public Cardinal Detect(Cardinal Up, Cardinal Right, Cardinal Down, Cardinal Left)
        {
            Up = 0;
            Right = 0;
            Down = 0;
            Left = 0;
            return Left;

        }

        public void FreeMove()
        {
            System.ConsoleKey put = Console.ReadKey(true).Key;
            
            if (put == ConsoleKey.W)
            {
                Yco += 1;
            }
            else if (put == ConsoleKey.A)
            {
                Xco += 1;
            }
            else if (put == ConsoleKey.S)
            {
                Yco -= 1;
            }
            else if (put == ConsoleKey.D)
            {
                Xco -= 1;
            }
            else if (put == ConsoleKey.Spacebar)
            {
                mode = Mode.locked;
            }
        }




        
    }
}
