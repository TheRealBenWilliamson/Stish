using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class PathNode : TreeNode
    {
        private uint m_Cost;
        private uint m_Health; 
        private BoardState World;
        private Coordinate Pos;


        public PathNode(TreeNode Parent) : base(Parent)
        {
            
        }

        public PathNode(TreeNode Parent, uint CalledCost, uint CalledHealth, BoardState CalledWorld, Coordinate CalledPos) : base(Parent)
        {
            this.Parent = Parent;
            Cost = CalledCost;
            Health = CalledHealth;
            World = CalledWorld;
            Pos = CalledPos;
        }

        public Coordinate Position
        {
            get
            {
                return Pos;
            }
            set
            {
                Pos = value;
            }
        }

        public uint Cost
        {
            get
            {
                return m_Cost;
            }
            set
            {
                m_Cost = value;
            }
        }

        public uint Health
        {
            get
            {
                return m_Health;
            }
            set
            {
                m_Health = value;
            }
        }
    }
}
