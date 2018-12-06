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
