using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public abstract class Player
    {

        //private static Player Player1Instance;
        //private static Player Player2Instance;
        //dont know where to use these

        //by now a StishBoard.Instance should already have been created. StishBoard.Instance allows us to get a reference to the existing StishBoard.Instance.

        //using enums because it is an easy way to show exclusive possible properties of something. eg an enum for the day of the week would be appropriate because there are seven set possibilities yet the day could only be one of them at once.
        //why didnt we use enums for deployemnt?
        public enum PlayerNumber { Player1, Player2};
        public enum PlayerType { Human, Computer};

        protected PlayerNumber playerNumber;
        protected uint balance;

        public uint CursorX = 5;
        public uint Cursory = 5;
        private uint baseX = 0;
        private uint baseY = 0;        

        public uint BaseX
        {
            get
            {
                return baseX;
            }
            set
            {
                baseX = value;
            }
        }

        public uint BaseY
        {
            get
            {
                return baseY;
            }
            set
            {
                baseY = value;
            }
        }

        protected Base homeBase;

        protected Player(PlayerNumber PN)
        {
            playerNumber = PN;
            //balance can be changed for testing and balancing
            balance = 5;

            //homeBase = new Base();
            if (playerNumber == PlayerNumber.Player1)
            {
                BaseX = (StishBoard.Instance.BoardSize) / 2;
                BaseY = StishBoard.Instance.BoardSize - 2;                                      
            }
            else
            {
                BaseX = (StishBoard.Instance.BoardSize) / 2;
                BaseY = 1;                          
            }
            Coordinate ThisCo = new Coordinate(BaseX, BaseY);
            new Base(this, StishBoard.Instance.getSquare(ThisCo), 20);
            for (uint y = BaseY - 1; y < BaseY + 2; y++)
            {
                for (uint x = BaseX - 1; x < BaseX + 2; x++)
                {
                    ThisCo.X = x;
                    ThisCo.Y = y;
                    StishBoard.Instance.getSquare(ThisCo).Owner = this;
                }
            }
        }        

        public string GetPlayerNum
        {
            get
            {
                return playerNumber.ToString();
            }
        }


        public ConsoleColor GetRenderColour()
        {
            ConsoleColor retval;

            switch (playerNumber)
            {
                case PlayerNumber.Player1:
                    retval = ConsoleColor.DarkRed;
                    break;
                case PlayerNumber.Player2:
                    retval = ConsoleColor.Blue;
                    break;
                default:
                    retval = ConsoleColor.White;
                    break;
            }

            return retval;
        }

        //a function that can be called upon to create a player of a given type. the player is aslo assigned a number to represent them so we can distinguish between to players of the same type.
        public static Player PlayerFactory(PlayerNumber PN, PlayerType PT)
        {
            Player creation = null;

            //creates either a human or computer object and tells it the which player number it has.
            if (PT == PlayerType.Human)
            {
                creation = new Human(PN);
            }
            if (PT == PlayerType.Computer)
            {
                creation = new Computer(PN);
            }

            return creation;
        }

        public uint Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }        

        public virtual void MakeMove()
        {
            
        }

        public void TurnBalance()
        {
            Coordinate ThisCo = new Coordinate();
            for (uint y = 0; y < StishBoard.Instance.BoardSize; y++)
            {
                for (uint x = 0; x < StishBoard.Instance.BoardSize; x++)
                {
                    ThisCo.X = x;
                    ThisCo.Y = y;
                    if((StishBoard.Instance.getSquare(ThisCo).Dep.DepType == "Barracks" || StishBoard.Instance.getSquare(ThisCo).Dep.DepType == "Base") && StishBoard.Instance.getSquare(ThisCo).Dep.OwnedBy == this)
                    {
                        balance ++ ;
                    }           
                }
            }
        }        


        public void MaxMP()
        {
            //this fuction is run at the start of a turn and sets all units that belong to this player to the max MP.
            Coordinate ThisCo = new Coordinate();
            for (uint y = 0; y < StishBoard.Instance.BoardSize; y++)
            {
                for (uint x = 0; x < StishBoard.Instance.BoardSize; x++)
                {
                    ThisCo.X = x;
                    ThisCo.Y = y;
                    Square ThisSquare = StishBoard.Instance.getSquare(ThisCo);
                    if ((ThisSquare.Owner == this) && (ThisSquare.Dep.DepType == "Unit"))
                    {
                        //This number is subject to change throughout testing and balancing
                        ThisSquare.Dep.MP = StishBoard.Instance.GameMP;
                        ThisSquare.Dep.JustCreated = false;
                    }
                }
            }
        }


    }
}
