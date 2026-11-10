using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Board
    {
        Cell[,] grid = new Cell[6, 6];

        public Board(Cell[,] grid)
        {
            this.grid = grid;
        }

        public void DisplayBoard()
        {
            Console.WriteLine(" 1 2 3 4 5 6");

            for (int y = 0; y < 6; y++)
            {
                Console.Write(" ");
                for (int x = 0; x < 6; x++)
                {
                    Console.Write(grid[y, x]?.Type ?? '-');
                }
                Console.WriteLine();
            }
        }
    }
}