using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Program
    {
        public enum Won { Player1, Player2 };
        public enum Turn { Player1, Player2 };

        static void Main(string[] args)
        {

            //GAME STARTS HERE
            Console.SetWindowSize(130, 25);

            StishBoard.Instance.Player1 = Player.PlayerFactory(Player.PlayerNumber.Player1, Player.PlayerType.Human, StishBoard.Instance); ;
            StishBoard.Instance.Player2 = Player.PlayerFactory(Player.PlayerNumber.Player2, Player.PlayerType.Computer, StishBoard.Instance); ;

            Console.Clear();
            StishBoard.Instance.Render();

            //game loop takes place here
            bool GameEnd = false;
            Won won = 0;
            Turn turn = Turn.Player1;                

            while (GameEnd == false)
            {
                //checks if a base has been destroyed. if one has then the other player has won.
                //if not then alternate player turns
                Coordinate P1Base = new Coordinate(StishBoard.Instance.Player1.BaseX, StishBoard.Instance.Player1.BaseY);
                Coordinate P2Base = new Coordinate(StishBoard.Instance.Player2.BaseX, StishBoard.Instance.Player2.BaseY);
                if (StishBoard.Instance.getSquare(P2Base).Dep.Health < 1)
                {
                    //Player1 has won
                    GameEnd = true;
                    won = Won.Player1;
                }
                else if (StishBoard.Instance.getSquare(P1Base).Dep.Health < 1)
                {
                    //Player2 has won
                    GameEnd = true;
                    won = Won.Player2;
                }
                else
                {
                    //Game Continues
                    if(turn == Turn.Player1)
                    {                    
                        Cursor.Instance.FindX = StishBoard.Instance.Player1.CursorX;
                        Cursor.Instance.FindY = StishBoard.Instance.Player1.CursorY;

                        StishBoard.Instance.Player1.TurnBalance(StishBoard.Instance);
                        StishBoard.Instance.Player1.MaxMP(StishBoard.Instance);
                        StishBoard.Instance.Player1.MakeMove();
                        turn++;
                    }
                    else if (turn == Turn.Player2)
                    {
                        StishBoard.Instance.Player2.TurnBalance(StishBoard.Instance);
                        StishBoard.Instance.Player2.MaxMP(StishBoard.Instance);
                        Cursor.Instance.FindX = StishBoard.Instance.Player2.CursorX;
                        Cursor.Instance.FindY = StishBoard.Instance.Player2.CursorY;

                        StishBoard.Instance.Player2.MakeMove();
                        turn--;
                    }                   
                }
                

            }

            Console.WriteLine("{0} HAS WON THE GAME \nPRESS <ENTER> TO KILL THE PROGRAM", won.ToString());
            Console.ReadLine();

            

        }
    }
}
