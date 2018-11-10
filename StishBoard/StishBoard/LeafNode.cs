using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class LeafNode : ValuedTreeNode
    {
        LeafNode(TreeNode Parent) : base(Parent)
        {

        }

        public void Evaluate()
        {
            //should be called on a leaf node (neither alpha nor beta but has a value)
            //calls the PathNode function to find a list with the reverse chronological order of turns taken from the present to this node.
            //assigns a value to this node that represents how good this sequence of moves is for this player (AI)
            List<TreeNode> MyPath = new List<TreeNode>();
            MyPath = this.PathNodes();
        }
    }
}
