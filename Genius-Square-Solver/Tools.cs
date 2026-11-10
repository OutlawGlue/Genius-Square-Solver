using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Tools
    {
        public static Cell StrToCell(string coord, bool blocker = false)
        {
            // Remember letter is the row, number is the column
            int x = Convert.ToInt16(coord[1] - 49); //string num to int num
            int y = Convert.ToInt16((char)coord[0]) - 65; //string letter to int

            if (!blocker)
            {
                return new Cell(x, y);
            }
            else
            {
                return new Cell(x, y, 'x');
            }
        }

        public static string CellToStr(Cell cell)
        {
            char letter = (char)('A' + cell.Y);
            return letter.ToString() + (cell.X + 1).ToString();
        }
    }
}
