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
            depType = "Barracks";
            Icon = "B";
            ownedBy = null;
            //health may be changed for gameplay
            Health = 5;
        }

        public Barracks(Player player, Square square, uint CalledHealth)
        {
            //add health
            depType = "Barracks";
            Icon = "B";
            ownedBy = player;
            square.Dep = this;
            square.Owner = player;
            Health = CalledHealth;

        }

        public Barracks(Barracks CopyFrom, Human Owner)
        {
            depType = CopyFrom.depType;
            Icon = CopyFrom.Icon;
            ownedBy = Owner;
            MovementPoints = CopyFrom.MovementPoints;
            health = CopyFrom.Health;
            m_JustCreated = CopyFrom.JustCreated;
        }

        public Barracks(Barracks CopyFrom, Computer Owner)
        {
            depType = CopyFrom.depType;
            Icon = CopyFrom.Icon;
            ownedBy = Owner;
            MovementPoints = CopyFrom.MovementPoints;
            health = CopyFrom.Health;
            m_JustCreated = CopyFrom.JustCreated;
        }

        public Barracks(Barracks CopyFrom)
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
