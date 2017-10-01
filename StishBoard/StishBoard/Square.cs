using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Square
    {
        private Deployment dep;
        
        public Square()
        {
            dep = null;
        }
        

        public string DepType
        {
            get
            {
                return dep.DepType;
            }
        }

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
    }
}
