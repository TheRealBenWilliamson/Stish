using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Movement
    {
        private Player player;

        public Movement(Player p)
        {
            player = p;
        }

        StishBoard board = StishBoard.Instance;

        protected void PlayerCrane(uint FromX, uint FromY, uint ToX, uint ToY)
        {
            //this method will be used by any object derived from the player class. it will allow a player to munipulate deployment positions on the board hence letting them move move a unit or buy/place a deployment.

            Square From = board.getSquare(FromX, FromY);
            Square To = board.getSquare(ToX, ToY);

            From.Owner = player;
            To.Owner = player;

            To.Dep = From.Dep;
            From.Dep = new Empty();

        }


        protected void TerritoryMapping(uint FromX, uint FromY, uint ToX, uint ToY)
        {
            //changes the territory value of every square in two dimention! because of this, all movement must be one dimentional for this fuction to work properly!

            Square NewTerritory;

            if ((ToX > FromX) || (ToY > FromY))
            {
                //if the unit is moveing right or down then the co-ordinates need to increase
                for (uint y = FromY; y <= ToY; y++)
                {
                    for (uint x = FromX; x <= ToX; x++)
                    {
                        NewTerritory = board.getSquare(x, y);
                        NewTerritory.Owner = player;
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
                        NewTerritory.Owner = player;
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


        public void HumanMoveUnit()
        {
            //will use the user interface to ask the player which unit they want to move and where to. it will call MoveUnit(from , to) in Player class to make from's position empty. it will set to's position to a unit or barracks.
            //need to make sure there is a friendly Unit at the 'from' posititon
            bool ValidUnit = false;
            do
            {

                try
                {
                    Console.WriteLine("\nWhat Unit would you like to move? \n X co-ordinate:");
                    uint UnitX = UInt32.Parse(Console.ReadLine());
                    Console.WriteLine(" Y co-ordinate:");
                    uint UnitY = UInt32.Parse(Console.ReadLine());

                    //if and else statement to make sure there is a friendly Unit in the given spot
                    Square MoveTo = null;
                    Square MoveFrom = board.getSquare(UnitX, UnitY);

                    if ((MoveFrom.Dep.DepType == "Unit") && (MoveFrom.Dep.OwnedBy == player))
                    {
                        //selected spot is a friendly unit
                        ValidUnit = true;

                        //ask for direction of movement or if the user is done moving, reminding the user about how many movement points this unit has left
                        //ask by how much they want to move
                        //use different method for territory mapping

                        //only one of MoveX or MoveY will change and the other will remain the same as the unit's original position because of one dimentional movement in two dimentional space
                        uint MoveX = UnitX;
                        uint MoveY = UnitY;

                        //RepeatMove will let the user change the position of one unit multiple times at once without having to reselect it every time
                        bool RepeatMove = true;

                        do
                        {
                            //declared here because it needs a wide scope
                            uint magnitude = 0;

                            Console.WriteLine("\nUnit's Remaining Movement Points: {0}", MoveFrom.Dep.MP);
                            Console.WriteLine("Press the Corrosponding number to act: \n 0. Movement is finished \n 1. Change X co-ordinates \n 2. Change Y co-ordinates");
                            string axis = Console.ReadLine();
                            if (axis == "0")
                            {
                                Console.WriteLine("Returning to menu \nPress [ENTER] to continue");
                                Console.ReadLine();
                                RepeatMove = false;
                            }
                            else if (axis == "1" || axis == "2")
                            {

                                bool ValidChange = false;
                                do
                                {

                                    if (axis == "1")
                                    {
                                        Console.WriteLine("Would you like to go Right or Left? \n1. Right \n2. Left");
                                        string direction = Console.ReadLine();
                                        Console.WriteLine("How far would you like to go?");
                                        magnitude = UInt32.Parse(Console.ReadLine());
                                        //as long as it is within range

                                        if (direction == "1")
                                        {
                                            MoveX = UnitX + magnitude;
                                            try
                                            {
                                                //if the new position is off the board then it will throw an error to be caught which wont kick the player out of their selection
                                                MoveTo = board.getSquare(MoveX, MoveY);
                                                ValidChange = true;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Please enter a valid answer");
                                            }

                                        }
                                        else if (direction == "2")
                                        {
                                            MoveX = UnitX - magnitude;
                                            try
                                            {
                                                //if the new position is off the board then it will throw an error to be caught which wont kick the player out of their selection
                                                MoveTo = board.getSquare(MoveX, MoveY);
                                                ValidChange = true;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Please enter a valid answer");
                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine("Please enter a valid answer");
                                        }

                                    }
                                    else if (axis == "2")
                                    {
                                        Console.WriteLine("Would you like to go Up or Down? \n1. Up \n2. Down");
                                        string direction = Console.ReadLine();
                                        Console.WriteLine("How far would you like to go?");
                                        magnitude = UInt32.Parse(Console.ReadLine());
                                        //as long as it is within range

                                        //number decreases up the board and increases lower on the board for the y axis!
                                        if (direction == "1")
                                        {
                                            MoveY = UnitY - magnitude;
                                            try
                                            {
                                                //if the new position is off the board then it will throw an error to be caught which wont kick the player out of their selection
                                                MoveTo = board.getSquare(MoveX, MoveY);
                                                ValidChange = true;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Please enter a valid answer");
                                            }

                                        }
                                        else if (direction == "2")
                                        {
                                            MoveY = UnitY + magnitude;
                                            try
                                            {
                                                //if the new position is off the board then it will throw an error to be caught which wont kick the player out of their selection
                                                MoveTo = board.getSquare(MoveX, MoveY);
                                                ValidChange = true;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Please enter a valid answer");
                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine("Please enter a valid answer");
                                        }

                                    }

                                } while (ValidChange == false);



                                //events on moving a unit
                                if ((MoveX < 0 || MoveX > 11) || (MoveY < 0 || MoveY > 11))
                                {
                                    //does not move. position does not exist on the board
                                    Console.WriteLine("Please Enter co-ordinates that are on the board \nPress [ENTER] to continue");
                                    Console.ReadLine();
                                }
                                else if ((MoveTo.Dep.DepType == "Unit" && MoveTo.Dep.OwnedBy == player) || (MoveTo.Dep.DepType == "Barracks" && MoveTo.Dep.OwnedBy == player))
                                {
                                    //does not move. friendly barracks or unit occupies this square
                                    Console.WriteLine("You cannot move onto an occupied friendly square \nPress [ENTER] to continue");
                                    Console.ReadLine();
                                }
                                else if (MoveObstructed(UnitX, UnitY, MoveX, MoveY) == true)
                                {
                                    Console.WriteLine("The Unit was not able to move directly to this spot. the movement was obstructed \nPress [ENTER] to continue");
                                    Console.ReadLine();
                                }
                                else if (MoveFrom.Dep.MP < magnitude)
                                {
                                    Console.WriteLine("The Unit does not have enough remaining Movement Points \nPress [ENTER] to continue");
                                    Console.ReadLine();
                                }
                                else if ((MoveTo.Dep.DepType == "Unit" && MoveTo.Dep.OwnedBy != player) && (MoveObstructed(UnitX, UnitY, MoveX, MoveY) == false))
                                {
                                    //COMBAT against Unit, only moves to square if it wins the fight
                                    Console.WriteLine("combat test! Unit! \nPress [ENTER] to continue");
                                    Console.ReadLine();
                                }
                                else if ((MoveTo.Dep.DepType == "Barracks" && MoveTo.Dep.OwnedBy != player) && (MoveObstructed(UnitX, UnitY, MoveX, MoveY) == false))
                                {
                                    //COMBAT against Barracks, unit moves to square if it wins. unit dies if it loses 
                                    Console.WriteLine("combat test! Barracks! \nPress [ENTER] to continue");
                                    Console.ReadLine();
                                }
                                else if ((MoveTo.Dep.DepType == "Empty") && (MoveObstructed(UnitX, UnitY, MoveX, MoveY) == false))
                                {
                                    //moves with only territory impact
                                    //replaces old 'from' information with new position
                                    MoveFrom.Dep.MP = MoveFrom.Dep.MP - magnitude;
                                    PlayerCrane(UnitX, UnitY, MoveX, MoveY);
                                    TerritoryMapping(UnitX, UnitY, MoveX, MoveY);
                                    UnitX = MoveX;
                                    UnitY = MoveY;
                                    MoveFrom = board.getSquare(UnitX, UnitY);

                                    Console.WriteLine("Empty! Unit will move! \nPress [ENTER] to continue");
                                    Console.ReadLine();
                                }


                            }
                            else
                            {
                                Console.WriteLine("Please enter a valid answer");
                            }

                        } while (RepeatMove == true);



                    }
                    else
                    {
                        //selected spot is not a friendly unit
                        Console.WriteLine("Please enter the Co-ordinates of a friendly Unit \nPress [ENTER] to continue");
                        Console.ReadLine();
                    }
                }
                catch
                {
                    //process crashed due to invalid input / attempting to parse a non int variable
                    Console.WriteLine("Please enter a valid answer \nPress [ENTER] to continue");
                    Console.ReadLine();
                }
            }
            while (ValidUnit == false);

        }

    }
}
