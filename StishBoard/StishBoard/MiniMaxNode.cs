using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class MiniMaxNode : ValuedTreeNode
    {
        private BoardState m_boardState;

        //need to re-work the arguments as we cant just pass through the current state, we need to add the next possible states.
        protected MiniMaxNode(TreeNode Parent, StishBoard CurrentBoard) : base(Parent)
        {
            m_boardState = CurrentBoard.GetBoardState();
        }
        protected MiniMaxNode(TreeNode Parent) : base(Parent)
        {

        }
        protected MiniMaxNode()
        {

        }


    }
}
