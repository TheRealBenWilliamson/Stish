using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class StishMiniMaxNode : MiniMaxNode
    {
        private BoardState m_BoardState;

        //Alligiance is used to show whos turn this node is representing
        private Player m_Allegiance;

        BoardState AssignBoardState
        {
            set
            {
                m_BoardState = value;
            }
        }

        Player Allegiance
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

        public StishMiniMaxNode(TreeNode Parent, Player PlayersTurn) : base(Parent)
        {
            Allegiance = PlayersTurn;
        }

        private int P1BarracksHealth;
        private int P1UnitHealth;
        private int P1BaseHealth;
        private int P1BarracksNumber;
        private int P1Balance;

        private int P2BarracksHealth;
        private int P2UnitHealth;
        private int P2BaseHealth;
        private int P2BarracksNumber;
        private int P2Balance;       

        public void FindValue(TreeNode Parent, BoardState PassedBoardState)
        {
            //parent value, current barracks of both players, base health of both players, health of units of both players
            P1BarracksNumber = (int)PassedBoardState.Counting("Barracks", PassedBoardState.Player1, true);
            P1BarracksHealth = (int)PassedBoardState.Counting("Barracks", PassedBoardState.Player1, false);
            P1UnitHealth = (int)PassedBoardState.Counting("Unit", PassedBoardState.Player1, false);
            P1BaseHealth = (int)PassedBoardState.Counting("Base", PassedBoardState.Player1, false);           
            P1Balance = (int)PassedBoardState.Player1.Balance;

            P2BarracksNumber = (int)PassedBoardState.Counting("Barracks", PassedBoardState.Player2, true);
            P2BaseHealth = (int)PassedBoardState.Counting("Base", PassedBoardState.Player2, false);
            P2BarracksHealth = (int)PassedBoardState.Counting("Barracks", PassedBoardState.Player2, false);
            P2UnitHealth = (int)PassedBoardState.Counting("Unit", PassedBoardState.Player2, false);           
            P2Balance = (int)PassedBoardState.Player2.Balance;

            Value = (1 * (P1BarracksNumber - P2BarracksNumber)) + (1 * (P1BarracksHealth - P2BarracksHealth)) + (1 * (P1UnitHealth - P2UnitHealth)) + (1 * (P1BaseHealth - P2BaseHealth)) + (1 * (P1Balance - P2Balance));
        }

        StishMiniMaxNode(TreeNode Parent, BoardState PassedBoardState) : base(Parent)
        {
            m_BoardState = PassedBoardState;
            FindValue(Parent, PassedBoardState);
            
        }

        
    }
}
