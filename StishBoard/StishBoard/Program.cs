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

            TreeNode RootNode = new TreeNode();
            NamedTreeNode Child1 = new NamedTreeNode("Child1",RootNode);
            NamedTreeNode Child2 = new NamedTreeNode("Child2");
            Child1.AddChild(Child2);
            NamedTreeNode Child3 = new NamedTreeNode("Child3",Child1);
            NamedTreeNode Child4 = new NamedTreeNode("Child4",Child1);

            TreeNode inspect = Child4;
            while (inspect != null)
            {
                inspect = inspect.GetParent();
            }

            Child4.Remove();           

            //GAME STARTS HERE
            Console.SetWindowSize(130, 25);

            Player P1;
            P1 = Player.PlayerFactory(Player.PlayerNumber.Player1, Player.PlayerType.Human);
            
            Player P2;
            P2 = Player.PlayerFactory(Player.PlayerNumber.Player2, Player.PlayerType.Human);

            StishBoard.Instance.Player1 = P1;
            StishBoard.Instance.Player2 = P2;

            Console.Clear();
            StishBoard.Instance.Render();

            
            ForeSight test = new ForeSight();
            Coordinate testCo = new Coordinate(4, 4);
            test.Sweep(testCo, P1);
            

            //game loop takes place here
            bool GameEnd = false;
            Won won = 0;
            Turn turn = Turn.Player1;
            while (GameEnd == false)
            {
                //checks if a base has been destroyed. if one has then the other player has won.
                //if not then alternate player turns
                Coordinate P1Base = new Coordinate(P1.BaseX, P1.BaseY);
                Coordinate P2Base = new Coordinate(P2.BaseX, P2.BaseY);
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
                        P1.TurnBalance();
                        P1.MaxMP();
                        Cursor.Instance.FindX = P1.CursorX;
                        Cursor.Instance.FindY = P1.Cursory;
                        P1.MakeMove();
                        turn++;
                    }
                    else if (turn == Turn.Player2)
                    {
                        P2.TurnBalance();
                        P2.MaxMP();
                        Cursor.Instance.FindX = P2.CursorX;
                        Cursor.Instance.FindY = P2.Cursory;
                        P2.MakeMove();
                        turn--;
                    }
                }
                

            }

            Console.WriteLine("{0} HAS WON THE GAME \nPRESS <ENTER> TO KILL THE PROGRAM", won.ToString());
            Console.ReadLine();

            

        }
    }
}
