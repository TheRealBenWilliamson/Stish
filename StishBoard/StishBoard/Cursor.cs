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

        //this constructor is behaving wildly and is causing a lot of errors so i have removed it
        private uint Xco = 0;
        private uint Yco = 0;
        public enum Mode { free, locked };
        private Mode mode = Mode.free;

        public Mode GetMode()
        {
            return mode;
        }


        //this is purely cosmetic and helps describe the squares surrounding the cursor
        //it is not done yet
        public enum SquareType { Empty, Edge, FUnit, EUnit, FBarracks, EBarracks, FBase, EBase}


        public void Cardinal(uint Xc, uint Yc, uint Xu, uint Yu, uint Xr, uint Yr, uint Xd, uint Yd, uint Xl, uint Yl)
        {
            Square Centre, Up, Right, Down, Left;
            Centre = board.getSquare(Xc,Yc);
            Up = board.getSquare(Xu, Yu);
            Right = board.getSquare(Xr, Yr);
            Down = board.getSquare(Xd, Yd);
            Left = board.getSquare(Xl, Yl);

            if (Centre != null)
            {
                string CentreType;
                if (Centre.Dep.DepType == null)
                {
                    CentreType = "Nothing";
                }
                else
                {
                    CentreType = Centre.Dep.DepType;
                }

                string CentreOwner;
                if (Centre.Dep.OwnedBy == null)
                {
                    CentreOwner = "No One";
                }
                else
                {
                    CentreOwner = Centre.Dep.OwnedBy.ToString();
                }

                //there is definately a better way to do this using for loops
                //add more info
                Console.SetCursorPosition(4 * 13, 2);
                Console.WriteLine("Centre has: {0} Health, it is contains: {1} and belongs to: {2}", Centre.GetHealth.ToString(), CentreType, CentreOwner);
            }
            

        }

        public void Render()
        {
            //Info
            Cardinal(Xco, Yco, Xco, (Yco - 1), (Xco + 1), Yco, Xco, (Yco + 1), (Xco - 1), Yco);


            //Cursor
            if(GetMode() == Mode.free)
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

        private void Drag(uint FromX, uint FromY, uint ToX, uint ToY, Player MyPlayer)
        {
            Square From = board.getSquare(FromX, FromY);
            Square To = board.getSquare(ToX, ToY);

            From.Owner = MyPlayer;
            To.Owner = MyPlayer;

            To.Dep = From.Dep;
            From.Dep = new Empty();
        }
        
        private void Attack(uint AttackX, uint AttackY, uint DefendX, uint DefendY, Player MyPlayer)
        {
            
        }
            
            /*
            From.Owner = MyPlayer;
            To.Owner = MyPlayer;

            To.Dep = From.Dep;
            From.Dep = new Empty();
            */       


        public bool Action(uint FromX, uint FromY, uint CheckX, uint CheckY , Player MyPlayer)
        {
            //the bool output lets the caller know if the unit moved
            bool Moved = false;
            String CheckDep = board.getSquare(CheckX, CheckY).Dep.DepType;
            Player Owner = board.getSquare(CheckX, CheckY).Dep.OwnedBy;
            if ((CheckDep == "Empty") || (CheckDep == "Barracks" && Owner == MyPlayer))
            {
                //drag or destroys a friendly barracks
                Drag(Xco, Yco, CheckX, CheckY, MyPlayer);
                Moved = true;
            }
            else if ((CheckDep == "Unit" || CheckDep == "Barracks") && (Owner != MyPlayer))
            {
                //attack
                //adjust health and then if the attacking unit won, use the drag function
                //i dont know if i want to use the drag function on an attack. i will wait until i test it to decide
                //i dont have to redefine these squares but i think it helps the code read better
                Square Attacker = board.getSquare(FromX, FromY);
                Square Defender = board.getSquare(CheckX, CheckY);
            
                //checks who wins the combat: attacker or defender
                //attacker wins
                if(Attacker.GetHealth > Defender.GetHealth)
                {
                    Attacker.GetHealth -= Defender.GetHealth;
                    Defender.Dep = new Empty();
                }
                //defender wins
                else if (Attacker.GetHealth < Defender.GetHealth)
                {
                    Defender.GetHealth -= Attacker.GetHealth;
                    Attacker.Dep = new Empty();
                    mode = Mode.free;
                }
                //both have equal health
                else
                {
                    Attacker.Dep = new Empty();
                    Defender.Dep = new Empty();
                    mode = Mode.free;
                }
            }

            return Moved;
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
            try
            {
                Square Check = board.getSquare(numX, numY);
                if(Check != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch
            {
                return false;
            }
        }       

        public void Move(Player ConPlayer)
        {
            Render();

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
                else if (mode == Mode.locked)
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
                if (GetMode() == Mode.free)
                {
                    Xco = ChangeX;
                    Yco = ChangeY;                  
                }

                //locked
                if (GetMode() == Mode.locked)
                {
                    if ( Action(Xco, Yco, ChangeX, ChangeY, ConPlayer) == true)
                    {
                        Xco = ChangeX;
                        Yco = ChangeY;
                    }
                }

            }
            else
            {
                //move was not valid
            }

            

        }
    }
}




        

        
    

