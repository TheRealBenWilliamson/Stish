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

        protected Base homeBase;

        protected Player(PlayerNumber PN)
        {
            playerNumber = PN;
            //balance can be changed for testing and balancing
            balance = 10;

            //homeBase = new Base();
            if (playerNumber == PlayerNumber.Player1)
            {
                new Base(this,board.getSquare(5, 9));
            }
            else
            {
                new Base(this, board.getSquare(5, 1));
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

        protected void TurnBalance()
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


        protected void PlayerCrane(uint FromX , uint FromY, uint ToX, uint ToY)
        {
            //this method will be used by any object derived from the player class. it will allow a player to munipulate deployment positions on the board hence letting them move move a unit or buy/place a deployment.

            Square From = board.getSquare(FromX, FromY);
            Square To = board.getSquare(ToX, ToY);

            From.Owner = this;
            To.Owner = this;
                
            To.Dep = From.Dep;
            From.Dep = new Empty();
                       
        }


        protected void TerritoryMapping(uint FromX, uint FromY, uint ToX, uint ToY)
        {
            //changes the territory value of every square in two dimention! because of this, all movement must be one dimentional for this fuction to work properly!

            Square NewTerritory;
           
            if((ToX > FromX) || (ToY > FromY))
            {
                //if the unit is moveing right or down then the co-ordinates need to increase
                for (uint y = FromY; y <= ToY; y++)
                {
                    for (uint x = FromX; x <= ToX; x++)
                    {
                        NewTerritory = board.getSquare(x, y);
                        NewTerritory.Owner = this;
                    }
                }
            }
            else
            {
                //if the unit is moveing left or up then the co-ordinates need to decrease
                for (uint y = FromY; y >= ToY; y--)
                {
                    for (uint x = FromX; x >= ToX; x--)
                    {
                        NewTerritory = board.getSquare(x, y);
                        NewTerritory.Owner = this;
                    }
                }
            }

            
        }

        protected bool MoveObstructed(uint FromX, uint FromY, uint ToX, uint ToY)
        {
            //will function similarly to Terriroty mapping however it will return a boolean if any square in the movement path is not empty
            bool obstructed = false;

            //x and y values are increased or decreased by 1 so that it does not read the square that the unit is on as an obstruction. it also does not read the destination incase the unit is attempting to attack another unit

            if ((ToX > FromX) || (ToY > FromY))
            {
                //if the unit is moveing right or down then the co-ordinates need to increase
                if (ToX != FromX)
                {
                    //x value has changed
                    for (uint x = FromX + 1; x < ToX; x++)
                    {
                        if (board.getSquare(x, FromY).Dep.DepType != "Empty")
                        {
                            obstructed = true;
                        }
                    }
                }
                else if (ToY != FromY)
                {
                    //y value has changed
                    for (uint y = FromY + 1; y < ToY; y++)
                    {
                        if (board.getSquare(FromX, y).Dep.DepType != "Empty")
                        {
                            obstructed = true;
                        }
                    }
                }               
            }
            else if ((ToX < FromX) || (ToY < FromY))
            {
                //if the unit is moveing left or up then the co-ordinates need to decrease
                if (ToX != FromX)
                {
                    //x value has changed
                    for (uint x = FromX - 1; x > ToX; x--)
                    {
                        if (board.getSquare(x, FromY).Dep.DepType != "Empty")
                        {
                            obstructed = true;
                        }
                    }
                }
                else if (ToY != FromY)
                {
                    //y value has changed
                    for (uint y = FromY - 1; y > ToY; y--)
                    {
                        if (board.getSquare(FromX, y).Dep.DepType != "Empty")
                        {
                            obstructed = true;
                        }
                    }
                }
            }
          

            return obstructed;
        }


        protected void MaxMP()
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
                        ThisSquare.Dep.MP = 10;
                    }
                }
            }
        }


    }
}
