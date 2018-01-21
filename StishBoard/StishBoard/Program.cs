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

            //creates a 'Square' object called "s1" and assigns it to position (1,1) on the board. it then tells s1 that it contains a barracks.
            Square s1 = board.getSquare(1, 1);
            s1.Dep = new Barracks();
            s1.Dep.OwnedBy = "player1";

            //creates a 'Square' object called "s2" and assigns it to position (2,2) on the board. it then tells s2 that it contains a unit. 
            Square s2 = board.getSquare(2, 2); 
            s2.Dep = new Unit();
            s2.Dep.OwnedBy = "player2";

            //creates a 'Square' object called "s3" and assigns it to position (3,3) on the board. nothing is put inside s3 so it assumes that it is empty.
            Square s3 = board.getSquare(3, 3);

            Player P1;
            P1 = Player.PlayerFactory(Player.PlayerNumber.Player1, Player.PlayerType.Human);

            //calls the Render function on the object "board". the Render function is called from the StishBoard class because "board" belongs to that class. this renders the board onto the console for the screen.
            board.Render(0, 0);


        }
    }
}
