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
        GameMaster master = GameMaster.Instance;
        public Coordinate Pos = new Coordinate();

        //this constructor is behaving wildly and is causing a lot of errors so i have removed it        
        private uint Xco = 0;
        private uint Yco = 0;
        
        public enum Mode { free, locked };
        private Mode mode = Mode.free;

        public Mode CursorMode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
            }
            
        }

        public uint FindX
        {
            get
            {
                return Pos.X;
            }
            set
            {
                Pos.X = value;
            }
        }

        public uint FindY
        {
            get
            {
                return Pos.Y;
            }
            set
            {
                Pos.Y = value;
            }
        }


        //this is purely cosmetic and helps describe the squares surrounding the cursor         

        public void Cardinal(Player Cont)
        {
            Coordinate up = new Coordinate(Pos.X, Pos.Y);
            up.MoveUp();
            Coordinate right = new Coordinate(Pos.X, Pos.Y);
            right.MoveRight();
            Coordinate down = new Coordinate(Pos.X, Pos.Y);
            down.MoveDown();
            Coordinate left = new Coordinate(Pos.X, Pos.Y);
            left.MoveLeft();
            System.Console.ForegroundColor = Cont.GetRenderColour();
            Console.SetCursorPosition(4 * 17, 0);
            Console.WriteLine("{0}'s Turn", Cont.GetPlayerNum);
            Console.ResetColor();           
            
            Console.SetCursorPosition(0, 0);
            
            List<string> CardinalString = new List<string>() { "Centre", "Up", "Right", "Down", "Left" };
            List<Coordinate> Direction = new List<Coordinate>() { Pos, up, right, down, left };
            
            for (int card = 0; card < 5; card++)
            {
                //Square Check = board.getSquare(Coord[card,0], Coord[card, 1]);
                Square Check = board.getSquare(Direction[card]);

                if (Check != null)
                {
                    string CheckType;
                    if (Check.Dep.DepType == null)
                    {
                        CheckType = "Nothing";
                    }
                    else
                    {
                        CheckType = Check.Dep.DepType;
                    }

                    string CheckOwner;
                    if (Check.Owner == null)
                    {
                        CheckOwner = "No One";
                    }
                    else
                    {
                        CheckOwner = Check.Owner.GetPlayerNum;
                    }

                    Console.SetCursorPosition(4 * 17, (card + 2));
                    Console.WriteLine("{0} has: {1} Health, it is contains: {2} , belongs to: {3} and has {4} Movement Points", CardinalString[card],Check.Dep.Health.ToString(), CheckType, CheckOwner, Check.Dep.MP.ToString());
                }         
            }            
        }


        public void Render(Player Cont)
        {
            //Info   
            Coordinate up = new Coordinate(Xco, Yco);
            up.MoveUp();
            Coordinate right = new Coordinate(Xco, Yco);
            right.MoveRight();
            Coordinate down = new Coordinate(Xco, Yco);
            down.MoveDown();
            Coordinate left = new Coordinate(Xco, Yco);
            left.MoveLeft();
            Cardinal(Cont);
        
            //Cursor
            if (CursorMode == Mode.free)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            
            int x = (int)Pos.X;
            int y = (int)Pos.Y;
            x = x * 4;
            Console.SetCursorPosition(x, y + 2);
            Console.WriteLine("[");
            Console.SetCursorPosition(x + 2, y + 2);
            Console.WriteLine("] ");
            Console.ResetColor();
            Console.SetCursorPosition(0, 13);

        }       

        
        
        public bool Land(Coordinate Check, Player MyPlayer)
        {
            if (board.getSquare(Check).Dep.DepType == "Unit" && board.getSquare(Check).Dep.OwnedBy == MyPlayer)
            {
                return true;
            }
            else return false;
        }       

        

        public void Move(Player ConPlayer, string input)
        {

            Coordinate CursorCoord = new Coordinate(Pos.X, Pos.Y);
            //uint ChangeY = Yco;

            if (input == "W")
            {
                CursorCoord.MoveUp();
            }
            else if (input == "A")
            {
                CursorCoord.MoveLeft();
            }
            else if (input == "S")
            {
                CursorCoord.MoveDown();
            }
            else if (input == "D")
            {
                CursorCoord.MoveRight();
            }
            else if (input == " ")
            {
                //free
                if (mode == Mode.free)
                {
                    //can only be done on a friendly Unit
                    if (Land(CursorCoord, ConPlayer) == true)
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
            else if (input == "Q")
            {
                //buy barracks
                master.BuyBarracks(CursorCoord, ConPlayer);
            }
            else if (input == "E")
            {
                //buy unit
                master.BuyUnit(CursorCoord, ConPlayer);
            }
            else if (input == "_")
            {
                //enter has to be done in the human/computer override
            }

            if (master.OnBoard(CursorCoord) == true)
            {
                //free
                if (CursorMode == Mode.free)
                {
                    Pos.X = CursorCoord.X;
                    Pos.Y = CursorCoord.Y;
                }

                //locked
                if (CursorMode == Mode.locked)
                {
                    if (master.Action(Pos, CursorCoord, ConPlayer) == true)
                    {
                        Pos.X = CursorCoord.X;
                        Pos.Y = CursorCoord.Y;
                    }
                }

            }
            else
            {
                //move was not valid
            }

            Console.Clear();
            board.Render();
            Render(ConPlayer);

            //at the end of a turn the cursor is set to free so that the other player cannot control enemy units


        }
    }
}




        

        
    

