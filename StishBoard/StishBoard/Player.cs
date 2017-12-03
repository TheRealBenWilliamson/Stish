using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Player
    {
        private static Player Player1Instance;
        private static Player Player2Instance;

        public enum PlayerNumber { Player1, Player2};
        public enum PlayerType { Human, Computer};

        public static Player PlayerFactory(PlayerNumber PN, PlayerType PT)
        {
            return new Player();
        }
    }
}
