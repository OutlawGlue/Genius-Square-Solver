using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Shape
    {
        char id;
        Cell[] squares;

        public Shape(char id, params Cell[] squares)
        {
            this.id = id;
            this.squares = squares;

            foreach (Cell square in squares)
            {
                square.Type = id;
            }
        }

        public Cell[] Squares
        {
            get { return squares; }
        }

        public char Id
        {
            get { return id; }
        }

        public void RotateRight()
        {
            Cell[] newSquares = new Cell[squares.Length];

            for (int i = 0; i < squares.Length; i++)
            {
                Cell square = squares[i];
                newSquares[i] = new Cell(square.Y, -square.X, id);
            }

            squares = newSquares;

            Normalise();
        }

        public void Flip()
        {
            Cell[] newSquares = new Cell[squares.Length];

            for (int i = 0; i < squares.Length; i++)
            {
                Cell square = squares[i];
                newSquares[i] = new Cell(-square.X, square.Y, id);
            }

            squares = newSquares;

            Normalise();
        }

        public void Normalise()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;

            foreach (Cell square in squares)
            {
                if (square.X < minX) minX = square.X;
                if (square.Y < minY) minY = square.Y;
            }

            // If already normalised (mins are 0), return as-is
            if (minX == 0 && minY == 0)
                return;

            Cell[] shifted = new Cell[squares.Length];
            for (int i = 0; i < squares.Length; i++)
            {
                Cell square = squares[i];
                shifted[i] = new Cell(square.X - minX, square.Y - minY, id);
            }

            squares = shifted;
        }
    }
}
