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
        private StishMiniMaxNode m_BestChild;
        private bool m_AlreadyGen;

        public StishMiniMaxNode BestChild
        {
            set
            {
                m_BestChild = value;
            }
            get
            {
                return m_BestChild;
            }
        }

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

        public bool AlreadyGen
        {
            set
            {
                m_AlreadyGen = value;
            }
            get
            {
                return m_AlreadyGen;
            }
        }

        public StishMiniMaxNode(Player PlayersTurn)
        {
            Allegiance = PlayersTurn;
            Inherit_Allegiance();
            m_AlreadyGen = false;
        }

        public StishMiniMaxNode(TreeNode Parent, Player PlayersTurn) : base(Parent)
        {
            Allegiance = PlayersTurn;
            Inherit_Allegiance();
            m_AlreadyGen = false;
        }

        public StishMiniMaxNode(TreeNode Parent, Player PlayersTurn, BoardState PassedBoardState) : base(Parent)
        {
            m_BoardState = PassedBoardState;
            //FindValue(Parent, PassedBoardState);
            Allegiance = PlayersTurn;
            Inherit_Allegiance();
            m_AlreadyGen = false;
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

        private double m_NegaMaxValue;

        public double NegaMaxValue
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

        private uint MeBarracksHealth;
        private uint MeUnitHealth;
        private uint MeBaseHealth;
        private uint MeBarracksNumber;
        private uint MeBalance;

        private double UnitProximity;

        private uint OpBarracksHealth;
        private uint OpUnitHealth;
        private uint OpBaseHealth;
        private uint OpBarracksNumber;
        private uint OpBalance;

        public double FindValue(TreeNode Parent, BoardState PassedBoardState, Player ThisPlayer)
        {
            Player Me;
            Player Oppenent;
            if (ThisPlayer.GetPlayerNum == "Player1")
            {
                Me = PassedBoardState.Player1;
                Oppenent = PassedBoardState.Player2;

                MeBalance = PassedBoardState.Player1.Balance;
                OpBalance = PassedBoardState.Player2.Balance;
            }
            else
            {
                Me = PassedBoardState.Player2;
                Oppenent = PassedBoardState.Player1;

                MeBalance = PassedBoardState.Player2.Balance;
                OpBalance = PassedBoardState.Player1.Balance;
            }

            //parent value, current barracks of both players, base health of both players, health of units of both players
            MeBarracksNumber = PassedBoardState.Counting("Barracks", Me, true);
            MeBarracksHealth = PassedBoardState.Counting("Barracks", Me, false);
            MeUnitHealth = PassedBoardState.Counting("Unit", Me, false);
            MeBaseHealth = PassedBoardState.Counting("Base", Me, false);

            UnitProximity = PassedBoardState.DistanceValuation(Me, Oppenent);

            OpBarracksNumber = PassedBoardState.Counting("Barracks", Oppenent, true);
            OpBaseHealth = PassedBoardState.Counting("Base", Oppenent, false);
            OpBarracksHealth = PassedBoardState.Counting("Barracks", Oppenent, false);
            OpUnitHealth = PassedBoardState.Counting("Unit", Oppenent, false);   

            Value = (20 * UnitProximity) + (10 * ((int)MeBarracksNumber - (int)OpBarracksNumber)) + (6 * ((int)MeBarracksHealth - (int)OpBarracksHealth)) + (3 * ((int)MeUnitHealth - (int)OpUnitHealth)) + (6 * ((int)MeBaseHealth - (int)OpBaseHealth)) + (1 * ((int)MeBalance - (int)OpBalance));

            if (MeBaseHealth < 1)
            {
                Value = int.MinValue;
            }
            if (OpBaseHealth < 1)
            {
                Value = int.MaxValue;
            }

            return Value;
        }

       

    }
}
