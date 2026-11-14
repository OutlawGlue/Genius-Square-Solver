using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Solver
    {
        readonly List<Shape> defaultShapes = new List<Shape>
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

        private bool findAll;
        private bool isMulti;

        public List<char[,]> Solutions { get; private set; } = new List<char[,]>();

        private bool solved = false;

        public Solver(bool findAll, bool isMulti)
        {
            this.findAll = findAll;
            this.isMulti = isMulti;

            foreach (Shape shape in defaultShapes)
                shape.CalculateOrientations(); // !!!CALLS CALCULATE ORIENTATIONS FOR EVERY SEED IN A MULTI!!!
        }

        public bool Solved
        {
            get { return solved; }
            private set { solved = value; }
        }

        public bool IsMulti
        {
            get { return isMulti; }
        }

        public void ManageSolve(Board board)
        {
            List<Shape> unusedShapes = new List<Shape>(defaultShapes);

            if (findAll)
            {
                Solve(board, unusedShapes);
                Console.WriteLine($"Found {Solutions.Count:N0} solutions.");

                if (!isMulti)
                {
                    //Display some solutions:
                    for (int i = 0; i < Math.Min(3, Solutions.Count); i++)
                    {
                        DisplayRaw(Solutions[i]);
                    }
                }
            }
            else
            {
                Solved = Solve(board, unusedShapes);
                board.DisplayBoard();
                if (!Solved)
                    Console.WriteLine("No solution exists.");
            }
        }

        private bool Solve(Board board, List<Shape> shapes)
        {
            Cell[,] grid = board.Grid;

            int x, y;
            (x, y) = FindEmptyCell(board.Grid);

            //Check if board is full:
            if (x == -1)
            {
                if (findAll)
                {
                    Solutions.Add(SnapshotBoard(grid));
                    return false; //Continue searching
                }
                return true;
            }
   

            for (int i = 0; i < shapes.Count; i++)
            {
                Shape shape = shapes[i];

                foreach (Cell[] orientation in shape.Orientations)
                {
                    if (!CheckShapeFit(grid, orientation, x, y)) continue;

                    PlaceShape(board, orientation, x, y);
                    shapes.RemoveAt(i);

                    if (Solve(board, shapes))
                    {
                        if (!findAll)
                        {
                            return true;
                        }
                    }

                    UnplaceShape(board, orientation, x, y);
                    shapes.Insert(i, shape);
                }
            }
            return false;
        }

        private static char[,] SnapshotBoard(Cell[,] grid)
        {
            char[,] gridCopy = new char[6, 6];

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    gridCopy[row, col] = grid[row, col].Type;
                }
            }
                    
            return gridCopy;
        }

        public static void DisplayRaw(char[,] board)
        {
            Console.WriteLine("  1 2 3 4 5 6");

            for (int y = 0; y < 6; y++)
            {
                Console.Write((char)('A' + y) + " ");
                for (int x = 0; x < 6; x++)
                {
                    switch (board[y, x])
                    {
                        case '-': break;
                        case 'x': Console.ForegroundColor = ConsoleColor.Yellow; break;  // blockers
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

                    Console.Write(board[y, x] + " ");

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
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

                if (gridX < 0 || gridX > 5 || gridY < 0 || gridY > 5)
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
