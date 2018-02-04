using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Square
    {
        //a square object has the ability to contain any derivative of 'deployment'

        //creates a reference to the private object of Deployment called "dep".
        private Deployment dep;
        
        //default constructor: makes 'dep' contain a deployment object of "empty".
        public Square()
        {
            dep = new Empty();
        }
        
        //this is the accessor for the deployment type of the square. it allows another client to find what a particular square contains or to set what a particular square contains.
        public Deployment Dep
        {
            get
            {
                return dep;
            }
            set
            {
                dep = value;
            }
        }

        //creates a public render method called "Render" which utilises the 'helper' class to draw the squares into the console. it creates the *shell* of the square and fills it with whatever the square actually contains. 
        public void Render(int x, int y)
        {
            //the x coordinate is multiplied by four since each "square" on the board consists of 4 ascii characters.
            x = x * 4;
            Helper.StishWrite(x, y, "[");
            dep.Render(x+1, y);
            Helper.StishWrite(x+2, y, "] ");

            return;
        }
    }
}
