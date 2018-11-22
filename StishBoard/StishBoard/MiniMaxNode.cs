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
        MiniMaxNode(TreeNode Parent, StishBoard CurrentBoard) : base(Parent)
        {
            m_boardState = CurrentBoard.GetBoardState();
        }

        public void CheckChildren()
        {
            //without alpha-beta pruning, i would call a CheckChildren function onto all of this node's children and then inherit to correct values into this node  
            //this would create a function that begins from the top, works its way down the tree and then builds values onto nodes as it works it's way back up
            //unfortunately this would not perform alpha-beta pruning and would also call a function from inside itself
        }
    }
}
