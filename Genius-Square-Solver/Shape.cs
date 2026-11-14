using System.Collections.Generic;

namespace Genius_Square_Solver
{
    internal class Shape
    {
        private readonly char id;
        private Cell[] squares;

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

        public List<Cell[]> Orientations 
        { 
            get; 
            private set; 
        }

        public void CalculateOrientations()
        {
            Orientations = new List<Cell[]>();
            HashSet<string> seen = new HashSet<string>();

            for (int flipRound = 0; flipRound < 2; flipRound++) //0 = not, 1 = flipped
            {
                //Create clone each round
                Shape cloneShape = Clone();
                if (flipRound == 1)
                    cloneShape.Flip();

                for (int rotation = 0; rotation < 4; rotation++)
                {
                    string fingerprint = cloneShape.Key();

                    //If clone is new, add:
                    if (seen.Add(fingerprint))
                    {
                        Orientations.Add(AnchorToFirstSquare(cloneShape.CopySquares()));
                    }

                    //Else rotate and retry
                    cloneShape.RotateRight();
                }
            }
        }

        public string Key()
        {
            //Build "x,y" text for every square, then sort + join
            List<string> parts = new List<string>();
            foreach (Cell square in squares)
            {
                parts.Add(square.X + "," + square.Y);
            }
            parts.Sort();

            return string.Join(";", parts);
        }

        //Create a copy of this shape
        private Shape Clone()
        {
            Cell[] copy = new Cell[squares.Length];
            for (int i = 0; i < squares.Length; i++)
            {
                Cell s = squares[i];
                copy[i] = new Cell(s.X, s.Y, id);
            }

            //Skips orientation calc
            return new Shape(id, copy);   
        }

        //Copy the squares, not the shape
        private Cell[] CopySquares()
        {
            Cell[] copy = new Cell[squares.Length];
            for (int i = 0; i < squares.Length; i++)
            {
                Cell s = squares[i];
                copy[i] = new Cell(s.X, s.Y, id);
            }
            return copy;
        }

        //Shift a set of squares so the first square sits at (0,0)
        private Cell[] AnchorToFirstSquare(Cell[] cells)
        {
            Cell anchor = cells[0];
            foreach (Cell cell in cells)
            {
                if (cell.Y < anchor.Y || (cell.Y == anchor.Y && cell.X < anchor.X))
                    anchor = cell;
            }

            Cell[] shifted = new Cell[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                shifted[i] = new Cell(cells[i].X - anchor.X, cells[i].Y - anchor.Y, id);
            }
            return shifted;
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
