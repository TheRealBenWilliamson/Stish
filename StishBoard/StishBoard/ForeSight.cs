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

        public void UnitBasedMovement(StishMiniMaxNode Parent, BoardState Now, Coordinate From, Player Side, Coordinate To)
        {
            //this will check what is on the destination square and then use the game master function.
            //the game master currently only works with the StishBoard. if i make StishBoard a dervived class from the board state, i can generalize the game master functions and use them here
            BoardState UnitMovedChild = new BoardState(Now);
            GameMaster.Instance.Action(From, To, Side, Now);
            Player NextTurn;
            if (Side.GetPlayerNum == "Player1")
            {
                //opposite allegiance to it's parent
                NextTurn = UnitMovedChild.Player2;
            }
            else
            {
                NextTurn = UnitMovedChild.Player1;
            }
            StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, NextTurn, UnitMovedChild);
        }

        //this may not need to be void however i dont yet know if or what the output will be (currently just counts)
        public void SweepSearch(StishMiniMaxNode Parent, BoardState Now, Coordinate Check, Player Side)
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
                            UnitBasedMovement(Parent, Now, Check, Side, Look);
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
                    StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, UnitBoardState.Player2, UnitBoardState);
                }
                else if(Side.GetPlayerNum == "Player2")
                {
                    UnitBoardState.Player2.Balance = 0;
                    StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, UnitBoardState.Player1, UnitBoardState);
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
            else if((Now.getSquare(Invest).Dep.OwnedBy == Side) && (Now.getSquare(Invest).Dep.DepType == "Unit"))
            {

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
