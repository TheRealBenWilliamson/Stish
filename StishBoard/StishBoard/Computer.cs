using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    public class Computer : Player
    {
        public Computer(PlayerNumber PN) : base(PN)
        {

        }

        public override void MakeMove()
        {
            
        }

        public Computer(Computer Cpu) : base(Cpu)
        {

        }
    }
}
