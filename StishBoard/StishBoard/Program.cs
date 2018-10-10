using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Program
    {
        public enum Lost { Player1, Player2 };
        public enum Turn { Player1, Player2 };

        static void Main(string[] args)
        {
            //creates a board from the StishBoard class called "board" by calling the public 'Instance' method.
            StishBoard board = StishBoard.Instance;
            Cursor cursor = Cursor.Instance;

            Console.SetWindowSize(170, 25);

            Player P1;
            P1 = Player.PlayerFactory(Player.PlayerNumber.Player1, Player.PlayerType.Human);
            
            Player P2;
            P2 = Player.PlayerFactory(Player.PlayerNumber.Player2, Player.PlayerType.Human);

            board.Player1 = P1;
            board.Player2 = P2;

            Console.Clear();
            board.Render();

            //game loop takes place here
            bool GameEnd = false;
            Lost lost = 0;
            Turn turn = Turn.Player1;
            while (GameEnd == false)
            {
                //checks if a base has been destroyed. if one has then the other player has won.
                //if not then alternate player turns
                if(board.getSquare(5, 9).Dep.Health < 1)
                {
                    //Player1 has lost
                    GameEnd = true;
                    lost = Lost.Player1;
                }
                else if (board.getSquare(5, 1).Dep.Health < 1)
                {
                    //Player2 has lost
                    GameEnd = true;
                    lost = Lost.Player2;
                }
                else
                {
                    //Game Continues
                    if(turn == Turn.Player1)
                    {
                        P1.TurnBalance();
                        P1.MaxMP();
                        cursor.FindX = P1.CursorX;
                        cursor.FindY = P1.Cursory;
                        P1.MakeMove();
                        turn++;
                    }
                    else if (turn == Turn.Player2)
                    {
                        P2.TurnBalance();
                        P2.MaxMP();
                        cursor.FindX = P2.CursorX;
                        cursor.FindY = P2.Cursory;
                        P2.MakeMove();
                        turn--;
                    }
                }
                

            }

            Console.WriteLine("{0} HAS WON THE GAME \nPRESS <ENTER> TO KILL THE PROGRAM", lost.ToString());
            Console.ReadLine();

            

        }
    }
}
