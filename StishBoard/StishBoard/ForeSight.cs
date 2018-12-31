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

        public void UnitBasedMovement(BoardState Now, Coordinate Check, Player Side, Coordinate To)
        {
            //this will check what is on the destination square and then use the game master function.
            //the game master currently only works with the StishBoard. if i make StishBoard a dervived class from the board state, i can generalize the game master functions and use them here
        }

        //this may not need to be void however i dont yet know if or what the output will be (currently just counts)
        public void SweepSearch(BoardState Now, Coordinate Check, Player Side)
        {
            Coordinate Upper = new Coordinate();
            Coordinate Lower = new Coordinate();
            Coordinate Look = new Coordinate();
            //checking if too close to an edge of the board

            //upper refers to the upper left corner (lower coordinate values)
            if (Check.X < StishBoard.Instance.GameMP)
            {
                Upper.X = 0;
            }
            if (Check.X > Now.BoardSize - StishBoard.Instance.GameMP)
            {
                Lower.X = Now.BoardSize;
            }
            if (Check.Y < StishBoard.Instance.GameMP)
            {
                Upper.Y = 0;
            }
            if (Check.Y > Now.BoardSize - StishBoard.Instance.GameMP)
            {
                Lower.Y = Now.BoardSize;
            }


            for (uint x = Upper.X; x < Lower.X; x++)
            {
                for (uint y = Upper.Y; y < Lower.Y; y++)
                {
                    Look.X = x;
                    Look.Y = y;
                    //stishboard.instance is okay here as this variable is global and is not dependant on a particular state
                    if (Check.Get2DDistance(Look) <= StishBoard.Instance.GameMP)
                    {
                        //general squares around unit within range
                        if ((Now.getSquare(Look) != null) && !((Now.getSquare(Look).Dep.OwnedBy) == Side && ((Now.getSquare(Look).Dep.DepType == "Unit") || (Now.getSquare(Look).Dep.DepType == "Base"))))
                        {
                            //the unit can legally move to any of these positions however the events of this action are not distinguished
                            //obstructions are not accounted for
                            
                        }

                    }
                }
            }
            
        }

        public uint BarracksCost(BoardState Now, Player Side)
        {
            //finds if the player can afford a barracks
            uint multiply = 1;

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

        public void BuyPossibility(StishMiniMaxNode Parent, BoardState Now, Player Side, Coordinate look)
        {
            uint cost = BarracksCost(Now, Side);

            if (Side.Balance > 0)
            {
                //can afford a unit

                BoardState UnitBoardState = new BoardState(Now);
                UnitBoardState.getSquare(look).Dep = new Unit(Side, Now.getSquare(look), Side.Balance);
                if (Side.GetPlayerNum == "Player1")
                {
                    UnitBoardState.Player1.Balance = 0;
                    //opposite allegiance to it's parent
                    StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, UnitBoardState.Player2);
                }
                else if(Side.GetPlayerNum == "Player2")
                {
                    UnitBoardState.Player2.Balance = 0;
                    StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, UnitBoardState.Player1);
                }
                
            }
            if (Side.Balance >= cost)
            {
                //can afford a barracks

                BoardState BarracksBoardState = new BoardState(Now);
                BarracksBoardState.getSquare(look).Dep = new Barracks(Side, Now.getSquare(look), 5);
                if (Side.GetPlayerNum == "Player1")
                {
                    BarracksBoardState.Player1.Balance -= cost;
                    //opposite allegiance to it's parent
                    StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, BarracksBoardState.Player2);
                }
                else if (Side.GetPlayerNum == "Player2")
                {
                    BarracksBoardState.Player2.Balance -= cost;
                    StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, BarracksBoardState.Player1);
                }
               
            }


        }

        private void TestSquare(StishMiniMaxNode Parent, BoardState Now, Player Side, Coordinate Invest)
        {
            if ((Now.getSquare(Invest).Dep.OwnedBy == Side) && (Now.getSquare(Invest).Dep.DepType == "Empty"))
            {
                //can buy possibly buy
                BuyPossibility(Parent, Now, Side, Invest);
            }

        }


        public void GenerateChildren(StishMiniMaxNode Parent)
        {
            //parent argument will always contain "this" when called.
            Player Allegiance = Parent.Allegiance;
            BoardState Position = Parent.NodeBoardState;

            Coordinate Look = new Coordinate();
            for (uint y = 0; y < Position.BoardSize; y++)
            {
                for (uint x = 0; x < Position.BoardSize; x++)
                {
                    Look.Y = y;
                    Look.X = x;

                    TestSquare(Parent, Position, Allegiance, Look);
                }
            }
        }


    }
}
