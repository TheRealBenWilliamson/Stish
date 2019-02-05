using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Computer : Player
    {
        public Computer(PlayerNumber PN, PlayerType PT, BoardState Board) : base(PN, PT, Board)
        {

        }     

        public Computer(Computer Cpu) : base(Cpu)
        {

        }

        public override void MakeMove()
        {
            StishMiniMaxNode GameNode = new StishMiniMaxNode(null, StishBoard.Instance.Player1);
            GameNode.NodeBoardState = new BoardState(StishBoard.Instance);

            //int evaluation = MiniMaxMind.Instance.BuildABTree(GameNode, 4, int.MinValue, int.MaxValue, 1);
            MiniMaxMind.Instance.RecBuildMMTree(GameNode, 1);
            int evaluation = MiniMaxMind.Instance.TraverseTree(GameNode, 1, -1);

            //ForeSight.Instance.PredctionCount();

            //StishBoard.Instance.getBoard = new BoardState(GameNode.BestChild.NodeBoardState.getBoard);

        }


        public void DetermineMove()
        {

        }
    }
}
