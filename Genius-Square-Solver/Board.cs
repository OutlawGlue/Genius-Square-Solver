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

        public Cell[,] Grid
        {
            get { return grid; }
        }

        public void DisplayBoard()
        {
            Console.WriteLine("  1 2 3 4 5 6");

            for (int y = 0; y < 6; y++)
            {
                Console.Write((char)('A' + y) + " ");
                for (int x = 0; x < 6; x++)
                {
                    switch (grid[y,x].Type)
                    {
                        case '-': break;
                        case 'Z': Console.ForegroundColor = ConsoleColor.Red; break;
                        case 'L': Console.ForegroundColor = ConsoleColor.Blue; break;
                        case 'T': Console.ForegroundColor = ConsoleColor.DarkGreen; break;
                        case 'o': Console.ForegroundColor = ConsoleColor.Green; break;
                        case '|': Console.ForegroundColor = ConsoleColor.DarkGray; break;
                        case 'v': Console.ForegroundColor = ConsoleColor.DarkMagenta; break;
                        case 'I': Console.ForegroundColor = ConsoleColor.White; break;
                        case 'i': Console.ForegroundColor = ConsoleColor.Magenta; break;
                        case '.': Console.ForegroundColor = ConsoleColor.DarkBlue; break;
                    }

                    Console.Write(grid[y, x].Type + " ");

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void AddCellToBoard(Cell cell)
        {
            grid[cell.Y, cell.X] = cell;
        }

        public void AddShapeToBoard(Shape shape, int startX, int startY)
        {
            foreach (Cell square in shape.Squares)
            {
                int gridX = startX + square.X;
                int gridY = startY + square.Y;
                Grid[gridY, gridX].Type = shape.Id; //Mutate the existing grid cell
            }
        }
    }
}