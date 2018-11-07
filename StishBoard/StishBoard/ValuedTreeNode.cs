using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class ValuedTreeNode : TreeNode
    {
        private int Alpha;
        private int Beta;

        public ValuedTreeNode(int CalledAlpha, int CalledBeta, TreeNode Parent) : base(Parent)
        {
            Alpha = CalledAlpha;
            Beta = CalledBeta;
        }

        public List<TreeNode> PathNodes()
        {
            //runs up the tree until the rootnode (a node with no parent) is found, adding all pathnodes to a list
            List<TreeNode> MyPath = new List<TreeNode>();
            return MyPath;
        }

        public void Assign()
        {
            //calls the PathNode function to find a list with the reverse chronological order of turns taken from the present to this node. this should be a leaf node
            //this function will assign alpha and beta values to......
        }

    }
}
