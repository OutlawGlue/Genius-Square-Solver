using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Solver
    {
        readonly List<Shape> unusedShapes = new List<Shape>
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

        bool solved = false;

        public Solver(Board board)
        {
            //Precalc shape rotations:
            foreach (Shape shape in unusedShapes)
                shape.CalculateOrientations();

            Solved = Solve(board, unusedShapes);

            board.DisplayBoard();
            if (!Solved)
                Console.WriteLine("No solution exists.");
        }

        public bool Solved
        {
            get { return solved; }
            private set { solved = value; }
        }

        private bool Solve(Board board, List<Shape> shapes)
        {
            Cell[,] grid = board.Grid;

            int x, y;
            (x, y) = FindEmptyCell(board.Grid);

            if (x == -1) return true;   //Check if board is full

            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = shapes[i];

                foreach (Cell[] orientation in shape.Orientations)
                {
                    if (!CheckShapeFit(grid, orientation, x, y)) continue;

                    PlaceShape(board, orientation, x, y);
                    shapes.RemoveAt(i);

                    if (Solve(board, shapes))
                        return true;

                    UnplaceShape(board, orientation, x, y);
                    shapes.Insert(i, shape);
                }
            }
            return false;
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

            return (-1, -1);
        }

        public static bool CheckShapeFit(Cell[,] grid, Cell[] squares, int startX, int startY)
        {
            foreach (Cell square in squares)
            {
                int gridX = startX + square.X;
                int gridY = startY + square.Y;

                if (gridX > 5 || gridY > 5)
                    return false;

                if (grid[gridY, gridX].Type != '-')
                    return false;
            }
            return true;
        }

        private static void PlaceShape(Board board, Cell[] squares, int startX, int startY)
        {
            foreach (Cell square in squares)
            {
                int y = startY + square.Y;
                int x = startX + square.X;
                board.Grid[y, x].Type = square.Type;
            }
        }

        private static void UnplaceShape(Board board, Cell[] squares, int startX, int startY)
        {
            foreach (Cell square in squares)
            {
                int y = startY + square.Y;
                int x = startX + square.X;
                board.Grid[y, x].Type = '-';
            }
        }
    }
}
