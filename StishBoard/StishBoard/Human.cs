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
                Console.WriteLine("\nWhat Unit would you like to move? \n X co-ordinate:");
                string unitX = Console.ReadLine();
                int UnitX;
                Console.WriteLine(" Y co-ordinate:");
                string unitY = Console.ReadLine();
                int UnitY;
                try
                {
                    UnitX = Int32.Parse(unitX);
                    UnitY = Int32.Parse(unitY);

                    //if and else statement to make sure there is a friendly Unit in the given spot
                    Square MoveFrom = board.getSquare(UnitX, UnitY);
                    if (MoveFrom.Dep.DepType == "Unit" && MoveFrom.Dep.OwnedBy == this)
                    {
                        //selected spot is a friendly unit

                        Console.WriteLine("Where would you like to move this unit to? \n X co-ordinate:");
                        string moveX = Console.ReadLine();
                        int MoveX;
                        Console.WriteLine(" Y co-ordinate:");
                        string moveY = Console.ReadLine();
                        int MoveY;

                        MoveX = Int32.Parse(moveX);
                        MoveY = Int32.Parse(moveY);
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

                            break;
                        case Action.BuyBarracks:
                            //buy a barracks (and place it)

                            break;
                        case Action.EndTurn:
                            //end player's turn
                            
                            //EndTurn is set to True to end the turn loop.
                            EndTurn = true;
                            break;
                        default:
                            //invalid
                            Console.WriteLine("Please enter a valid answer \nPress [ENTER] to continue");
                            Console.ReadLine();
                            break;
                    }

                    UpdateBalance();
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
