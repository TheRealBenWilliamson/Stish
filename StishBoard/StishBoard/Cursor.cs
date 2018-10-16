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
        public enum Purchase { Barracks, Unit, Burst };
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

        private void Drag(Coordinate FromCo, Coordinate ToCo, Player MyPlayer)
        {
            Square From = board.getSquare(FromCo);
            Square To = board.getSquare(ToCo);

            From.Owner = MyPlayer;
            To.Owner = MyPlayer;

            To.Dep = From.Dep;
            From.Dep = new Empty();
        }


        /*
        private void SearchAndPlace(uint valueX, uint valueY, List<uint> SlistX, List<uint> SlistY, List<uint> PlistX, List<uint> PlistY)
        {
            bool found = false;
            for (int index = 0; index < SlistX.Count; index++)
            {
                if ((SlistX[index] == valueX) && (SlistY[index] == valueY))
                {
                    found = true;
                }
            }
            if (found == false)
            {
                PlistX.Add(valueX);
                PlistY.Add(valueY);
            }
        }

        private bool Search(uint valueX, uint valueY, List<uint> SlistX, List<uint> SlistY)
        {
            bool found = false;
            for (int index = 0; index < SlistX.Count; index++)
            {
                if ((SlistX[index] == valueX) && (SlistY[index] == valueY))
                {
                    found = true;
                }
            }
            return found;
        }

        
        public void TerritoryDeaths()
        {
            for (int look = 0; look < 2; look++)
            {
                Player LookPlayer;
                //lists as we dont want a limit that would be given by an array
                List<uint> ToCheckX = new List<uint>();
                List<uint> ToCheckY = new List<uint>();
                List<uint> CheckedX = new List<uint>();
                List<uint> CheckedY = new List<uint>();
                uint investX = 5;
                uint investY;
                
                if(look == 0)
                {
                    LookPlayer = board.Player1;
                    investY = 9;
                }
                else
                {
                    LookPlayer = board.Player2;
                    investY = 1;
                }
               
                ToCheckX.Add(investX);
                ToCheckY.Add(investY);
                //Checking Invest
                while(ToCheckX.Count != 0)
                {
                    investX = ToCheckX[0];
                    investY = ToCheckY[0];

                    ToCheckX.Remove(investX);
                    ToCheckY.Remove(investY);
                    CheckedX.Add(investX);
                    CheckedY.Add(investY);
                    if (board.getSquare((investX - 1), investY) != null)
                    {
                        //left
                        if (board.getSquare((investX - 1), investY).Owner == LookPlayer)
                        {
                            SearchAndPlace(investX - 1, investY, CheckedX, CheckedY, ToCheckX, ToCheckY);
                        }
                    }
                    if (board.getSquare(investX, (investY - 1)) != null)
                    {
                        //up
                        if (board.getSquare(investX, (investY - 1)).Owner == LookPlayer)
                        {
                            SearchAndPlace(investX, investY - 1, CheckedX, CheckedY, ToCheckX, ToCheckY);
                        }
                    }
                    if (board.getSquare((investX + 1), investY) != null)
                    {
                        //right
                        if (board.getSquare((investX + 1), investY).Owner == LookPlayer)
                        {
                            SearchAndPlace(investX + 1, investY, CheckedX, CheckedY, ToCheckX, ToCheckY);
                        }
                    }
                    if (board.getSquare(investX, (investY + 1)) != null)
                    {
                        //down
                        if (board.getSquare(investX, (investY + 1)).Owner == LookPlayer)
                        {
                            SearchAndPlace(investX, investY + 1, CheckedX, CheckedY, ToCheckX, ToCheckY);
                        }
                    }                                                       
                }

                for (uint y = 0; y < board.BoardSize; y++)
                {
                    for (uint x = 0; x < board.BoardSize; x++)
                    {
                        if ((board.getSquare(x,y).Owner == LookPlayer) && (Search(x,y,CheckedX,CheckedY) == false))
                        {
                            if(board.getSquare(x,y).Dep.DepType == "Barracks")
                            {
                                board.getSquare(x, y).Dep.OwnedBy = null;
                                board.getSquare(x, y).Dep.Health = 0;
                            }
                            else
                            {
                                board.getSquare(x, y).Dep = new Empty();
                            }
                            board.getSquare(x, y).Owner = null;
                        }
                        
                    }
                }


            }
        }
        

        public void BaseSquares()
        {
            for (int look = 0; look < 2; look++)
            {
                Player LookPlayer;
                uint min = 0, max = 0;
                if (look == 0)
                {
                    LookPlayer = board.Player1;
                    min = board.Player1.BaseY - 1;
                    max = board.Player1.BaseY + 1;
                }
                else
                {
                    LookPlayer = board.Player2;
                    min = board.Player2.BaseY - 1;
                    max = board.Player2.BaseY + 1;
                }
              

                for (uint y = min; y < max; y++)
                {
                    for (uint x = (board.BoardSize/2) - 1; x < (board.BoardSize / 2) + 1; x++)
                    {
                        if (board.getSquare(x, y).Owner == null)
                        {
                            board.getSquare(x, y).Owner = LookPlayer;
                        }
                    }
                }

            }
        }
        */
        
        public bool Action(Coordinate From, Coordinate Check , Player MyPlayer)
        {
            //the bool output lets the caller know if the unit moved
            bool Moved = false;
            String CheckDep = board.getSquare(Check).Dep.DepType;
            Player Owner = board.getSquare(Check).Dep.OwnedBy;
            if (((CheckDep == "Empty") || (CheckDep == "Barracks") && Owner == MyPlayer))
            {
                //drag or destroys a friendly barracks
                if(board.getSquare(From).Dep.MP > 0)
                {
                    board.getSquare(From).Dep.MP--;
                    Drag(new Coordinate(Pos.X,Pos.Y), new Coordinate(Check.X,Check.Y), MyPlayer);
                    Moved = true;
                }
                
            }
            else if ((CheckDep == "Unit" || CheckDep == "Barracks" || CheckDep == "Base") && (Owner != MyPlayer))
            {
                //attack
                //adjust health and then if the attacking unit won, use the drag function
                //i dont know if i want to use the drag function on an attack. i will wait until i test it to decide
                //i dont have to redefine these squares but i think it helps the code read better
                Square Attacker = board.getSquare(From);
                Square Defender = board.getSquare(Check);
                
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
                            board.getSquare(Check).Dep = new Barracks(MyPlayer, board.getSquare(Check), 5);
                            board.getSquare(Check).Owner = MyPlayer;
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
                            board.getSquare(Check).Dep = new Barracks(MyPlayer, board.getSquare(Check), 5);
                            board.getSquare(Check).Owner = MyPlayer;
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

            //TerritoryDeaths();
            //BaseSquares();
            return Moved;
        }

        public bool Land(Coordinate Check, Player MyPlayer)
        {
            if (board.getSquare(Check).Dep.DepType == "Unit" && board.getSquare(Check).Dep.OwnedBy == MyPlayer)
            {
                return true;
            }
            else return false;
        }       

        private void BuyDep(Coordinate Pur,Player ConPlayer)
        {
            if((board.getSquare(Pur) != null) && (board.getSquare(Pur).Dep.DepType == "Empty") && (board.getSquare(Pur).Owner == ConPlayer))
            {
                //purchase is acceptable
                if (purchase == Purchase.Barracks)
                {
                    //check how many barracks the player already has and multiply it buy the cost of one barracks
                    uint multiply = 0;
                    Coordinate See = new Coordinate();

                    for (uint y = 0; y < board.BoardSize; y++)
                    {
                        for (uint x = 0; x < board.BoardSize; x++)
                        {
                            See.X = x;
                            See.Y = y;
                            if ((board.getSquare(See).Dep.DepType == "Barracks" || board.getSquare(See).Dep.DepType == "Base") && board.getSquare(See).Dep.OwnedBy == ConPlayer)
                            {
                                multiply++;
                            }
                        }
                    }

                    if(ConPlayer.Balance >= 3 * multiply)
                    {
                        board.getSquare(Pur).Dep = new Barracks(board.getSquare(Pur).Owner, board.getSquare(Pur), 5);
                        ConPlayer.Balance -= 3 * multiply;
                    }
                    
                }
                else if(purchase == Purchase.Unit)
                {
                    //spend the entire player balance 
                    if(ConPlayer.Balance > 0)
                    {
                        board.getSquare(Pur).Dep = new Unit(board.getSquare(Pur).Owner, board.getSquare(Pur), ConPlayer.Balance);
                        ConPlayer.Balance = 0;
                    }
                    
                }               
            }
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
                purchase = Purchase.Barracks;
                BuyDep(CursorCoord, ConPlayer);
            }
            else if (input == "E")
            {
                //buy unit
                purchase = Purchase.Unit;
                BuyDep(CursorCoord, ConPlayer);
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
                    if (Action(Pos, CursorCoord, ConPlayer) == true)
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




        

        
    

