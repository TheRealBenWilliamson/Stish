using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class MaxNode : ValuedTreeNode
    {
        MaxNode(TreeNode Parent) : base(Parent)
        {

        }

        public void CheckChildren()
        {
            //without alpha-beta pruning, i would call a CheckChildren function onto all of this node's children and then inherit to correct values into this node  
            //this would create a function that begins from the top, works its way down the tree and then builds values onto nodes as it works it's way back up
            //unfortunately this would not perform alpha-beta pruning and would also call a function from inside itself
        }
    }
}
