using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class ValuedTreeNode : TreeNode
    {
        protected int m_Alpha;
        protected int m_Beta;
        protected double m_Value;

        public ValuedTreeNode(TreeNode Parent) : base(Parent)
        {
            m_Alpha = int.MinValue;
            m_Beta = int.MaxValue;
        }

        public ValuedTreeNode()
        {
            m_Alpha = int.MinValue;
            m_Beta = int.MaxValue;
        }

        public void AlphaBeta()
        {
            //starts at the root node and begins using Alpha-Beta pruning to decide which leaf nodes need to have the Evaluate() function called on them
            //WOULD (there is an error) will call the CheckChildren() function on the root node to begin a chain of other nodes also calling this function
        }

        public double Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

    }
}
