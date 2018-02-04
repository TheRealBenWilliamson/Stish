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

            //creates a 'Square' object called "s1" and assigns it to position (1,1) on the board. it then tells s1 that it contains a barracks.
            Square s1 = board.getSquare(1, 1);
            s1.Dep = new Barracks();
            s1.Dep.OwnedBy = P1;

            //creates a 'Square' object called "s2" and assigns it to position (2,2) on the board. it then tells s2 that it contains a unit. 
            Square s2 = board.getSquare(2, 2); 
            s2.Dep = new Unit();
            s2.Dep.OwnedBy = P2;

            //creates a 'Square' object called "s3" and assigns it to position (3,3) on the board. nothing is put inside s3 so it assumes that it is empty.
            Square s3 = board.getSquare(3, 3);

            //two units are created for move testing
            Square uni1 = board.getSquare(0, 0);
            uni1.Dep = new Unit();
            uni1.Dep.OwnedBy = P1;

            Square uni2 = board.getSquare(0, 1);
            uni2.Dep = new Unit();
            uni2.Dep.OwnedBy = P2;

            //calls the Render function on the object "board". the Render function is called from the StishBoard class because "board" belongs to that class. this renders the board onto the console for the screen.
            //not sure if this change is right, there used to be parameter values that did next to nothing ...   board.Render(0, 0);
            P1.MakeMove();

            Console.Clear();
            board.Render();

        }
    }
}
