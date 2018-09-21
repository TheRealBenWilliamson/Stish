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
        //the free cursor will be yellow and can be moved about the board freely, it does not change any game elements and is used to "land" the Locked cursor and show the information of squares beneath it.
        //the locked cursor will be green and is used to depict which square on the board is being munipulated. the static cursor can only be ontop of a friendly unit.
        //the cursor can only be toggled above friendly territory.

        //the locked cursor will detect information about it's surroundings and display them to the user. it will also be the driving force of movement and tell the underlying unit where to go.

        /*functions:
        movement that splits off to the appropriate "locked" or "free" functions
        free movment
        locked movement (Checks adjacent squares to see what will happen upon moving onto that square. the action is determined by the deptype and owner of the sqaure)
        detection to discover what surrounds the cursor
        evaluation to tell the user what surrounds the cursor
        render
        */

        //the Cursor is a singleton
        private static Cursor instance;

        public static Cursor Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Cursor();
                }
                return instance;
            }
        }

        StishBoard board = StishBoard.Instance;


        private uint Xco = 0;
        private uint Yco = 0;
        public enum Mode { free, locked }
        public Mode mode = Mode.free;


        //this is purely cosmetic and helps describe the squares surrounding the cursor
        //it is not done yet
        public enum SquareType { Empty, Edge, FUnit, EUnit, FBarracks, EBarracks, FBase, EBase}

        public SquareType Cardinal(SquareType Centre, SquareType Up, SquareType Right, SquareType Down, SquareType Left)
        {
            Centre = 0;
            Up = 0;
            Right = 0;
            Down = 0;
            Left = 0;
            return Left;
        }

        public void Render()
        {
            if(mode == Mode.free)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            
            int x = (int)Xco;
            int y = (int)Yco;
            x = x * 4;
            Console.SetCursorPosition(x, y + 2);
            Console.WriteLine("[");
            Console.SetCursorPosition(x + 2, y + 2);
            Console.WriteLine("] ");
            Console.ResetColor();
            Console.SetCursorPosition(0, 13);

        }

        public void Drag(uint FromX, uint FromY, uint ToX, uint ToY, Player MyPlayer)
        {
            Square From = board.getSquare(FromX, FromY);
            Square To = board.getSquare(ToX, ToY);

            From.Owner = MyPlayer;
            To.Owner = MyPlayer;

            To.Dep = From.Dep;
            From.Dep = new Empty();
        }


        public void CheckSQ(uint XCheck, uint YCheck , Player MyPlayer)
        {
            String CheckDep = board.getSquare(XCheck, YCheck).Dep.DepType;
            Player Owner = board.getSquare(XCheck, YCheck).Dep.OwnedBy;
            if (CheckDep == "Empty")
            {
                //drag
            }
            else if ((CheckDep == "Unit" || CheckDep == "Barracks") && (Owner != MyPlayer))
            {
                //attack
            }
            else if (CheckDep == "Barracks" && Owner == MyPlayer)
            {
                //destroy the barracks
            }
        }

        public bool Land(uint XCheck, uint YCheck, Player MyPlayer)
        {
            if (board.getSquare(XCheck, YCheck).Dep.DepType == "Unit" && board.getSquare(XCheck, YCheck).Dep.OwnedBy == MyPlayer)
            {
                return true;
            }
            else return false;
        }

        //coordinates are uints so anything less than 0 will overflow
        public bool OnBoard(uint numX, uint numY)
        {
            if ((numX < 11) && (numY < 11))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Move(Player ConPlayer)
        {
            System.ConsoleKey put = Console.ReadKey(true).Key;
            uint ChangeX = Xco;
            uint ChangeY = Yco;

            if (put == ConsoleKey.W)
            {
                ChangeY -= 1;
            }
            else if (put == ConsoleKey.A)
            {
                ChangeX -= 1;
            }
            else if (put == ConsoleKey.S)
            {
                ChangeY += 1;
            }
            else if (put == ConsoleKey.D)
            {
                ChangeX += 1;
            }
            else if (put == ConsoleKey.Spacebar)
            {
                //free
                if (mode == Mode.free)
                {
                    //can only be done on a friendly Unit
                    if (Land(ChangeX, ChangeY, ConPlayer) == true)
                    {
                        mode = Mode.locked;
                    }
                }

                //locked
                if (mode == Mode.locked)
                {
                    //can be done anytime
                    mode = Mode.free;
                }

            }
            else if (put == ConsoleKey.Enter)
            {
                //End Turn
                
            }

            if(OnBoard(ChangeX,ChangeY) == true)
            {
                //free
                if (mode == Mode.free)
                {
                    Xco = ChangeX;
                    Yco = ChangeY;
                    Render();
                }

                //locked
                if (mode == Mode.locked)
                {
                    CheckSQ(ChangeX, ChangeY, ConPlayer);
                }

            }
            else
            {
                //move was not valid
            }

        }


        

        
    }
}
