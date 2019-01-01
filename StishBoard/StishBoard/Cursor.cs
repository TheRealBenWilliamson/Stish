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
        //the free cursor will be yellow and can be moved about the StishBoard.Instance freely, it does not change any game elements and is used to "land" the Locked cursor and show the information of squares beneath it.
        //the locked cursor will be green and is used to depict which square on the StishBoard.Instance is being munipulated. the static cursor can only be ontop of a friendly unit.
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

        private Cursor()
        {
        }

        public Coordinate Pos = new Coordinate();

        //this constructor is behaving wildly and is causing a lot of errors so i have removed it        
        private uint Xco = 0;
        private uint Yco = 0;
        
        public enum Mode { free, locked };
        private Mode mode = Mode.free;

        private bool m_SpaceEnds = false;

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

        public bool SpaceEnds
        {
            get
            {
                return m_SpaceEnds;
            }
            set
            {
                m_SpaceEnds = value;
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
            Analytics.Cardinal(Cont, Pos);
        
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
            if (StishBoard.Instance.getSquare(Check).Dep.DepType == "Unit" && StishBoard.Instance.getSquare(Check).Dep.OwnedBy == MyPlayer)
            {
                return true;
            }
            else return false;
        }       

        public bool Move(Player ConPlayer, string input)
        {
            bool End = false;
            Coordinate CursorCoord = new Coordinate(Pos.X, Pos.Y);

            if ((input == "_") || ((input == " ") && (SpaceEnds == true)))
            {
                End = true;
            }
            else if (input == "W")
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
                End = GameMaster.Instance.BuyBarracks(CursorCoord, ConPlayer, StishBoard.Instance);
            }
            else if (input == "E")
            {
                //buy unit
                End = GameMaster.Instance.BuyUnit(CursorCoord, ConPlayer, StishBoard.Instance);
            }           

            if (GameMaster.Instance.OnBoard(CursorCoord, StishBoard.Instance) == true)
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
                    //action is true if the cursor moved. this helps distinguish if the cursor should move after attacking.
                    if (GameMaster.Instance.Action(Pos, CursorCoord, ConPlayer, StishBoard.Instance) == true)
                    {
                        Pos.X = CursorCoord.X;
                        Pos.Y = CursorCoord.Y;
                        SpaceEnds = true;
                    }
                }

            }
            else
            {
                //move was not valid
            }

            Console.Clear();
            StishBoard.Instance.Render();
            Render(ConPlayer);
            return End;
            //at the end of a turn the cursor is set to free so that the other player cannot control enemy units


        }
    }
}




        

        
    

