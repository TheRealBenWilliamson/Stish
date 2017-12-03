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
        public string Icon;

        //default constructor: makes any square assume it is empty. this can be overwritten by telling the square that it contains something.
        protected Deployment()
        {
            depType = "empty";
            Icon = " ";
        }

        //an accessor so that a client can find what type of deployment this particular object is.
        public string DepType
        {
            get
            {
                return depType;
            }
        }

        //creates a render method called "Render" which draws the icon of this deployment type to the console.
        public void Render(int x, int y)
        {
            Helper.StishWrite(x, y, Icon);
            return;
        }
    }
}
