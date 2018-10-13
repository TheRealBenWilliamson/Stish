using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StishBoard
{
    class Coordinate
    {
        private uint Xco;
        private uint Yco;

        public uint X
        {
            get
            {
                return Xco;
            }
            set
            {
                Xco = value;
            }
        }

        public uint Y
        {
            get
            {
                return Yco;
            }
            set
            {
                Yco = value;
            }
        }

        public Coordinate()
        {
            Xco = 0;
            Yco = 0;
        }

        public Coordinate(uint X, uint Y)
        {
            Xco = X;
            Yco = Y;
        }

        public void MoveLeft()
        {
            Xco--;
        }
        public void MoveRight()
        {
            Xco++;
        }
        public void MoveUp()
        {
            Yco--;
        }
        public void MoveDown()
        {
            Yco++;
        }
    }
}
