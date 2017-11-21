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
            dep = new Empty();
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

        public void Render(int x, int y)
        {
            x = x * 4;
            Helper.StishWrite(x, y, "[");
            dep.Render(x+1, y);
            Helper.StishWrite(x+2, y, "] ");
            return;
        }
    }
}
