using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Square
    {
        //a square object has the ability to contain any derivative of 'deployment'

        //creates a reference to the private object of Deployment called "dep".
        private Deployment dep;

        private Player owner;
        
        //default constructor: makes 'dep' contain a deployment object of "empty".
        public Square()
        {
            owner = null;
            dep = new Empty();
        }
            
        //this is a copy constructor
        public Square(Square Original, Human Player1, Human Player2)
        {
            if (Original.Owner != null)
            {
                if (Original.Owner.GetPlayerNum == "Player1")
                {
                    owner = Player1;
                }
                else
                {
                    owner = Player2;
                }
            }
            else
            {
                owner = null;
            }

            if (Original.dep.DepType == "Base")
            {
                dep = new Base((Base)Original.dep, (Human)this.owner);
            }
            else if (Original.dep.DepType == "Barracks")
            {
                dep = new Barracks((Barracks)Original.dep, (Human)this.owner);
            }
            else if (Original.dep.DepType == "Unit")
            {
                dep = new Unit((Unit)Original.dep, (Human)this.owner);
            }
            else if (Original.dep.DepType == "Empty")
            {
                dep = new Empty((Empty)Original.dep, (Human)this.owner);
            }
          
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

        public Player Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }
      

        //creates a public render method called "Render" which utilises the 'helper' class to draw the squares into the console. it creates the *shell* of the square and fills it with whatever the square actually contains. 
        public void Render(int x, int y)
        {
            if (Owner != null)
            {
                System.Console.ForegroundColor = Owner.GetRenderColour();
            }
            //the x coordinate is multiplied by four since each "square" on the board consists of 4 ascii characters.
            x = x * 4;
            Analytics.StishWrite(x, y, "[");
            Console.ResetColor();
            dep.Render(x+1, y);
            if (Owner != null)
            {
                System.Console.ForegroundColor = Owner.GetRenderColour();
            }
            Analytics.StishWrite(x+2, y, "] ");
            Console.ResetColor();

            return;
        }
    }
}
