using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class TreeNode
    {
        private TreeNode m_ParentNode;
        private List<TreeNode> m_ChildrenNodes = new List<TreeNode>();

        //Alligiance is used to show whos turn this node is representing
        private Player m_Allegiance;
       
        public TreeNode Parent
        {
            get
            {
                return m_ParentNode;
            }
            set
            {
                m_ParentNode = value;
            }
        }

        public TreeNode()
        {

        }
        public TreeNode(TreeNode ParentNode)
        {
            m_ParentNode = ParentNode;
            if (m_ParentNode != null)
            {
                ParentNode.AddChild(this);
            }          
        }

        public Player Allegiance
        {
            get
            {
                return m_Allegiance;
            }
            set
            {
                m_Allegiance = value;
            }
        }

        public void AddChild(TreeNode ChildNode)
        {
            m_ChildrenNodes.Add(ChildNode);
            ChildNode.Parent = this;
        }

        public TreeNode GetParent()
        {
            return m_ParentNode;
        }
        public TreeNode GetChild(int index)
        {
            return m_ChildrenNodes[index];
        }

        public void RemoveChild(TreeNode KillChild)
        {
            m_ChildrenNodes.Remove(KillChild);
        }
        public void Remove()
        {
            m_ParentNode.RemoveChild(this);
            m_ParentNode = null;
        }

        public int CountChildren()
        {
            return m_ChildrenNodes.Count;
        }

        public int FindDepth()
        {
            //a depth of zero is at the rootnode. addition each layer down
            int Depth = 0;
            TreeNode Invest = this;
            while (Invest.Parent != null)
            {
                Invest = Invest.Parent;
                Depth++;
            }
            return Depth;
        }

        public List<TreeNode> PathNodes()
        {
            //runs up the tree until the rootnode (a node with no parent) is found, adding all pathnodes to a list
            List<TreeNode> MyPath = new List<TreeNode>();
            TreeNode inspect = this;
            while (inspect != null)
            {
                MyPath.Add(inspect);
                inspect = inspect.GetParent();
            }
            return MyPath;
        }

    }
}
