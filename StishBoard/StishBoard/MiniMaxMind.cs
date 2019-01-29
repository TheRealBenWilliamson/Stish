using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class MiniMaxMind
    {

        private static MiniMaxMind instance;

        public static MiniMaxMind Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MiniMaxMind();
                }
                return instance;
            }
        }

        public void Visit()
        {

        }

        public void TraverseTree(StishMiniMaxNode CurrentNode, int DepthCount)
        {
            //Depth count is given as 0 when called at root

            if (CurrentNode == null || DepthCount >= 5)
            {
                //the number 3 is a variable to be changed as sight increases
                return;
            }

            Visit();

            for (int index = 0; index < CurrentNode.CountChildren(); index++)
            {
                RecBuildMMTree((StishMiniMaxNode)CurrentNode.GetChild(index), DepthCount + 1);
            }
        }

        public void RecBuildMMTree(StishMiniMaxNode CurrentNode, int DepthCount)
        {
            //Depth count is given as 0 when called at root

            if(CurrentNode == null || DepthCount >= 4)
            {
                //the number 3 is a variable to be changed as sight increases
                return;
            }

            Visit();

            ForeSight.Instance.GenerateChildren(CurrentNode);

            for (int index = 0; index < CurrentNode.CountChildren(); index++)
            {              
                RecBuildMMTree((StishMiniMaxNode)CurrentNode.GetChild(index), DepthCount + 1);
            }
        }

        public void BuildMMTree(StishMiniMaxNode RootNode, int DepthLimit)
        {
            StishMiniMaxNode InvestNode;
            List<StishMiniMaxNode> GenQueue = new List<StishMiniMaxNode>();
            GenQueue.Add(RootNode);

            while(GenQueue.Count > 0)
            {
                InvestNode = GenQueue[0];
                ForeSight.Instance.GenerateChildren(InvestNode);
                //creates the depth layer but doesnt generate from them
                if (InvestNode.FindDepth() + 1 < DepthLimit)
                {                  
                    for (int index = 0; index < InvestNode.CountChildren(); index++)
                    {
                        GenQueue.Add((StishMiniMaxNode)InvestNode.GetChild(index));
                    }
                }

                GenQueue.Remove(InvestNode);
            }
        }
    }
}
