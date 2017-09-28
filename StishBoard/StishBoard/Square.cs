using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Square
    {
        
        public Square()
        {
            con = "empty";
        }
        
        private string con;

        public string Con
        {
            get
            {
                return con;
            }
            set
            {
                con = value;
            }
        }
    }
}
