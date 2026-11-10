using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Solver
    {
        public Solver()
        {

        }

        public static void SingleSolver(Board board)
        {
            List<Shape> unusedShapes = new List<Shape>
            {
                new Shape('Z', new Cell(0, 0), new Cell(0, 1), new Cell(1, 1), new Cell(1, 2)),  // S/Z
                new Shape('L', new Cell(0, 0), new Cell(0, 1), new Cell(0, 2), new Cell(1, 2)),  // L
                new Shape('T', new Cell(0, 0), new Cell(0, 1), new Cell(0, 2), new Cell(1, 1)),  // T
                new Shape('o', new Cell(0, 0), new Cell(0, 1), new Cell(1, 0), new Cell(1, 1)),  // 2x2 square
                new Shape('|', new Cell(0, 0), new Cell(0, 1), new Cell(0, 2), new Cell(0, 3)),  // bar tetromino
                new Shape('v', new Cell(0, 0), new Cell(0, 1), new Cell(1, 0)),              // corner tromino
                new Shape('I', new Cell(0, 0), new Cell(0, 1), new Cell(0, 2)),              // bar tromino
                new Shape('i', new Cell(0, 0), new Cell(0, 1)),                              // straight domino
                new Shape('.', new Cell(0, 0)),                                              // monomino
            };

            int x, y;
            (x, y) = FindEmptyCell(board.Grid);

            Cell[,] grid = board.Grid;

            foreach (Shape shape in unusedShapes)
            {
                bool placed = false;

                for (int flip = 0; flip < 2 && !placed; flip++)
                {
                    for (int rot = 0; rot < 4 && !placed; rot++)
                    {
                        if (CheckShapeFit(grid, shape, x, y))
                        {
                            Console.WriteLine($"{shape.Id} fits!");
                            board.AddShapeToBoard(shape);
                            board.DisplayBoard();
                            Console.ReadLine();
                            placed = true;
                        }
                        else
                        {
                            shape.RotateRight();
                        }
                    }
                    if (!placed)
                        shape.Flip(); //After 4 rotations, flip and retry
                }
            }

            //temp code:
            //Cell tempcell = new Cell(x, y);
            //string coords = Tools.CellToStr(tempcell);
            //Console.WriteLine($"First empty cell is {coords}");
            //Console.ReadLine();
        }

        public static (int, int) FindEmptyCell(Cell[,] grid)
        {
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    if (grid[y,x].Type == '-')
                    {
                        return (x,y);
                    }
                }
            }

            Console.WriteLine("Solved, no empty squares.");
            return (-1, -1);
        }

        public static bool CheckShapeFit(Cell[,] grid, Shape shape, int startX, int startY)
        {
            foreach (Cell square in shape.Squares)
            {
                //ie cell we check = from initial start cell
                // + square's offset from anchor
                int gridX = startX + square.X;
                int gridY = startY + square.Y;

                if (grid[gridY, gridX].Type != '-')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
