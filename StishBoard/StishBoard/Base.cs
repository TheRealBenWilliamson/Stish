using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Base : Deployment
    {
        //constructor: gives the new object it's variable values to represent that it contains a barracks
        public Base(Player player, Square square, uint CalledHealth)
        {
            //add health
            depType = "Base";
            Icon = "H";
            ownedBy = player;
            square.Dep = this;
            square.Owner = player;

            //health may be changed for gameplay
            Health = 20;

        }


        public Base(Base CopyFrom, Human Owner)
        {
            depType = CopyFrom.depType;
            Icon = CopyFrom.Icon;
            ownedBy = Owner;
            MovementPoints = CopyFrom.MovementPoints;
            health = CopyFrom.Health;
            m_JustCreated = CopyFrom.JustCreated;
        }

        public Base(Base CopyFrom, Computer Owner)
        {
            depType = CopyFrom.depType;
            Icon = CopyFrom.Icon;
            ownedBy = Owner;
            MovementPoints = CopyFrom.MovementPoints;
            health = CopyFrom.Health;
            m_JustCreated = CopyFrom.JustCreated;
        }

        public Base(Base CopyFrom)
        {
            depType = CopyFrom.depType;
            Icon = CopyFrom.Icon;
            ownedBy = null;
            MovementPoints = CopyFrom.MovementPoints;
            health = CopyFrom.Health;
            m_JustCreated = CopyFrom.JustCreated;
        }
    }
}
