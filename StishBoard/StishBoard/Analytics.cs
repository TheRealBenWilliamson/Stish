using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Analytics
    {

        //creates a function that allows the console to write into specific positions
        public static void StishWrite(int x, int y, string C)
        {
            Console.SetCursorPosition(x, y + 2);
            Console.WriteLine(C);
            return;
        }

        //this is purely cosmetic and helps describe the squares surrounding the cursor         

        public static void Cardinal(Player Cont, Coordinate Pos)
        {
            Coordinate up = new Coordinate(Pos.X, Pos.Y);
            up.MoveUp();
            Coordinate right = new Coordinate(Pos.X, Pos.Y);
            right.MoveRight();
            Coordinate down = new Coordinate(Pos.X, Pos.Y);
            down.MoveDown();
            Coordinate left = new Coordinate(Pos.X, Pos.Y);
            left.MoveLeft();
            System.Console.ForegroundColor = Cont.GetRenderColour();
            Console.SetCursorPosition(0, 12);
            Console.WriteLine("{0}'s Turn", Cont.GetPlayerNum);
            Console.ResetColor();

            Console.SetCursorPosition(0, 0);

            List<string> CardinalString = new List<string>() { "Centre", "Up", "Right", "Down", "Left" };
            List<Coordinate> Direction = new List<Coordinate>() { Pos, up, right, down, left };

            for (int card = 0; card < 5; card++)
            {
                //Square Check = StishBoard.Instance.getSquare(Coord[card,0], Coord[card, 1]);
                Square Check = StishBoard.Instance.getSquare(Direction[card]);

                if (Check != null)
                {
                    string CheckType;
                    if (Check.Dep.DepType == null)
                    {
                        CheckType = "Nothing";
                    }
                    else
                    {
                        CheckType = Check.Dep.DepType;
                    }

                    string CheckOwner;
                    if (Check.Owner == null)
                    {
                        CheckOwner = "No One";
                    }
                    else
                    {
                        CheckOwner = Check.Owner.GetPlayerNum;
                    }

                    Console.SetCursorPosition(4 * 6, (card + 3));
                    Console.WriteLine("{0} has: {1} Health, contains: {2} , belongs to: {3} , Movement Points: {4}", CardinalString[card], Check.Dep.Health.ToString(), CheckType, CheckOwner, Check.Dep.MP.ToString());
                }
            }
        }
    }
}
