using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public abstract class Player
    {
        // public or protected publicity?

        private static Player Player1Instance;
        private static Player Player2Instance;
        //dont know where to use these

        //by now a board should already have been created. StishBoard.Instance allows us to get a reference to the existing board.
        StishBoard board = StishBoard.Instance;

        //using enums because it is an easy way to show exclusive possible properties of something. eg an enum for the day of the week would be appropriate because there are seven set possibilities yet the day could only be one of them at once.
        //why didnt we use enums for deployemnt?
        public enum PlayerNumber { Player1, Player2};
        public enum PlayerType { Human, Computer};

        protected PlayerNumber playerNumber;
        protected uint balance;

        protected Player(PlayerNumber PN)
        {
            playerNumber = PN;
            balance = 0;
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


        protected void PlayerCrane(int FromX , int FromY, int ToX, int ToY)
        {
            //this method will be used by any object derived from the player class. it will allow a player to munipulate deployment positions on the board hence letting them move move a unit or buy/place a deployment.

            //if a deployement is being bought then it has no original position. if the crane is given negative co-ordinates then it will not try to take it from any square on the board.
            if (FromX < 0 && FromY < 0)
            {
                Square To = board.getSquare(ToX, ToY);
            }
            else
            {
                Square From = board.getSquare(FromX, FromY);
                Square To = board.getSquare(ToX, ToY);
                
                To.Dep = From.Dep;
                From.Dep = new Empty();
            }
            


        }

    }
}
