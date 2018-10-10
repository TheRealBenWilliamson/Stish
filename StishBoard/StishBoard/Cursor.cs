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
        public enum Purchase { Barracks, Unit };
        private Purchase purchase;

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
                return Xco;
            }
            set
            {
                Xco = value;
            }
        }

        public uint FindY
        {
            get
            {
                return Yco;
            }
            set
            {
                Yco = value;
            }
        }


        //this is purely cosmetic and helps describe the squares surrounding the cursor       

        public void Cardinal(uint Xc, uint Yc, uint Xu, uint Yu, uint Xr, uint Yr, uint Xd, uint Yd, uint Xl, uint Yl, Player Cont)
        {

            System.Console.ForegroundColor = Cont.GetRenderColour();
            Console.SetCursorPosition(4 * 17, 0);
            Console.WriteLine("{0}'s Turn", Cont.GetPlayerNum);
            Console.ResetColor();           
            
            Console.SetCursorPosition(0, 0);
            

            uint[,] CardinalDir = new uint[1, 5];
            uint[,] Coord = new uint[5, 2];
            List<string> CardinalString = new List<string>() { "Centre", "Up", "Right", "Down", "Left" };
            List<uint> CoordSing = new List<uint>(){ Xc, Yc, Xu, Yu, Xr, Yr, Xd, Yd, Xl, Yl };

            int count = 0;
            for (uint row = 0; row < 5; row++)
            {
                for (uint col = 0; col < 2; col++)
                {
                    Coord[row, col] = CoordSing[count];
                    count++;
                }
            }
            
            for (int card = 0; card < 5; card++)
            {
                Square Check = board.getSquare(Coord[card,0], Coord[card, 1]);

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

                    //there is definately a better way to do this using for loops
                    //repeat for all cardinal directions
                    //add movement points
                    Console.SetCursorPosition(4 * 17, (card + 2));
                    Console.WriteLine("{0} has: {1} Health, it is contains: {2} , belongs to: {3} and has {4} Movement Points", CardinalString[card],Check.Dep.Health.ToString(), CheckType, CheckOwner, Check.Dep.MP.ToString());
                }

            
            }
            

        }

        public void Render(Player Cont)
        {
            //Info
            Cardinal(Xco, Yco, Xco, (Yco - 1), (Xco + 1), Yco, Xco, (Yco + 1), (Xco - 1), Yco, Cont);
        
            //Cursor
            if (CursorMode == Mode.free)
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
            

        public bool Action(uint FromX, uint FromY, uint CheckX, uint CheckY , Player MyPlayer)
        {
            //the bool output lets the caller know if the unit moved
            bool Moved = false;
            String CheckDep = board.getSquare(CheckX, CheckY).Dep.DepType;
            Player Owner = board.getSquare(CheckX, CheckY).Dep.OwnedBy;
            if ((CheckDep == "Empty") || (CheckDep == "Barracks" && Owner == MyPlayer))
            {
                //drag or destroys a friendly barracks
                if(board.getSquare(FromX, FromY).Dep.MP > 0)
                {
                    board.getSquare(FromX, FromY).Dep.MP--;
                    Drag(Xco, Yco, CheckX, CheckY, MyPlayer);
                    Moved = true;
                }
                
            }
            else if ((CheckDep == "Unit" || CheckDep == "Barracks" || CheckDep == "Base") && (Owner != MyPlayer))
            {
                //attack
                //adjust health and then if the attacking unit won, use the drag function
                //i dont know if i want to use the drag function on an attack. i will wait until i test it to decide
                //i dont have to redefine these squares but i think it helps the code read better
                Square Attacker = board.getSquare(FromX, FromY);
                Square Defender = board.getSquare(CheckX, CheckY);
                
                //attacker must have more some MP to attack to prevent spawn attacking
                if (Attacker.Dep.MP > 0)
                {
                    //checks who wins the combat: attacker or defender
                    //attacker wins
                    if (Attacker.Dep.Health > Defender.Dep.Health)
                    {
                        Attacker.Dep.Health -= Defender.Dep.Health;
                        //Barracks are a special case
                        if (CheckDep == "Barracks")
                        {
                            board.getSquare(CheckX, CheckY).Dep.OwnedBy = MyPlayer;
                            board.getSquare(CheckX, CheckY).Owner = MyPlayer;
                        }
                        else
                        {
                            Defender.Dep = new Empty();
                        }

                    }
                    //defender wins
                    else if (Attacker.Dep.Health < Defender.Dep.Health)
                    {
                        Defender.Dep.Health -= Attacker.Dep.Health;
                        Attacker.Dep = new Empty();
                        mode = Mode.free;
                    }
                    //both have equal health
                    else
                    {
                        if (CheckDep == "Barracks")
                        {
                            board.getSquare(CheckX, CheckY).Dep.OwnedBy = MyPlayer;
                            board.getSquare(CheckX, CheckY).Owner = MyPlayer;
                        }
                        else
                        {
                            Defender.Dep = new Empty();
                        }
                        Attacker.Dep = new Empty();
                        mode = Mode.free;
                    }
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

        private void BuyDep(uint XPur,uint YPur,Player ConPlayer)
        {
            if((board.getSquare(XPur,YPur) != null) && (board.getSquare(XPur, YPur).Dep.DepType == "Empty") && (board.getSquare(XPur, YPur).Owner == ConPlayer))
            {
                //purchase is acceptable
                if (purchase == Purchase.Barracks)
                {
                    //check how many barracks the player already has and multiply it buy the cost of one barracks
                    uint multiply = 0;

                    for (uint y = 0; y < 11; y++)
                    {
                        for (uint x = 0; x < 11; x++)
                        {
                            if ((board.getSquare(x, y).Dep.DepType == "Barracks" || board.getSquare(x, y).Dep.DepType == "Base") && board.getSquare(x, y).Dep.OwnedBy == ConPlayer)
                            {
                                multiply++;
                            }
                        }
                    }

                    if(ConPlayer.Balance >= 3 * multiply)
                    {
                        board.getSquare(XPur, YPur).Dep = new Barracks(board.getSquare(XPur, YPur).Owner, board.getSquare(XPur, YPur), 5);
                        ConPlayer.Balance -= 3 * multiply;
                    }
                    
                }
                else if(purchase == Purchase.Unit)
                {
                    //spend the entire player balance 
                    if(ConPlayer.Balance > 0)
                    {
                        board.getSquare(XPur, YPur).Dep = new Unit(board.getSquare(XPur, YPur).Owner, board.getSquare(XPur, YPur), ConPlayer.Balance);
                        ConPlayer.Balance = 0;
                    }
                    
                }
            }
        }

        public void Move(Player ConPlayer, string input)
        {            

            uint ChangeX = Xco;
            uint ChangeY = Yco;

            if (input == "W")
            {
                ChangeY -= 1;
            }
            else if (input == "A")
            {
                ChangeX -= 1;
            }
            else if (input == "S")
            {
                ChangeY += 1;
            }
            else if (input == "D")
            {
                ChangeX += 1;
            }
            else if (input == " ")
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
            else if (input == "Q")
            {
                //buy barracks
                purchase = Purchase.Barracks;
                BuyDep(ChangeX, ChangeY, ConPlayer);
            }
            else if (input == "E")
            {
                //buy unit
                purchase = Purchase.Unit;
                BuyDep(ChangeX, ChangeY, ConPlayer);
            }
            else if (input == "_")
            {
                //enter has to be done in the human/computer override
            }

            if (OnBoard(ChangeX, ChangeY) == true)
            {
                //free
                if (CursorMode == Mode.free)
                {
                    Xco = ChangeX;
                    Yco = ChangeY;
                }

                //locked
                if (CursorMode == Mode.locked)
                {
                    if (Action(Xco, Yco, ChangeX, ChangeY, ConPlayer) == true)
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

            Console.Clear();
            board.Render();
            Render(ConPlayer);

            //at the end of a turn the cursor is set to free so that the other player cannot control enemy units


        }
    }
}




        

        
    

