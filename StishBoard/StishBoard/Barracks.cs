using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Barracks : Deployment
    {
        //constructor: gives the new object it's variable values to represent that it contains a barracks
        public Barracks ()
        {
            //add health
            depType = "Barracks";
            Icon = "B";
            ownedBy = null;
        }

        public Barracks(Player player, Square square)
        {
            //add health
            depType = "Barracks";
            Icon = "B";
            ownedBy = player;
            square.Dep = this;
            square.Owner = player;

        }

    }
}
