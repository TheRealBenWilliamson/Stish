using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class ForeSight
    {
        //the foresight class helps us build new nodes by predicting all possible moves from a node
        //these functions will be called by the node objects in order to tell the nodes, how to configure their children

        //predict will take a boardstate argument and then create a new board state and node for each possible future move

        public bool Connected(Coordinate MoveFrom, Coordinate MoveTo)
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
                

                if ((StishBoard.Instance.getSquare(Up) != null) && (StishBoard.Instance.getSquare(Up).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Up);
                }
                if ((StishBoard.Instance.getSquare(Right) != null) && (StishBoard.Instance.getSquare(Right).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Right);
                }
                if ((StishBoard.Instance.getSquare(Down) != null) && (StishBoard.Instance.getSquare(Down).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Down);
                }
                if ((StishBoard.Instance.getSquare(Left) != null) && (StishBoard.Instance.getSquare(Left).Dep.DepType == "Empty"))
                {
                    ToCheck.Add(Left);
                }
            }



            return Connect;
        }


        public bool Route(Coordinate From, Coordinate To)
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
        public void Sweep(Coordinate Check, Player Side)
        {
            int count = 0;

            Coordinate look = new Coordinate();
            for(uint x = 0; x < StishBoard.Instance.BoardSize; x++)
            {
                for(uint y = 0; y < StishBoard.Instance.BoardSize; y++)
                {
                    look.X = x;
                    look.Y = y;
                    if(Check.Get2DDistance(look) <= StishBoard.Instance.GameMP)
                    {
                        //general squares around unit within range
                        if((StishBoard.Instance.getSquare(look) != null) && !((StishBoard.Instance.getSquare(look).Dep.OwnedBy) == Side && ((StishBoard.Instance.getSquare(look).Dep.DepType == "Unit") || (StishBoard.Instance.getSquare(look).Dep.DepType == "Base"))))
                        {
                            //the unit can legallymove to any of these positions however the events of this action are not distinugished
                            count++;
                        }
                        
                    }
                }
            }         
        }

        public void BuyPossibility(Player Side)
        {
            int count = 0;
            int multiply = 1;

            Coordinate look = new Coordinate();
            for (uint x = 0; x < StishBoard.Instance.BoardSize; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSize; y++)
                {
                    look.X = x;
                    look.Y = y;

                    if ((StishBoard.Instance.getSquare(look).Dep.OwnedBy == Side) && (StishBoard.Instance.getSquare(look).Dep.DepType == "Barracks"))
                    {
                        multiply++;
                    }

                    if ((StishBoard.Instance.getSquare(look).Dep.OwnedBy == Side) && (StishBoard.Instance.getSquare(look).Dep.DepType == "Empty"))
                    {   
                        if(Side.Balance > 0)
                        {
                            count++;
                        }                        
                        if (Side.Balance >= 3 * multiply)
                        {
                            count++;
                        }
                    }
                }
            }
        }


        public void Predict(BoardState ParentBoard, Player Side)
        {
            //side correlates to Allegiance but i dont want to call them the smae thing in order to avoid confusion
            //things that determine a next turn: all positions around a unit (done once for each unit), buying a unit in any currently owned territory, buying a unit in any currently owned territory, absolutely nothing.
            //movement about a unit can be found using the "diamond" technique or the "jail bars" technique

            Coordinate look = new Coordinate();
            for (uint y = 0; y < StishBoard.Instance.BoardSize; y++)
            {
                for (uint x = 0; x < StishBoard.Instance.BoardSize; x++)
                {
                    look.Y = y;
                    look.X = x;
                    Sweep(look, Side);
                }
            }
            
        }
    }
}
