using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Tools
    {
        public static Cell StrToCell(string coord)
        {
            // Remember letter is the row, number is the column
            int x = Convert.ToInt16(coord[1] - 48); //string num to int num
            int y = Convert.ToInt16((char)coord[0]) - 64; //string letter to int

            return new Cell(x, y);
        }

        public static string CellToStr(Cell cell)
        {
            char letter = (char)('A' + cell.Y - 1);
            return letter.ToString() + cell.X.ToString();
        }
    }
}
