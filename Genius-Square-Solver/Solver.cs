using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Solver
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

        public Solver(Board board)
        {
            while (unusedShapes.Count > 0)
            {
                int before = unusedShapes.Count;
                SingleSolver(board);

                if (unusedShapes.Count == before)
                {
                    //Check if made no progress this call (dead end)
                    board.DisplayBoard();
                    Console.WriteLine("Stuck: no shape fits the first empty cell.");
                    Console.WriteLine("Remaining pieces: " +
                        string.Join(", ", unusedShapes.Select(s => s.Id)));
                    break;
                }
            }
        }

        public void SingleSolver(Board board)
        {
            int x, y;
            (x, y) = FindEmptyCell(board.Grid);

            if (x == -1) return;   //Check if board is full

            Cell[,] grid = board.Grid;

            for (int i = 0; i < unusedShapes.Count; i++)
            {
                Shape shape = unusedShapes[i];
                bool placed = false;

                for (int flip = 0; flip < 2 && !placed; flip++)
                {
                    for (int rot = 0; rot < 4 && !placed; rot++)
                    {
                        if (CheckShapeFit(grid, shape, x, y))
                        {
                            board.AddShapeToBoard(shape, x, y);
                            Console.WriteLine($"placed {shape.Id} at ({x},{y}), {unusedShapes.Count - 1} shapes left");
                            placed = true;
                        }
                        else
                        {
                            shape.RotateRight();
                        }
                    }
                    if (!placed)
                        shape.Flip();
                }

                if (placed)
                {
                    unusedShapes.RemoveAt(i); //Remove from available shapes
                    return;
                }
            }
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
                int gridX = startX + square.X;
                int gridY = startY + square.Y;

                //Check if off the board 
                if (gridX > 5 || gridY > 5)
                    return false;

                if (grid[gridY, gridX].Type != '-')
                    return false;
            }

            return true;
        }
    }
}
