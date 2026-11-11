using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Cell
    {
        int x;
        int y;
        char type;

        public Cell(int x, int y, char type = '-')
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }

        public char Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}