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

        private int m_TotalPredictionCount = 0;
        private int m_TurnPredictionCount = 0;

        public void PredctionCount()
        {
            m_TotalPredictionCount++;
            m_TurnPredictionCount++;
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
         
            Player NextTurn = Parent.Allegiance;
            /*
            if (Side.GetPlayerNum == "Player1")
            {
                //opposite allegiance to it's parent
                NextTurn = UnitMovedChild.Player2;
            }
            else
            {
                NextTurn = UnitMovedChild.Player1;
            }
            */

            //Parent.Parent as "parent" is actually just an updated model of the "real" parent where no actions were taken
            StishMiniMaxNode UnitMoveNode = new StishMiniMaxNode(Parent.Parent, NextTurn, UnitMovedChild);
            PredctionCount();

            //changes to the boardstate are made here using the FindPath function and itterating through the list with the gamemaster functions
        }

        public PathNode CreatePathNode(List<PathNode> ToCheck, PathNode Parent, uint Cost, uint Health, BoardState World, Coordinate Invest)
        {           
            PathNode Generate = new PathNode(Parent, Cost, Health, World, Invest);
            ToCheck.Add(Generate);
            return Generate;
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
            List<Coordinate> YoungestToOldest = new List<Coordinate>();

            //finds youngest to oldest and then reverses the list
            PathNode Invest = Youngest;
            while(Invest != null)
            {
                YoungestToOldest.Add(Invest.Position);
                Invest = (PathNode)Invest.Parent;
            }

            return YoungestToOldest;
        }

        public List<Coordinate> TrainPath(BoardState board, Coordinate UnitPos, Coordinate Twitch, uint MoveCost, uint MoveHealth, Player Cont, List<PathNode> ToCheck, List<PathNode> Checked, List<Coordinate> Path)
        {
            uint PersonalMoveCost = MoveCost + 1;
            uint PersonalMoveHealth = MoveHealth;

            if ((board.getSquare(Twitch).Dep.OwnedBy != Cont) && (board.getSquare(Twitch).Dep.DepType == "Unit" || board.getSquare(Twitch).Dep.DepType == "Barracks" || board.getSquare(Twitch).Dep.DepType == "Base"))
            {
                //if an enemy dep type that can remove health
                //this prevents underflow errors as Health is a Uint
                if (PersonalMoveHealth >= board.getSquare(Twitch).Dep.Health)
                {
                    PersonalMoveHealth -= board.getSquare(Twitch).Dep.Health;
                }
                else
                {
                    PersonalMoveHealth = 0;
                }
            }

            //parameter for being a path member in the if statement
            //within range (an allowed cost) and positive health. if drops under zero it will overflow.
            if ((PersonalMoveCost <= StishBoard.Instance.GameMP) && ((PersonalMoveHealth <= board.getSquare(UnitPos).Dep.Health) && (PersonalMoveHealth != 0)))
            {
                //suitable to be created as a new node but not checked for suitability for the list system
                PathNode NodeToTest = new PathNode(null, PersonalMoveCost, PersonalMoveHealth, board, Twitch);

                if (SearchForPathNode(NodeToTest, Checked) == false)
                {
                    Coordinate SolidPos = new Coordinate(Twitch);
                    //comment below is no longer true but the changes are possible
                    //not already searched. note, this method allows there to be more than one node per square as long as the path is not 'essentially the same' which is a nice unintended consequence of this method.
                    //PathNode TestedNode = CreatePathNode(ToCheck, ToCheck[0], PersonalMoveCost, PersonalMoveHealth, board, SolidPos);

                    PathNode TestedNode = CreatePathNode(ToCheck, ToCheck[0], PersonalMoveCost, PersonalMoveHealth, board, SolidPos);
                    //is this the destination?
                    if ((Twitch.X == UnitPos.X) && (Twitch.Y == UnitPos.Y))
                    {              
                        //recursion to create a list of PathNode Parents
                        Path = FollowParents(TestedNode);
                        //returns a list of coordinates 'UnitPos --> To' for each individual step
                        ToCheck[0].RemoveChild(TestedNode);

                        return Path;
                    }
                }
            }

            //nothing of interest
            return null;
        }    

        //not void! returns a list of Pathnodes
        public List<Coordinate> FindPath(Coordinate UnitPos, Coordinate To, BoardState board)
        {
            //lists as we dont want a limit that would be given by an array
            List<Coordinate> Path = new List<Coordinate>();
            List<PathNode> ToCheck = new List<PathNode>();
            List<PathNode> Checked = new List<PathNode>();
            Coordinate Invest = new Coordinate();
            Coordinate Twitch = new Coordinate();
            uint MoveCost;
            uint MoveHealth = board.getSquare(UnitPos).Dep.Health;
            Player Cont = board.getSquare(UnitPos).Dep.OwnedBy;
            //MoveHealth must remain above 0 and MoveCost must remain below the maximum unit move distance

            //Start at To and end at UnitPos
            Invest.X = To.X;
            Invest.Y = To.Y;
            MoveCost = 0;

            //has to be cast here as the parent has to be given as null
            //adds this node to the list ToCheck
            CreatePathNode(ToCheck, null, MoveCost, MoveHealth, board, Invest);

            //Spreading from Invest
            while (ToCheck.Count != 0)
            {                                         
                for (int dir = 0; dir < 4; dir++)
                {
                    Invest = ToCheck[0].Position;
                    MoveCost = ToCheck[0].Cost;
                    MoveHealth = ToCheck[0].Health;

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
                    else if (dir == 3)
                    {
                        //left
                        Twitch.MoveLeft();
                    }

                    if (board.getSquare(Twitch) != null)
                    {
                        //the 2d distance this way around makes a lot of sense and is nicely restricting                       
                        if(UnitPos.Get2DDistance(Twitch) <= StishBoard.Instance.GameMP)
                        {
                            //tests to see if twitch is ontop of a friendly deployment but gives an exception for the destination which is the moving unit
                            if (((Twitch.X == UnitPos.X) && (Twitch.Y == UnitPos.Y))  || !(board.getSquare(Twitch).Owner == board.getSquare(UnitPos).Owner && board.getSquare(Twitch).Dep.DepType != "Empty"))
                            {
                                Coordinate SolidPos = new Coordinate(Twitch);
                                //make changes to Movecost and MoveHealth here
                                //trained equals true if this node is suitable and lands at the destination
                                List<Coordinate> Trained = TrainPath(board, UnitPos, SolidPos, MoveCost, MoveHealth, Cont, ToCheck, Checked, Path);
                                if (Trained != null)
                                {
                                    return Trained;
                                }
                            }                          
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
        public void SweepSearch(StishMiniMaxNode Parent, BoardState Now, Coordinate UnitPos, Player Side)
        {
            Coordinate Upper = new Coordinate();
            Coordinate Lower = new Coordinate();
            Coordinate Look = new Coordinate();
          
            //upper refers to the upper left corner (lower coordinate values)
            //assigns full values to the bounds and then changes them if they are inappropriate
            Upper.X = UnitPos.X - StishBoard.Instance.GameMP;
            Upper.Y = UnitPos.Y - StishBoard.Instance.GameMP;
            Lower.X = UnitPos.X + StishBoard.Instance.GameMP;
            Lower.Y = UnitPos.Y + StishBoard.Instance.GameMP;
            
            //checks if the unit is losing movement positions because it is too close to the edge of the board
            if (UnitPos.X < StishBoard.Instance.GameMP)
            {
                Upper.X = 0;
            }
            if (UnitPos.X > Now.BoardSizeX - StishBoard.Instance.GameMP)
            {
                Lower.X = Now.BoardSizeX;
            }
            if (UnitPos.Y < StishBoard.Instance.GameMP)
            {
                Upper.Y = 0;
            }
            if (UnitPos.Y > Now.BoardSizeY - StishBoard.Instance.GameMP)
            {
                Lower.Y = Now.BoardSizeY;
            }
         
            for (uint y = Upper.Y; y <= Lower.Y; y++)
            {
                for (uint x = Upper.X; x <= Lower.X; x++)
                {
                    Look.X = x;
                    Look.Y = y;
                    //stishboard.instance is okay here as this variable is global and is not dependant on a particular state
                    if (UnitPos.Get2DDistance(Look) <= StishBoard.Instance.GameMP)
                    {
                        //general squares around unit within range
                        if ((Now.getSquare(Look) != null) && !((Now.getSquare(Look).Owner) == Side && ((Now.getSquare(Look).Dep.DepType == "Unit") || (Now.getSquare(Look).Dep.DepType == "Base"))))
                        {
                            //the unit can legally move to any of these positions however the events of this action are not distinguished
                            //obstructions are not accounted for    
                            List<Coordinate> Path = FindPath(UnitPos, Look, Now);
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
            for (uint y = 0; y < Now.BoardSizeY; y++)
            {
                for (uint x = 0; x < Now.BoardSizeX; x++)
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
                Player PlayersTurn = Parent.Allegiance;
                /*
                if (Side.GetPlayerNum == "Player1")
                {
                    //opposite allegiance to it's parent
                    PlayersTurn = UnitBoardState.Player2;
                }
                else
                {
                    PlayersTurn = UnitBoardState.Player1;
                }
                */

                //Parent.Parent as "parent" is actually just an updated model of the "real" parent where no actions were taken
                StishMiniMaxNode UnitCaseNode = new StishMiniMaxNode(Parent.Parent, PlayersTurn, UnitBoardState);
                PredctionCount();

            }
            if (Side.Balance >= cost)
            {
                //can afford a barracks

                BoardState BarracksBoardState = new BoardState(Now);
                GameMaster.Instance.BuyBarracks(look, Side, BarracksBoardState);
                Player PlayersTurn = Parent.Allegiance;
                /*
                if (Side.GetPlayerNum == "Player1")
                {
                    //opposite allegiance to it's parent
                    PlayersTurn = BarracksBoardState.Player2;
                }
                else
                {
                    PlayersTurn = BarracksBoardState.Player1;
                }
                */

                //Parent.Parent as "parent" is actually just an updated model of the "real" parent where no actions were taken
                StishMiniMaxNode BarracksCaseNode = new StishMiniMaxNode(Parent.Parent, PlayersTurn, BarracksBoardState);
                PredctionCount();

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
                SweepSearch(Parent, Now, Invest, Side);
            }

        }


        public void GenerateChildren(StishMiniMaxNode NodeParent)
        {
            //parent argument will always contain "this" when called.
            Player OppositeAllegience;
            if (NodeParent.Allegiance.GetPlayerNum == "Player1")
            {
                OppositeAllegience = new Human(NodeParent.NodeBoardState.Player2);
            }
            else
            {
                OppositeAllegience = new Human(NodeParent.NodeBoardState.Player1);
            }

            //is a child of NodeParent as this is a turn spent doing nothing. the balance and MP are updated below and then this node is used as an example for others
            BoardState ParentBoardState = new BoardState(NodeParent.NodeBoardState);
            StishMiniMaxNode Parent = new StishMiniMaxNode(NodeParent, OppositeAllegience, ParentBoardState);

            m_TurnPredictionCount = 0;

            Parent.Allegiance.TurnBalance(Parent.NodeBoardState);
            Parent.Allegiance.MaxMP(Parent.NodeBoardState);     

            uint cost = BarracksCost(Parent.NodeBoardState, Parent.Allegiance);
            Coordinate Look = new Coordinate();
            for (uint y = 0; y < Parent.NodeBoardState.BoardSizeY; y++)
            {
                for (uint x = 0; x < Parent.NodeBoardState.BoardSizeX; x++)
                {
                    Look.Y = y;
                    Look.X = x;

                    TestSquare(Parent, Parent.NodeBoardState, Parent.Allegiance, Look, cost);
                }
            }
        }


    }
}
