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

        public void Sweep(Coordinate Check, Player Side)
        {
            Coordinate Sweep = new Coordinate();
            uint Shift = 5;
            //uint Shift = StishBoard.Instance.GameMP;
            Sweep.X = Check.X - Shift;
            Sweep.Y = Check.Y;

            for(int CurrentShift = 0; CurrentShift == (int)Shift; CurrentShift++)
            {
                for(int drop = 0; drop < CurrentShift * 2; drop++)
                {
                    Sweep.MoveDown();
                }
                Sweep.MoveRight();
            }
            for (int CurrentShift = -1 * (int)Shift; CurrentShift <= 0; CurrentShift++)
            {
                for (int drop = (int)Shift; drop < CurrentShift * -2; drop--)
                {
                    Sweep.MoveDown();
                }
                Sweep.MoveRight();
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
