using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Human : Player
    {
        public Human(PlayerNumber PN) :  base(PN)
        {

        }

        public enum Action { MoveUnit, BuyUnit, BuyBarracks, EndTurn };

        //by now a board should already have been created. StishBoard.Instance allows us to get a reference to the existing board.
        StishBoard board = StishBoard.Instance;

        //define methods for the player's move

        private void HumanMoveUnit()
        {
            //will use the user interface to ask the player which unit they want to move and where to. it will call MoveUnit(from , to) in Player class to make from's position empty. it will set to's position to a unit or barracks.
            //need to make sure there is a friendly Unit at the 'from' posititon
            while (true)
            {               
                try
                {
                    Console.WriteLine("\nWhat Unit would you like to move? \n X co-ordinate:");
                    uint UnitX = UInt32.Parse(Console.ReadLine());
                    Console.WriteLine(" Y co-ordinate:");
                    uint UnitY = UInt32.Parse(Console.ReadLine());

                    //if and else statement to make sure there is a friendly Unit in the given spot
                    Square MoveFrom = board.getSquare(UnitX, UnitY);
                    if (MoveFrom.Dep.DepType == "Unit" && MoveFrom.Dep.OwnedBy == this)
                    {
                        //selected spot is a friendly unit

                        Console.WriteLine("Where would you like to move this unit to? \n X co-ordinate:");
                        uint MoveX = UInt32.Parse(Console.ReadLine());
                        Console.WriteLine(" Y co-ordinate:");
                        uint MoveY = UInt32.Parse(Console.ReadLine());
                        Square MoveTo = board.getSquare(MoveX, MoveY);

                        //events on moving a unit
                        if ((MoveX < 0 || MoveX > 11) || (MoveY < 0 || MoveY > 11))
                        {
                            //does not move. position does not exist on the board
                            Console.WriteLine("Please Enter co-ordinates that are on the board \nPress [ENTER] to continue");
                            Console.ReadLine();
                        }                        
                        else if ((MoveTo.Dep.DepType == "Unit" && MoveTo.Dep.OwnedBy == this) || (MoveTo.Dep.DepType == "Barracks" && MoveTo.Dep.OwnedBy == this))
                        {
                            //does not move. friendly barracks or unit occupies this square
                            Console.WriteLine("You cannot move onto an occupied friendly square \nPress [ENTER] to continue");
                            Console.ReadLine();
                        }
                        else if (MoveTo.Dep.DepType == "Unit" && MoveTo.Dep.OwnedBy != this)
                        {
                            //COMBAT against Unit, only moves to square if it wins the fight
                            Console.WriteLine("combat test! Unit! \nPress [ENTER] to continue");
                            Console.ReadLine();


                            //break because move was vaild
                            break;
                        }
                        else if (MoveTo.Dep.DepType == "Barracks" && MoveTo.Dep.OwnedBy != this)
                        {
                            //COMBAT against Barracks, moves to square infront of barracks if the Unit doesnt die
                            Console.WriteLine("combat test! Barracks! \nPress [ENTER] to continue");
                            Console.ReadLine();


                            //break because move was vaild
                            break;
                        }
                        else if (MoveTo.Dep.DepType == "Empty")
                        {
                            //moves with only territory impact
                            PlayerCrane(UnitX, UnitY, MoveX, MoveY);
                            Console.WriteLine("Empty! Unit will move! \nPress [ENTER] to continue");
                            Console.ReadLine();


                            //break because move was vaild
                            break;
                        }
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
            

        }

        private void BuyUnit()
        {
            //ask how much the player wants to spend           
            uint cost;
            bool Cont = true;
            do
            {
                try
                {
                    //test that cost cannot be negative
                    Console.WriteLine("how much money would you like to spend?");
                    cost = UInt32.Parse(Console.ReadLine());
                    if (Balance >= cost)
                    {
                        Balance = Balance - cost;
                    }

                    Console.WriteLine("Where would you like to place this unit? \n X co-ordinate:");
                    uint placeX = UInt32.Parse(Console.ReadLine());
                    Console.WriteLine(" Y co-ordinate:");
                    uint placeY = UInt32.Parse(Console.ReadLine());

                    Square Place = board.getSquare(placeX, placeY);


                    //events on moving a unit
                    // create a single fuction for movement criteria
                    //move class and objects



                    if ((placeX < 0 || placeX > 11) || (placeY < 0 || placeY > 11))
                    {
                        //does not move. position does not exist on the board
                        Console.WriteLine("Please Enter co-ordinates that are on the board \nPress [ENTER] to continue");
                        Console.ReadLine();
                    }
                    else if ((Place.Dep.DepType == "Unit") || (Place.Dep.DepType == "Barracks"))
                    {
                        //does not move. friendly barracks or unit occupies this square
                        Console.WriteLine("You cannot move onto an occupied space \nPress [ENTER] to continue");
                        Console.ReadLine();
                    }
                    /*else if (Place.Dep.OwnedBy != this)
                    {
                        //does not move. friendly barracks or unit occupies this square
                        Console.WriteLine("You can only move onto a friendly space \nPress [ENTER] to continue");
                        Console.ReadLine();
                    }
                    */
                    else if ((Place.Dep.DepType == "Empty") /*&& (Place.Dep.OwnedBy == this)*/)
                    {
                        //moves with only territory impact

                        PlaceDep(new Unit(), placeX, placeY);

                        Console.WriteLine("a new Unit has been placed \nPress [ENTER] to continue");
                        Console.ReadLine();
                        //break because move was vaild
                        Cont = false;
                    }



                }
                catch
                {
                    //process crashed due to invalid input / attempting to parse a non int variable
                    Console.WriteLine("Please enter a valid answer \nPress [ENTER] to continue");
                    Console.ReadLine();
                }

            } while (Cont == true);
            
            //checked if they have that money
            //if they dont then loop
            //if they do then remove that quantity from the balance and ask where they want to place they unit
            //check if this position is valid
            //only continue if it is valid

        }

        private void BuyBarracks()
        {

        }

        public override void MakeMove()
        {

            //things to do in a turn: move a unit, buy a unit (and place it), buy a barracks (and place it), end their turn.
            bool EndTurn = false;

            while (!EndTurn)
            {
                //console is cleared so a render can take place
                Console.Clear();

                board.Render();
                
                Console.WriteLine("\nPress the Corrosponding number to act: \n 1. Move a unit \n 2. Buy a unit (and place it) \n 3. Buy a barracks (and place it) \n 4. End your turn");

                try
                {
                    Action Act = (Action)((Int32.Parse(Console.ReadLine())) - 1 );
                    switch (Act)
                    {
                        case Action.MoveUnit:
                            // move a unit
                            HumanMoveUnit();
                            break;
                        case Action.BuyUnit:
                            //buy a unit (and place it)
                            BuyUnit();
                            break;
                        case Action.BuyBarracks:
                            //buy a barracks (and place it)

                            break;
                        case Action.EndTurn:
                            //end player's turn

                            //EndTurn is set to True to end the turn loop.
                            TurnBalance();
                            EndTurn = true;
                            break;
                        default:
                            //invalid
                            Console.WriteLine("Please enter a valid answer \nPress [ENTER] to continue");
                            Console.ReadLine();
                            break;
                    }

                    
                }
                catch
                {
                    //process crashed due to invalid input (likely attempting to parse a non int variable)
                    Console.WriteLine("Please enter a valid answer \nPress [ENTER] to continue");
                    Console.ReadLine();
                }

            }

        }

    }
}
