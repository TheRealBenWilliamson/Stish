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

        //only a Unit should have more than 0 MP. a unit has the potential to move one square for every movement point it possesses. a unit is constructed with 0 MP but has them fully reset at the start of a turn. the max MP of a unit should be changed throughout testing and balancing in the "RefreshMP" function
        protected uint MovementPoints;

        //giving health to all Dep types
        ////Health represents the amount of damage a unit can deal or absorb. when it reaches 0, the Dep dies.
        //all Deps can die except Empty
        protected uint health;

        //this is only used by the units to stop them attacking on the turn that they are bought
        protected bool m_JustCreated;

        //ownedby Enums would be better than using strings to identify which player owns a square
        //public enum Owner { Null, Player1, Player2 };
        //protected Owner ownedby;

        //default constructor: makes any square assume it is empty and owned by no-one. this can be overwritten by telling the square that it contains something.
        protected Deployment()
        {
            depType = "Empty";
            Icon = " ";
            ownedBy = null;
            MovementPoints = 0;
            //Health is already passed through by the calling function
        }

        //this is the copy constructor
        public Deployment(Deployment Original)
        {
            depType = String.Copy(Original.depType);
            Icon = String.Copy(Original.Icon);
            //ONLY WORKS WITH HUMANS
            ownedBy = new Human(Original.ownedBy);
            MovementPoints = Original.MovementPoints;
            health = Original.health;
            m_JustCreated = Original.m_JustCreated;
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

        public uint Health
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        //MP stands for MovementPoints. this function should only be used on the Unit DepType
        public uint MP
        {
            get
            {
                return MovementPoints;
            }
            set
            {
                MovementPoints = value;
            }
        }

        public bool JustCreated
        {
            get
            {
                return m_JustCreated;
            }
            set
            {
                m_JustCreated = value;
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

            Analytics.StishWrite(x, y, Icon);
            Console.ResetColor();
            return;
        }
    }
}
