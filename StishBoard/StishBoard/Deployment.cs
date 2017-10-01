using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public abstract class Deployment
    {
        protected string depType;
        protected Deployment()
        {
            depType = "empty";
        }

        public string DepType
        {
            get
            {
                return depType;
            }

        }
    }
}
