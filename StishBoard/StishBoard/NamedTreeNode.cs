using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class NamedTreeNode : TreeNode
    {
        private string Name;

        public NamedTreeNode(string CalledName)
        {
            Name = CalledName;
        }
        public NamedTreeNode(string CalledName, TreeNode Parent) : base(Parent)
        {
            Name = CalledName;
        }
    }
}
