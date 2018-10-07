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

        //Option helps use enums for the player making number corrosponding branching choices
        public enum Option { opt0, opt1, opt2, opt3 };
        public enum Action { MoveUnit, BuyUnit, BuyBarracks, EndTurn};

        //by now a board should already have been created. StishBoard.Instance allows us to get a reference to the existing board.
        StishBoard board = StishBoard.Instance;
        Cursor cursor = Cursor.Instance;

        //define methods for the player's move


        public override void MakeMove()
        {
            Console.Clear();
            board.Render();
            cursor.Render(this);

            bool EndTurn = false;
            do
            {
                string output = "Nothing";
                System.ConsoleKey put = Console.ReadKey(true).Key;
                
                if (put == ConsoleKey.W)
                {
                    output = "W";
                }
                else if (put == ConsoleKey.A)
                {
                    output = "A";
                }
                else if (put == ConsoleKey.S)
                {
                    output = "S";
                }
                else if (put == ConsoleKey.D)
                {
                    output = "D";
                }
                else if (put == ConsoleKey.Spacebar)
                {
                    output = " ";
                }
                else if (put == ConsoleKey.Q)
                {
                    output = "Q";
                }
                else if (put == ConsoleKey.E)
                {
                    output = "E";
                }
                else if (put == ConsoleKey.Enter)
                {
                    EndTurn = true;
                    cursor.CursorMode = Cursor.Mode.free;
                    CursorX = cursor.FindX;
                    Cursory = cursor.FindY;
                }

                cursor.Move(this, output);
            } while (EndTurn == false);
            
        }

        /*

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
                    Console.WriteLine("how much money would you like to spend? \n(Enter '0' to go back)");
                    cost = UInt32.Parse(Console.ReadLine());
                    if(cost == 0)
                    {
                        Console.WriteLine("Returning to menu \nPress [ENTER] to continue");
                        Console.ReadLine();
                        Cont = false;
                    }
                    else if (Balance >= cost)
                    {
                        //cost will be deducted when the unit is actually placed

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
                            //cannot be placed. position does not exist on the board
                            Console.WriteLine("Please Enter co-ordinates that are on the board \nPress [ENTER] to continue");
                            Console.ReadLine();
                        }
                        else if (Place.Dep.DepType != "Empty")
                        {
                            //cannot be placed. a barracks or unit occupies this square
                            Console.WriteLine("You cannot place a unit onto an occupied space \nPress [ENTER] to continue");
                            Console.ReadLine();
                        }
                        else if (Place.Owner != this)
                        {
                            //cannot be placed. this square is not owned by the player
                            Console.WriteLine("You can only place a unit onto a friendly square \nPress [ENTER] to continue");
                            Console.ReadLine();
                        }                        
                        else if ((Place.Dep.DepType == "Empty") && (Place.Owner == this))
                        {
                            //is placed
                            Balance = Balance - cost;
                            new Unit(this, board.getSquare(placeX, placeY));

                            Console.WriteLine("a new Unit has been placed \nPress [ENTER] to continue");
                            Console.ReadLine();
                            //break because move was vaild
                            Cont = false;
                        }


                    }
                    else
                    {
                        Console.WriteLine("You do not have this much money");
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
            //ask how much the player wants to spend           
            bool Cont = true;
            if(balance < 5)
            {
                Cont = false;
                Console.WriteLine("This will cost 5 money. you cannot afford this. \nPress [ENTER] to continue");
                Console.ReadLine();
            }

            while (Cont == true)
            {
                try
                {
                    //test that cost cannot be negative
                    Console.WriteLine("This will cost 5 money. \nEnter '1' to Continue or Enter '0' to go back");
                    Option ans = (Option)((Int32.Parse(Console.ReadLine())));
                    switch (ans)
                    {
                        case Option.opt0:
                            Console.WriteLine("Returning to menu \nPress [ENTER] to continue");
                            Console.ReadLine();
                            Cont = false;
                            break;

                        case Option.opt1:
                            //cost will be deducted when the unit is actually placed

                            Console.WriteLine("Where would you like to place this Barracks? \n X co-ordinate:");
                            uint placeX = UInt32.Parse(Console.ReadLine());
                            Console.WriteLine(" Y co-ordinate:");
                            uint placeY = UInt32.Parse(Console.ReadLine());

                            Square Place = board.getSquare(placeX, placeY);

                            if ((placeX < 0 || placeX > 11) || (placeY < 0 || placeY > 11))
                            {
                                //cannot be placed. position does not exist on the board
                                Console.WriteLine("Please Enter co-ordinates that are on the board \nPress [ENTER] to continue");
                                Console.ReadLine();
                            }
                            else if (Place.Dep.DepType != "Empty")
                            {
                                //cannot be placed. a barracks or unit occupies this square
                                Console.WriteLine("You cannot place a barracks onto an occupied space \nPress [ENTER] to continue");
                                Console.ReadLine();
                            }
                            else if (Place.Owner != this)
                            {
                                //cannot be placed. this square is not owned by the player
                                Console.WriteLine("You can only place a barracks onto a friendly square \nPress [ENTER] to continue");
                                Console.ReadLine();
                            }
                            else if ((Place.Dep.DepType == "Empty") && (Place.Owner == this))
                            {
                                //is placed
                                Balance -= 5;
                                new Barracks(this, board.getSquare(placeX, placeY));

                                Console.WriteLine("a new Barracks has been placed \nPress [ENTER] to continue");
                                Console.ReadLine();
                                //break because move was vaild
                                Cont = false;
                            }

                            break;
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

        */


        /*
        public override void MakeMove()
        {

            //MovementPoints are set to max MP at the start of the turn
            MaxMP();

            //things to do in a turn: move a unit, buy a unit (and place it), buy a barracks (and place it), end their turn.
            bool EndTurn = false;

            while (!EndTurn)
            {
                //console is cleared so a render can take place
                Console.Clear();

                board.Render();
                
                Console.WriteLine("\nPress the Corrosponding number to act: \n 1. Move a unit \n 2. Buy a unit \n 3. Buy a barracks \n 4. End your turn");

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
                            //as long as one friendly unit is on the board
                            BuyUnit();
                            break;
                        case Action.BuyBarracks:
                            //buy a barracks (and place it)
                            BuyBarracks();
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
        */

    }
}
