using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StishBoard;

namespace StishBoard
{
    public abstract class Deployment
    {
        //creates a string variable for each square to recognise what it contains.
        protected string depType;

        //creates a variable called "Icon" which contains the symbol that corrosponds to what is contained by a square.
        protected string Icon;

        //a variable designed to show which player owns a particular as territory
        protected Player ownedBy;

        //ownedby Enums would be better than using strings to identify which player owns a square
        //public enum Owner { Null, Player1, Player2 };
        //protected Owner ownedby;

        //default constructor: makes any square assume it is empty and owned by no-one. this can be overwritten by telling the square that it contains something.
        protected Deployment()
        {
            depType = "empty";
            Icon = " ";
            ownedBy = null;
        }

        //an accessor so that a client can find what type of deployment this particular object is.
        public string DepType
        {
            get
            {
                return depType;
            }
        }

        public Player OwnedBy
        {
            get
            {
                return ownedBy;
            }
            set
            {
                ownedBy = value;
            }
        }

        //creates a render method called "Render" which draws the icon of this deployment type to the console.
        public void Render(int x, int y)
        {
            //decides what colour to draw the board based on which player owns each square
            if (ownedBy != null)
            {
                System.Console.ForegroundColor = ownedBy.GetRenderColour();
            }            

            Helper.StishWrite(x, y, Icon);
            Console.ResetColor();
            return;
        }
    }
}
