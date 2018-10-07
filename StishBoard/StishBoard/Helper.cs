using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Helper
    {
        //creates a function that allows the console to write into specific positions
        public static void StishWrite(int x, int y, string C)
        {            
            Console.SetCursorPosition(x , y+2);
            Console.WriteLine(C);
            return;
        }
       

    }
}
