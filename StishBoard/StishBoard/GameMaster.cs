using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class GameMaster
    {
        //the gamemaster controls the rules of stish and is what the players can call to make stuff happen
        //singleton class-object
        private static GameMaster instance;       

        public static GameMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameMaster();
                }
                return instance;
            }
        }                     

        private GameMaster()
        {

        }

        //coordinates are uints so anything less than 0 will overflow
        public bool OnBoard(Coordinate num, BoardState board)
        {
            try
            {
                Square Check = board.getSquare(num);
                if (Check != null)
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

        private void SearchAndPlace(Coordinate Value, List<Coordinate> Search, List<Coordinate> Place)
        {
            //searches the search list. if the value is not found in it, the value is added to the Place list
            bool found = false;
            for (int index = 0; index < Search.Count; index++)
            {
                if (Search[index] == Value)
                {
                    found = true;
                }
            }
            if (found == false)
            {
                Place.Add(Value);
            }
        }

        public List<Coordinate> Path2MP(Coordinate From, Coordinate To, Player Cont, BoardState board)
        {
            //2MP in the name of this function refers to the fact that this only works if a unit has 2 Movement points each turn.
            //finds the path from one square to another and returns a list containing the coordinates of each step.
            //this path takes into account the health of a unit incase of any attacking done on the path.
            //returns null if no possible path exists.

            List<Coordinate> Path = new List<Coordinate>();
            List<Coordinate> Checked = new List<Coordinate>();
            uint ThisHP = board.getSquare(From).Dep.Health;
            Coordinate Inspect = From;

            //loop with exceptions incase the route is not possible
            //stop if inspect determines that the coordinate 'To' is able to be moved onto OR if the path list 

            //only does one of these
            if(To.Y < Inspect.Y)
            {
                //Destination is above the start
                Inspect.Y--;
                Checked.Add(Inspect);
            }
            else if(To.X > Inspect.X)
            {
                //Destination is to the right of the start
                Inspect.X++;
            }
            else if (To.Y > Inspect.Y)
            {
                //Destination is below the start
                Inspect.Y++;
            }
            else if (To.X < Inspect.X)
            {
                //Destination is to the left of the start
                Inspect.X--;
            }

            


            return Path;
        }

        public void Connected(Coordinate From, Coordinate To, Player Cont, BoardState board)
        {
            for (int look = 0; look < 2; look++)
            {
                //lists as we dont want a limit that would be given by an array
                List<Coordinate> ToCheck = new List<Coordinate>();
                List<Coordinate> Checked = new List<Coordinate>();
                Coordinate Invest = new Coordinate();
                Coordinate Twitch = new Coordinate();               
                Invest.X = 5;

                if (Cont.GetPlayerNum == "Player1")
                {
                    Invest.Y = 9;
                }
                else
                {
                    Invest.Y = 1;
                }

                ToCheck.Add(Invest);
                //Checking Invest
                while (ToCheck.Count != 0)
                {
                    Invest = ToCheck[0];

                    ToCheck.Remove(Invest);
                    Checked.Add(Invest);

                    for (int dir = 0; dir < 4; dir++)
                    {
                        Twitch.X = Invest.X;
                        Twitch.Y = Invest.Y;

                        if (dir == 0)
                        {
                            //up
                            Twitch.MoveUp();
                        }
                        else if (dir == 1)
                        {
                            //right
                            Twitch.MoveRight();
                        }
                        else if (dir == 2)
                        {
                            //down
                            Twitch.MoveDown();
                        }
                        else if (dir == 4)
                        {
                            //left
                            Twitch.MoveLeft();
                        }

                        if (board.getSquare(Twitch) != null)
                        {
                            //parameter for being a path member in the if statement
                            if (board.getSquare((Twitch)).Owner == Cont)
                            {
                                SearchAndPlace(Twitch, Checked, ToCheck);
                            }
                        }
                    }
                }    
            }
        }

        public bool BuyBarracks(Coordinate Pur, Player ConPlayer, BoardState board)
        {
            bool bought = false;
            if ((board.getSquare(Pur) != null) && (board.getSquare(Pur).Dep.DepType == "Empty") && (board.getSquare(Pur).Owner == ConPlayer))
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

                if (ConPlayer.Balance >= 3 * multiply)
                {
                    board.getSquare(Pur).Dep = new Barracks(board.getSquare(Pur).Owner, board.getSquare(Pur), 5);
                    ConPlayer.Balance -= 3 * multiply;
                    bought = true;
                }
            }
            return bought;
        }

        public bool BuyUnit(Coordinate Pur, Player ConPlayer, BoardState board)
        {
            bool bought = false;
            if ((board.getSquare(Pur) != null) && (board.getSquare(Pur).Dep.DepType == "Empty") && (board.getSquare(Pur).Owner == ConPlayer))                
            {
                //spend the entire player balance 
                if (ConPlayer.Balance > 0)
                {
                    board.getSquare(Pur).Dep = new Unit(board.getSquare(Pur).Owner, board.getSquare(Pur), ConPlayer.Balance);
                    ConPlayer.Balance = 0;
                    bought = true;
                }                 
            }
            return bought;
        }

        private void Drag(Coordinate FromCo, Coordinate ToCo, Player MyPlayer, BoardState board)
        {
            Square From = board.getSquare(FromCo);
            Square To = board.getSquare(ToCo);

            From.Owner = MyPlayer;
            To.Owner = MyPlayer;

            To.Dep = From.Dep;
            From.Dep = new Empty();
        }

        public void Attack(Coordinate From, Coordinate Check, Player MyPlayer, BoardState board)
        {
            //attack
            //adjust health and then if the attacking unit won, use the drag function
            //i dont know if i want to use the drag function on an attack. i will wait until i test it to decide
            //i dont have to redefine these squares but i think it helps the code read better
            Square Attacker = board.getSquare(From);
            Square Defender = board.getSquare(Check);
            String CheckDep = board.getSquare(Check).Dep.DepType;
            Player Owner = board.getSquare(Check).Dep.OwnedBy;

            //attacker must be more than 1 turn old in order to attack
            if (Attacker.Dep.JustCreated == false)
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
                    Cursor.Instance.CursorMode = Cursor.Mode.free;
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
                    Cursor.Instance.CursorMode = Cursor.Mode.free;
                }
            }
        }

        public bool Action(Coordinate From, Coordinate Check, Player MyPlayer, BoardState board)
        {
            //the bool output lets the caller know if the unit moved
            bool Moved = false;
            String CheckDep = board.getSquare(Check).Dep.DepType;
            Player Owner = board.getSquare(Check).Dep.OwnedBy;
            if (((CheckDep == "Empty") || (CheckDep == "Barracks") && Owner == MyPlayer))
            {
                //drag or destroys a friendly barracks
                if (board.getSquare(From).Dep.MP > 0)
                {
                    board.getSquare(From).Dep.MP--;
                    Drag(new Coordinate(From.X, From.Y), new Coordinate(Check.X, Check.Y), MyPlayer, board);
                    Moved = true;
                }

            }
            else if ((CheckDep == "Unit" || CheckDep == "Barracks" || CheckDep == "Base") && (Owner != MyPlayer))
            {
                Attack(From, Check, MyPlayer, board);
            }

            //TerritoryDeaths();
            //BaseSquares();
            return Moved;
        }


    }
}
