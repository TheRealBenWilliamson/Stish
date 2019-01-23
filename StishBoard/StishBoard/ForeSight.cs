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

        public void UnitBasedMovement(StishMiniMaxNode Parent, BoardState Now, List<Coordinate> Path, Player Side)
        {
            //this will check what is on the destination square and then use the game master function.
            //the game master currently only works with the StishBoard. if i make StishBoard a dervived class from the board state, i can generalize the game master functions and use them here
            BoardState UnitMovedChild = new BoardState(Now);

            for (int index = 0; index < Path.Count - 1; index++)
            {
                //from index to the one ahead of it therefore the function finishes one place short of the end
                GameMaster.Instance.Action(Path[index], Path[index +1], Side, UnitMovedChild);
            }
         
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
            StishMiniMaxNode UnitMoveNode = new StishMiniMaxNode(Parent, NextTurn, UnitMovedChild);

            //changes to the boardstate are made here using the FindPath function and itterating through the list with the gamemaster functions
        }

        public void CreatePathNode(List<PathNode> ToCheck, PathNode Parent, uint Cost, uint Health, BoardState World, Coordinate Invest)
        {           
            PathNode Generate = new PathNode(Parent, Cost, Health, World, Invest);
            ToCheck.Add(Generate);
        }

        private bool SearchForPathNode(PathNode Value, List<PathNode> Search)
        {
            //searches a list of path nodes and determines if there was a match
            bool found = false;
            for (int index = 0; index < Search.Count; index++)
            {
                //if this statement is changed to include cost, health and position in an "and" statement, the lowest cost movement could be found. this is not necessary though
                if (Search[index].Position == Value.Position)
                {
                    found = true;
                }
            }
            return found;
        }

        public List<Coordinate> FollowParents(PathNode Youngest)
        {
            List<Coordinate> OldestToYoungest = new List<Coordinate>();

            //finds youngest to oldest and then reverses the list
            PathNode Invest = Youngest;
            while(Invest != null)
            {
                OldestToYoungest.Add(Invest.Position);
                Invest = (PathNode)Invest.Parent;
            }

            OldestToYoungest.Reverse();

            return OldestToYoungest;
        }

        public List<Coordinate> TrainPath(BoardState board, Coordinate From, Coordinate Twitch, uint MoveCost, uint MoveHealth, Player Cont, List<PathNode> ToCheck, List<PathNode> Checked, List<Coordinate> Path)
        {
            MoveCost++;
            if ((board.getSquare(Twitch).Dep.OwnedBy != Cont) && (board.getSquare(Twitch).Dep.DepType == "Unit" || board.getSquare(Twitch).Dep.DepType == "Barracks" || board.getSquare(Twitch).Dep.DepType == "Base"))
            {
                //if an enemy dep type that can remove health
                //this prevents underflow errors as Health is a Uint
                if (MoveHealth >= board.getSquare(Twitch).Dep.Health)
                {
                    MoveHealth -= board.getSquare(Twitch).Dep.Health;
                }
                else
                {
                    MoveHealth = 0;
                }
            }

            //parameter for being a path member in the if statement
            //within range (an allowed cost) and positive health
            if ((MoveCost <= StishBoard.Instance.GameMP) && (MoveHealth > 0))
            {
                //suitable to be created as a new node but not checked for suitability for the list system
                PathNode NodeToTest = new PathNode(ToCheck[0], MoveCost, MoveHealth, board, Twitch);

                if (SearchForPathNode(NodeToTest, Checked) == false)
                {
                    //not already searched. note, this method allows there to be more than one node per square as long as the path is not 'essentially the same' which is a nice unintended consequence of this method.
                    CreatePathNode(ToCheck, ToCheck[0], MoveCost, MoveHealth, board, Twitch);

                    //is this the destination?
                    if ((Twitch.X == From.X) && (Twitch.Y == From.Y))
                    {
                        //recursion to create a list of PathNode Parents
                        Path = FollowParents(NodeToTest);
                        //returns a list of coordinates 'From --> To' for each individual step
                        return Path;
                    }
                }
            }

            //nothing of interest
            return null;
        }    

        //not void! returns a list of Pathnodes
        public List<Coordinate> FindPath(Coordinate From, Coordinate To, BoardState board)
        {
            //lists as we dont want a limit that would be given by an array
            List<Coordinate> Path = new List<Coordinate>();
            List<PathNode> ToCheck = new List<PathNode>();
            List<PathNode> Checked = new List<PathNode>();
            Coordinate Invest = new Coordinate();
            Coordinate Twitch = new Coordinate();
            uint MoveCost;
            uint MoveHealth = board.getSquare(To).Dep.Health;
            Player Cont = board.getSquare(From).Dep.OwnedBy;
            //MoveHealth must remain above 0 and MoveCost must remain below the maximum unit move distance

            //Start at To and end at From
            Invest.X = To.X;
            Invest.Y = To.Y;
            MoveCost = 0;

            //has to be cast here as the parent has to be given as null
            CreatePathNode(ToCheck, null, MoveCost, MoveHealth, board, Invest);

            //Spreading from Invest
            while (ToCheck.Count != 0)
            {
                Invest = ToCheck[0].Position;
                MoveCost = ToCheck[0].Cost;
                MoveHealth = ToCheck[0].Health;                               

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
                        //make changes to Movecost and MoveHealth here
                        List<Coordinate> Trained = TrainPath(board, From, Twitch, MoveCost, MoveHealth, Cont, ToCheck, Checked, Path);
                        if(Trained != null)
                        {
                            return Trained;
                        }                                                           
                    }
                }

                Checked.Add(ToCheck[0]);
                ToCheck.Remove(ToCheck[0]);

            }
            //there is no connection
            return null;
        }      

        //this may not need to be void however i dont yet know if or what the output will be (currently just counts)
        public void SweepSearch(StishMiniMaxNode Parent, BoardState Now, Coordinate Check, Player Side)
        {
            Coordinate Upper = new Coordinate();
            Coordinate Lower = new Coordinate();
            Coordinate Look = new Coordinate();

            //upper refers to the upper left corner (lower coordinate values)
            //assigns full values to the bounds and then changes them if they are inappropriate
            Upper.X = Check.X - StishBoard.Instance.GameMP;
            Upper.Y = Check.Y - StishBoard.Instance.GameMP;
            Lower.X = Check.X + StishBoard.Instance.GameMP;
            Lower.Y = Check.Y + StishBoard.Instance.GameMP;
            
            //checks if the unit is losing movement positions because it is too close to the edge of the board
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
                        if ((Now.getSquare(Look) != null) && !((Now.getSquare(Look).Owner) == Side && ((Now.getSquare(Look).Dep.DepType == "Unit") || (Now.getSquare(Look).Dep.DepType == "Base"))))
                        {
                            //the unit can legally move to any of these positions however the events of this action are not distinguished
                            //obstructions are not accounted for    
                            List<Coordinate> Path = FindPath(Check, Look, Now);
                            if (Path != null)
                            {
                                UnitBasedMovement(Parent, Now, Path, Side);
                            }
                            
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
            for (uint y = 0; y < Now.BoardSize; y++)
            {
                for (uint x = 0; x < Now.BoardSize; x++)
                {
                    look.X = x;
                    look.Y = y;

                    if ((Now.getSquare(look).Owner == Side) && (Now.getSquare(look).Dep.DepType == "Barracks"))
                    {
                        multiply++;
                    }
                }
            }
            return (3 * multiply);
        }

        public void BuyPossibility(StishMiniMaxNode Parent, BoardState Now, Player Side, Coordinate look, uint cost)
        {          

            if (Side.Balance > 0)
            {
                //can afford a unit

                BoardState UnitBoardState = new BoardState(Now);
                GameMaster.Instance.BuyUnit(look, Side, UnitBoardState);
                Player PlayersTurn;
                if (Side.GetPlayerNum == "Player1")
                {
                    //opposite allegiance to it's parent
                    PlayersTurn = UnitBoardState.Player2;
                }
                else
                {
                    PlayersTurn = UnitBoardState.Player1;
                }
                StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent, PlayersTurn, UnitBoardState);

            }
            if (Side.Balance >= cost)
            {
                //can afford a barracks

                BoardState BarracksBoardState = new BoardState(Now);
                GameMaster.Instance.BuyBarracks(look, Side, BarracksBoardState);
                Player PlayersTurn;
                if (Side.GetPlayerNum == "Player1")
                {
                    //opposite allegiance to it's parent
                    PlayersTurn = BarracksBoardState.Player2;
                }
                else
                {
                    PlayersTurn = BarracksBoardState.Player1;
                }
                StishMiniMaxNode BarracksCaseNode = new StishMiniMaxNode(Parent, PlayersTurn, BarracksBoardState);

            }


        }

        private void TestSquare(StishMiniMaxNode Parent, BoardState Now, Player Side, Coordinate Invest, uint cost)
        {
            if ((Now.getSquare(Invest).Owner == Side) && (Now.getSquare(Invest).Dep.DepType == "Empty"))
            {
                //can buy possibly buy
                BuyPossibility(Parent, Now, Side, Invest, cost);
            }
            if((Now.getSquare(Invest).Owner == Side) && (Now.getSquare(Invest).Dep.DepType == "Unit"))
            {
                //sweeps through all possible unit moves
                if(Now.getSquare(Invest).Dep.JustCreated == false)
                {
                    SweepSearch(Parent, Now, Invest, Side);
                }
            }

        }


        public void GenerateChildren(StishMiniMaxNode Parent)
        {
            //parent argument will always contain "this" when called.

            Player Allegiance = Parent.Allegiance;

            //this is the default "nothing happened" boardstate and node
            BoardState Position = new BoardState(Parent.NodeBoardState);
            Player NextTurn;
            if (Parent.Allegiance.GetPlayerNum == "Player1")
            {
                //opposite allegiance to it's parent
                NextTurn = new Human(Position.Player2);
            }
            else
            {
                NextTurn = new Human(Position.Player1);
            }
            StishMiniMaxNode NothingHappenedNode = new StishMiniMaxNode(Parent, NextTurn, Position);

            uint cost = BarracksCost(Parent.NodeBoardState, Allegiance);
            Coordinate Look = new Coordinate();
            for (uint y = 0; y < Parent.NodeBoardState.BoardSize; y++)
            {
                for (uint x = 0; x < Parent.NodeBoardState.BoardSize; x++)
                {
                    Look.Y = y;
                    Look.X = x;

                    TestSquare(Parent, Parent.NodeBoardState, Allegiance, Look, cost);
                }
            }
        }


    }
}
