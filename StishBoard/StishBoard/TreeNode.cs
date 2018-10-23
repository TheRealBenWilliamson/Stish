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

        private TreeNode Parent
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
            ParentNode.AddChild(this);
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

        private void RemoveChild(TreeNode KillChild)
        {
            m_ChildrenNodes.Remove(KillChild);            
        }
        public void Remove()
        {
            m_ParentNode.RemoveChild(this);
            m_ParentNode = null;
        }
    }
}
