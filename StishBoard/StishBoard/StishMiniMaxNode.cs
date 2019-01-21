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

        public BoardState NodeBoardState
        {
            set
            {
                m_BoardState = value;
            }
            get
            {
                return m_BoardState;
            }
        }

        public StishMiniMaxNode(Player PlayersTurn)
        {
            Allegiance = PlayersTurn;
        }

        public StishMiniMaxNode(TreeNode Parent, Player PlayersTurn) : base(Parent)
        {
            Allegiance = PlayersTurn;
        }

        public StishMiniMaxNode(TreeNode Parent, Player PlayersTurn, BoardState PassedBoardState) : base(Parent)
        {
            m_BoardState = PassedBoardState;
            FindValue(Parent, PassedBoardState);
            Allegiance = PlayersTurn;
        }

        private uint P1BarracksHealth;
        private uint P1UnitHealth;
        private uint P1BaseHealth;
        private uint P1BarracksNumber;
        private uint P1Balance;

        private uint P2BarracksHealth;
        private uint P2UnitHealth;
        private uint P2BaseHealth;
        private uint P2BarracksNumber;
        private uint P2Balance;       

        public void FindValue(TreeNode Parent, BoardState PassedBoardState)
        {
            //parent value, current barracks of both players, base health of both players, health of units of both players
            P1BarracksNumber = PassedBoardState.Counting("Barracks", PassedBoardState.Player1, true);
            P1BarracksHealth = PassedBoardState.Counting("Barracks", PassedBoardState.Player1, false);
            P1UnitHealth = PassedBoardState.Counting("Unit", PassedBoardState.Player1, false);
            P1BaseHealth = PassedBoardState.Counting("Base", PassedBoardState.Player1, false);           
            P1Balance = PassedBoardState.Player1.Balance;

            P2BarracksNumber = PassedBoardState.Counting("Barracks", PassedBoardState.Player2, true);
            P2BaseHealth = PassedBoardState.Counting("Base", PassedBoardState.Player2, false);
            P2BarracksHealth = PassedBoardState.Counting("Barracks", PassedBoardState.Player2, false);
            P2UnitHealth = PassedBoardState.Counting("Unit", PassedBoardState.Player2, false);           
            P2Balance = PassedBoardState.Player2.Balance;

            Value = (1 * ((int)P1BarracksNumber - (int)P2BarracksNumber)) + (1 * ((int)P1BarracksHealth - (int)P2BarracksHealth)) + (1 * ((int)P1UnitHealth - (int)P2UnitHealth)) + (1 * ((int)P1BaseHealth - (int)P2BaseHealth)) + (1 * ((int)P1Balance - (int)P2Balance));
        }

        

        
    }
}
