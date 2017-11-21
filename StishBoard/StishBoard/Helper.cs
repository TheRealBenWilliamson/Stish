using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Helper
    {

        public static void StishWrite(int x, int y, string C)
        {
            Console.SetCursorPosition(x , y);
            Console.WriteLine(C);
            return;
        }

    }
}
