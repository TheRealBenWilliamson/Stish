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
        public bool OnBoard(Coordinate num)
        {
            try
            {
                Square Check = StishBoard.Instance.getSquare(num);
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

        public void BuyBarracks(Coordinate Pur, Player ConPlayer)
        {
            if ((StishBoard.Instance.getSquare(Pur) != null) && (StishBoard.Instance.getSquare(Pur).Dep.DepType == "Empty") && (StishBoard.Instance.getSquare(Pur).Owner == ConPlayer))
            {
                //check how many barracks the player already has and multiply it buy the cost of one barracks
                uint multiply = 0;
                Coordinate See = new Coordinate();

                for (uint y = 0; y < StishBoard.Instance.BoardSize; y++)
                {
                    for (uint x = 0; x < StishBoard.Instance.BoardSize; x++)
                    {
                        See.X = x;
                        See.Y = y;
                        if ((StishBoard.Instance.getSquare(See).Dep.DepType == "Barracks" || StishBoard.Instance.getSquare(See).Dep.DepType == "Base") && StishBoard.Instance.getSquare(See).Dep.OwnedBy == ConPlayer)
                        {
                            multiply++;
                        }
                    }
                }

                if (ConPlayer.Balance >= 3 * multiply)
                {
                    StishBoard.Instance.getSquare(Pur).Dep = new Barracks(StishBoard.Instance.getSquare(Pur).Owner, StishBoard.Instance.getSquare(Pur), 5);
                    ConPlayer.Balance -= 3 * multiply;
                }
            }
        }

        public void BuyUnit(Coordinate Pur, Player ConPlayer)
        {
            if ((StishBoard.Instance.getSquare(Pur) != null) && (StishBoard.Instance.getSquare(Pur).Dep.DepType == "Empty") && (StishBoard.Instance.getSquare(Pur).Owner == ConPlayer))                
            {
                //spend the entire player balance 
                if (ConPlayer.Balance > 0)
                {
                    StishBoard.Instance.getSquare(Pur).Dep = new Unit(StishBoard.Instance.getSquare(Pur).Owner, StishBoard.Instance.getSquare(Pur), ConPlayer.Balance);
                    ConPlayer.Balance = 0;
                }                 
            }
        }

        private void Drag(Coordinate FromCo, Coordinate ToCo, Player MyPlayer)
        {
            Square From = StishBoard.Instance.getSquare(FromCo);
            Square To = StishBoard.Instance.getSquare(ToCo);

            From.Owner = MyPlayer;
            To.Owner = MyPlayer;

            To.Dep = From.Dep;
            From.Dep = new Empty();
        }

        public void Attack(Coordinate From, Coordinate Check, Player MyPlayer)
        {
            //attack
            //adjust health and then if the attacking unit won, use the drag function
            //i dont know if i want to use the drag function on an attack. i will wait until i test it to decide
            //i dont have to redefine these squares but i think it helps the code read better
            Square Attacker = StishBoard.Instance.getSquare(From);
            Square Defender = StishBoard.Instance.getSquare(Check);
            String CheckDep = StishBoard.Instance.getSquare(Check).Dep.DepType;
            Player Owner = StishBoard.Instance.getSquare(Check).Dep.OwnedBy;

            //attacker must have more some MP to attack to prevent spawn attacking
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
                        StishBoard.Instance.getSquare(Check).Dep = new Barracks(MyPlayer, StishBoard.Instance.getSquare(Check), 5);
                        StishBoard.Instance.getSquare(Check).Owner = MyPlayer;
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
                        StishBoard.Instance.getSquare(Check).Dep = new Barracks(MyPlayer, StishBoard.Instance.getSquare(Check), 5);
                        StishBoard.Instance.getSquare(Check).Owner = MyPlayer;
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

        public bool Action(Coordinate From, Coordinate Check, Player MyPlayer)
        {
            //the bool output lets the caller know if the unit moved
            bool Moved = false;
            String CheckDep = StishBoard.Instance.getSquare(Check).Dep.DepType;
            Player Owner = StishBoard.Instance.getSquare(Check).Dep.OwnedBy;
            if (((CheckDep == "Empty") || (CheckDep == "Barracks") && Owner == MyPlayer))
            {
                //drag or destroys a friendly barracks
                if (StishBoard.Instance.getSquare(From).Dep.MP > 0)
                {
                    StishBoard.Instance.getSquare(From).Dep.MP--;
                    Drag(new Coordinate(From.X, From.Y), new Coordinate(Check.X, Check.Y), MyPlayer);
                    Moved = true;
                }

            }
            else if ((CheckDep == "Unit" || CheckDep == "Barracks" || CheckDep == "Base") && (Owner != MyPlayer))
            {
                Attack(From, Check, MyPlayer);
            }

            //TerritoryDeaths();
            //BaseSquares();
            return Moved;
        }


    }
}
