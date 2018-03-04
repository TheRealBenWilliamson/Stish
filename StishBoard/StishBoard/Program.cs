using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Program
    {
        static void Main(string[] args)
        {
            //creates a board from the StishBoard class called "board" by calling the public 'Instance' method.
            StishBoard board = StishBoard.Instance;

            Player P1;
            P1 = Player.PlayerFactory(Player.PlayerNumber.Player1, Player.PlayerType.Human);
            
            Player P2;
            P2 = Player.PlayerFactory(Player.PlayerNumber.Player2, Player.PlayerType.Human);

            board.Player1 = P1;
            board.Player2 = P2;

            //creates a 'Square' object called "s1" and assigns it to position (1,1) on the board. it then tells s1 that it contains a barracks.
            new Barracks(P1, board.getSquare(1, 1));

            //creates a 'Square' object called "s2" and assigns it to position (2,2) on the board. it then tells s2 that it contains a unit. 
            new Unit(P2, board.getSquare(2, 2));

            //creates a 'Square' object called "s3" and assigns it to position (3,3) on the board. nothing is put inside s3 so it assumes that it is empty.
            Square s3 = board.getSquare(3, 3);

            //two units are created for move testing
            new Unit(P1, board.getSquare(0, 0));

            new Unit(P2, board.getSquare(0, 1));

            //calls the Render function on the object "board". the Render function is called from the StishBoard class because "board" belongs to that class. this renders the board onto the console for the screen.
            //not sure if this change is right, there used to be parameter values that did next to nothing ...   board.Render(0, 0);
            P1.MakeMove();

            Console.Clear();
            board.Render();

        }
    }
}
