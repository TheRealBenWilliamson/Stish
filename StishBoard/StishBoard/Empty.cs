﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Empty : Deployment
    {
        //constructor: gives the new object it's variable values to represent that it contains nothing
        public Empty()
        {
            depType = "Empty";
            Icon = " ";
            ownedBy = null;
        }

        public Empty(Empty CopyFrom)
        {
            depType = CopyFrom.depType;
            Icon = CopyFrom.Icon;
            ownedBy = CopyFrom.OwnedBy;
            MovementPoints = CopyFrom.MovementPoints;
            health = CopyFrom.Health;
            m_JustCreated = CopyFrom.JustCreated;
        }
    }
}

