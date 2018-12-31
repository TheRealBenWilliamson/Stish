using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class ForeSight
    {
        //singleton

        private static ForeSight instance;

        public static ForeSight Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ForeSight();
                }
                return instance;
            }
        }

        //the foresight class helps us build new nodes by predicting all possible moves from a node
        //these functions will be called by the node objects in order to tell the nodes, how to configure their children

        //predict will take a boardstate argument and then create a new board state and node for each possible future move

        public bool Connected(BoardState Now, Coordinate MoveFrom, Coordinate MoveTo)
        {
            //go from "MoveTo" to "MoveFrom"
            //use a similar path finding algorithm to the territory bursts but incentivise a direction.
            //searching a new node costs a movement point. branch termminates when all points are used. tree terminates when all branches have concluded

            bool Connect = true;
            //lists as we dont want a limit that would be given by an array
            List<Coordinate> ToCheck = new List<Coordinate>();
            List<Coordinate> Checked = new List<Coordinate>();
            List<Coordinate> Path = new List<Coordinate>();
            Coordinate Search = new Coordinate();

            //TO DO search cardinals in order of distance. use CASES!

            ToCheck.Add(MoveTo);
            while(ToCheck.Count != 0)
            {
                Search = ToCheck[0];
                ToCheck.Remove(Search);
                Checked.Add(Search);

                Coordinate Up = new Coordinate(Search.X,Search.Y - 1);
                Coordinate Right = new Coordinate(Search.X + 1, Search.Y);
                Coordinate Down = new Coordinate(Search.X, Search.Y + 1);
                Coordinate Left = new Coordinate(Search.X - 1, Search.Y);
                

                if ((Now.getSquare(Up) != null) && (Now.getSquare(Up).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Up);
                }
                if ((Now.getSquare(Right) != null) && (Now.getSquare(Right).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Right);
                }
                if ((Now.getSquare(Down) != null) && (Now.getSquare(Down).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Down);
                }
                if ((Now.getSquare(Left) != null) && (Now.getSquare(Left).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Left);
                }
            }



            return Connect;
        }


        public bool CheckObstruction(BoardState Now, Coordinate From, Coordinate To)
        {
            //the temp and final lists are used to record which path was used to get to the unit.
            //this route function will make sure there are no obstructions and an appropriate territory path will follow an AIs unit as it moves.
            List<Coordinate> Temp = new List<Coordinate>();
            List<Coordinate> Final = new List<Coordinate>();
            bool Obstruct = false;
            Coordinate look = new Coordinate();

            //there is often more than one route to a position but the fewest moves will be a higher priority
             
            return Obstruct;
        }

        //this may not need to be void however i dont yet know if or what the output will be (currently just counts)
        public void Sweep(BoardState Now, Coordinate Check, Player Side)
        {
            int count = 0;

            Coordinate look = new Coordinate();
            for(uint x = 0; x < Now.BoardSize; x++)
            {
                for(uint y = 0; y < Now.BoardSize; y++)
                {
                    look.X = x;
                    look.Y = y;
                    //stishboard.instance is okay here as this variable is global and is not dependant on a particular state
                    if(Check.Get2DDistance(look) <= StishBoard.Instance.GameMP)
                    {
                        //general squares around unit within range
                        if((Now.getSquare(look) != null) && !((Now.getSquare(look).Dep.OwnedBy) == Side && ((Now.getSquare(look).Dep.DepType == "Unit") || (Now.getSquare(look).Dep.DepType == "Base"))))
                        {
                            //the unit can legally move to any of these positions however the events of this action are not distinguished
                            //obstructions are not accounted for
                            count++;
                        }
                        
                    }
                }
            }         
        }

        public int BarracksCost(BoardState Now, Player Side)
        {
            //finds if the player can afford a barracks
            int multiply = 1;

            Coordinate look = new Coordinate();
            for (uint x = 0; x < Now.BoardSize; x++)
            {
                for (uint y = 0; y < Now.BoardSize; y++)
                {
                    look.X = x;
                    look.Y = y;

                    if ((Now.getSquare(look).Dep.OwnedBy == Side) && (Now.getSquare(look).Dep.DepType == "Barracks"))
                    {
                        multiply++;
                    }
                }
            }
            return (3 * multiply);
        }

        public void BuyPossibility(BoardState Now, Player Side)
        {
            //count will not be returned as it is just a place holder for the munipulating function
            int count = 0;
            int cost = BarracksCost(Now, Side);

            Coordinate look = new Coordinate();
            for (uint x = 0; x < Now.BoardSize; x++)
            {
                for (uint y = 0; y < Now.BoardSize; y++)
                {
                    look.X = x;
                    look.Y = y;

                    if ((Now.getSquare(look).Dep.OwnedBy == Side) && (Now.getSquare(look).Dep.DepType == "Empty"))
                    {                      
                        if (Side.Balance > 0)
                        {
                            //can afford a unit
                            count++;

                            BoardState UnitBoardState = new BoardState(Now);
                            UnitBoardState.getSquare(look).Dep = new Unit(Side, Now.getSquare(look), Side.Balance);
                            if(Side.GetPlayerNum == "Player1")
                            {

                            }
                            //UnitBoardState. PLAYER .balance -= cost;
                        }
                        if (Side.Balance >= cost)
                        {
                            //can afford a barracks
                            count++;

                            //BoardState BarracksBoardState = new BoardState(Now);
                            //BarracksBoardState.getSquare(look).Dep = new Barracks(Side, Now.getSquare(look), 5);
                            //GeneratedBoardState. PLAYER .balance -= cost;

                            //this takes a player from one boardstate and needs to substract the cost of this purchase in the new boardstate's player. the player is ambiguous so i dont know whether to call the get/set of "player1" or "player2"
                        }
                    }
                }
            }
        }


        public void Predict(BoardState Now, BoardState ParentBoard, Player Side)
        {
            //side correlates to Allegiance but i dont want to call them the same thing in order to avoid confusion
            //things that determine a next turn: all positions around a unit (done once for each unit), buying a unit in any currently owned territory, buying a unit in any currently owned territory, absolutely nothing.
            //movement about a unit can be found using the "diamond" technique or the "jail bars" technique

            Coordinate look = new Coordinate();
            for (uint y = 0; y < Now.BoardSize; y++)
            {
                for (uint x = 0; x < Now.BoardSize; x++)
                {
                    look.Y = y;
                    look.X = x;
                    Sweep(Now, look, Side);
                }
            }
            
        }

        /*belongs to the player: either empty or has a unit
        belongs to player and is empty: 
            buy a unit
                they have enough money (greater than 0)
                    NEW BOARDSTATE WITH UNIT ON THIS SQUARE
            buy a barracks
                they have enough money (greater than or equal to the barracks cost function)
                    NEW BOARDSTATE WITH BARRACKS ON THIS SQUARE
        any square
            is there a unit in range of this square?
                yes. 
                    if empty then NEW BOARDSTATE WITH UNIT ON THIS SQUARE
        
        */
    }
}
