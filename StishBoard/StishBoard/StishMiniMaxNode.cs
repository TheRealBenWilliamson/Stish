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
                Inherit_Allegiance();
            }
            get
            {
                return m_BoardState;
            }
        }

        public StishMiniMaxNode(Player PlayersTurn)
        {
            Allegiance = PlayersTurn;
            Inherit_Allegiance();
        }

        public StishMiniMaxNode(TreeNode Parent, Player PlayersTurn) : base(Parent)
        {
            Allegiance = PlayersTurn;
            Inherit_Allegiance();
        }

        public StishMiniMaxNode(TreeNode Parent, Player PlayersTurn, BoardState PassedBoardState) : base(Parent)
        {
            m_BoardState = PassedBoardState;
            //FindValue(Parent, PassedBoardState);
            Allegiance = PlayersTurn;
            Inherit_Allegiance();
        }

        public void Inherit_Allegiance()
        {
            if (m_BoardState != null)
            {
                if (Allegiance.GetPlayerNum == "Player1")
                {
                    Allegiance = m_BoardState.Player1;
                }
                else
                {
                    Allegiance = m_BoardState.Player2;
                }
            }
        }

        private int m_NegaMaxValue;

        public int NegaMaxValue
        {
            get
            {
                return m_NegaMaxValue;
            }
            set
            {
                m_NegaMaxValue = value;
            }
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

        private uint MeBarracksHealth;
        private uint MeUnitHealth;
        private uint MeBaseHealth;
        private uint MeBarracksNumber;
        private uint MeBalance;

        private uint OpBarracksHealth;
        private uint OpUnitHealth;
        private uint OpBaseHealth;
        private uint OpBarracksNumber;
        private uint OpBalance;

        public int FindValue(TreeNode Parent, BoardState PassedBoardState, Player ThisPlayer)
        {
            Player Me;
            Player Oppenent;
            if (ThisPlayer.GetPlayerNum == "Player1")
            {
                Me = PassedBoardState.Player1;
                Oppenent = PassedBoardState.Player2;
            }
            else
            {
                Me = PassedBoardState.Player2;
                Oppenent = PassedBoardState.Player1;
            }

            //parent value, current barracks of both players, base health of both players, health of units of both players
            MeBarracksNumber = PassedBoardState.Counting("Barracks", Me, true);
            MeBarracksHealth = PassedBoardState.Counting("Barracks", Me, false);
            MeUnitHealth = PassedBoardState.Counting("Unit", Me, false);
            MeBaseHealth = PassedBoardState.Counting("Base", Me, false);
            MeBalance = PassedBoardState.Player1.Balance;

            OpBarracksNumber = PassedBoardState.Counting("Barracks", Oppenent, true);
            OpBaseHealth = PassedBoardState.Counting("Base", Oppenent, false);
            OpBarracksHealth = PassedBoardState.Counting("Barracks", Oppenent, false);
            OpUnitHealth = PassedBoardState.Counting("Unit", Oppenent, false);
            OpBalance = PassedBoardState.Player2.Balance;

            Value = (1 * ((int)MeBarracksNumber - (int)OpBarracksNumber)) + (1 * ((int)MeBarracksHealth - (int)OpBarracksHealth)) + (1 * ((int)MeUnitHealth - (int)OpUnitHealth)) + (1 * ((int)MeBaseHealth - (int)OpBaseHealth)) + (1 * ((int)MeBalance - (int)OpBalance));
            return Value;
        }

        public int GenericFindValue(TreeNode Parent, BoardState PassedBoardState)
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
            return Value;
        }






    }
}
