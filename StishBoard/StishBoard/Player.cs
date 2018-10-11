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

        //by now a board should already have been created. StishBoard.Instance allows us to get a reference to the existing board.
        StishBoard board = StishBoard.Instance;

        //using enums because it is an easy way to show exclusive possible properties of something. eg an enum for the day of the week would be appropriate because there are seven set possibilities yet the day could only be one of them at once.
        //why didnt we use enums for deployemnt?
        public enum PlayerNumber { Player1, Player2};
        public enum PlayerType { Human, Computer};

        protected PlayerNumber playerNumber;
        protected uint balance;

        public uint CursorX = 5;
        public uint Cursory = 5;

        protected Base homeBase;

        protected Player(PlayerNumber PN)
        {
            playerNumber = PN;
            //balance can be changed for testing and balancing
            balance = 5;

            //homeBase = new Base();
            if (playerNumber == PlayerNumber.Player1)
            {
                new Base(this,board.getSquare(5, 9), 20);
                for (uint y = 8; y < 11; y++)
                {
                    for (uint x = 3; x < 8; x++)
                    {
                        board.getSquare(x, y).Owner = this;
                    }
                }
                
                
            }
            else
            {
                new Base(this, board.getSquare(5, 1), 20);
                for (uint y = 0; y < 3; y++)
                {
                    for (uint x = 3; x < 8; x++)
                    {
                        board.getSquare(x, y).Owner = this;
                    }
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
            for (uint y = 0; y < 11; y++)
            {
                for (uint x = 0; x < 11; x++)
                {
                    if((board.getSquare(x, y).Dep.DepType == "Barracks" || board.getSquare(x, y).Dep.DepType == "Base") && board.getSquare(x, y).Dep.OwnedBy == this)
                    {
                        balance ++ ;
                    }           
                }
            }
        }        


        public void MaxMP()
        {
            //this fuction is run at the start of a turn and sets all units that belong to this player to the max MP.

            for (uint y = 0; y < 11; y++)
            {
                for (uint x = 0; x < 11; x++)
                {
                    Square ThisSquare = board.getSquare(x, y);
                    if ((ThisSquare.Owner == this) && (ThisSquare.Dep.DepType == "Unit"))
                    {
                        //This number is subject to change throughout testing and balancing
                        ThisSquare.Dep.MP = 3;
                    }
                }
            }
        }


    }
}
