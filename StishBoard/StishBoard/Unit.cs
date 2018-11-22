using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Unit : Deployment
    {      

        //constructor: gives the new object it's variable values to represent that it contains a unit
        public Unit()
        {
            //add health
            depType = "Unit";
            Icon = "U";
            ownedBy = null;
            m_JustCreated = true;
        }

        //units will have to have another argument in the constructor that gives them a value for their health

        public Unit(Player player, Square square, uint CalledHealth)
        {
            //add health
            depType = "Unit";
            Icon = "U";
            ownedBy = player;
            square.Dep = this;
            square.Owner = player;
            MP = 0;
            Health = CalledHealth;
            m_JustCreated = true;
        }


    }
}
