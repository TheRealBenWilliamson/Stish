using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class StishMiniMaxNode : MiniMaxNode
    {
        private BoardState m_BoardState;

        StishMiniMaxNode(TreeNode Parent, BoardState PassedBoardState) : base(Parent)
        {
            m_BoardState = PassedBoardState;
            //parent value, current barracks of both players, base health of both players
        }

        
    }
}
