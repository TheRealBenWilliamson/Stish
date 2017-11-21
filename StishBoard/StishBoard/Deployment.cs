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
        protected string depType;

        public string Icon;

        protected Deployment()
        {
            depType = "empty";
            Icon = " ";
        }

        public string DepType
        {
            get
            {
                return depType;
            }

        }

        public void Render(int x, int y)
        {
            Helper.StishWrite(x, y, Icon);
            return;
        }
    }
}
