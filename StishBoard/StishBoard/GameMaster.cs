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

        StishBoard board = StishBoard.Instance;

        //coordinates are uints so anything less than 0 will overflow
        public bool OnBoard(Coordinate num)
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

    }
}
