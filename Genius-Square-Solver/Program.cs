using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Genius_Square_Solver
{
    internal class Program
    {
        static readonly string[,] DICE_FACES = {
            { "A1", "C1", "D1", "D2", "E2", "F3" },
            { "A2", "B2", "C2", "A3", "B1", "B3" },
            { "C3", "D3", "E3", "B4", "C4", "D4" },
            { "E1", "F2", "F2", "B6", "A5", "A5" },
            { "A4", "B5", "C6", "C5", "D6", "F6" },
            { "E4", "F4", "E5", "F5", "D5", "E6" },
            { "F1", "F1", "F1", "A6", "A6", "A6" }
        };


        static void Main()
        {
            Menu();
        }

        private static void Menu()
        {
            Console.WriteLine("Welcome to the Genius Square Solver!" +
                "\n 1: Roll dice" +
                "\n 2: Input dice values" +
                "\n 3: Coming soon");
            string input = Console.ReadLine();
            Console.Clear();

            string[] blockers = new string[7];

            switch (input)
            {
                case "1": RollDice(ref blockers); break;
                case "2": InputDice(ref blockers); break;
                case "3": break;
                default: Console.WriteLine("Invalid input."); break;
            }

            Board board = GenerateBoard(blockers);
            board.DisplayBoard();

            Solver solver = new Solver(board);

            Console.ReadLine();
        }

        private static void RollDice(ref string[] blockers)
        {
            Random rnd = new Random();

            for (int i = 0; i < 7; i++)
            {
                blockers[i] = DICE_FACES[i, rnd.Next(0, 6)];
            }
        }

        private static void InputDice(ref string[] blockers)
        {
            Board board = GenerateBoard();
            List<int> unusedDice = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

            board.DisplayBoard();
            int statusTop = Console.CursorTop;   //The line under the board (where status is if exists)
            int promptTop = statusTop + 1;       //The prompt is one line lower
            const string prompt = "Enter a dice (eg. B4): ";
            int statusPrevLen = 0;
            int promptPrevLen = prompt.Length;

            void UpdateStatus(string message)
            {
                Console.SetCursorPosition(0, statusTop);
                Console.Write(message.PadRight(statusPrevLen));
                statusPrevLen = message.Length;
            }

            while (unusedDice.Count > 0)
            {
                //Wipe the whole prompt line, then rewrite it and put the cursor after the text
                Console.SetCursorPosition(0, promptTop);
                Console.Write(string.Empty.PadRight(promptPrevLen));

                Console.SetCursorPosition(0, promptTop);
                Console.Write(prompt);
                Console.SetCursorPosition(prompt.Length, promptTop); //Put cursor back after prompt

                string input = Console.ReadLine().Trim().ToUpper();
                promptPrevLen = Math.Max(promptPrevLen, prompt.Length + input.Length); //Account for text from previous invalid input
                int diceIndex = ValidateInput(input, unusedDice);

                if (diceIndex == -1)
                {
                    UpdateStatus($"Invalid input ({input}). {unusedDice.Count} left.");
                    continue;
                }

                unusedDice.Remove(diceIndex);
                blockers[diceIndex] = input;
                board.AddCellToBoard(Tools.StrToCell(input, true));

                Console.Clear();
                board.DisplayBoard();
                statusTop = Console.CursorTop;
                promptTop = statusTop + 1;
                statusPrevLen = 0;
                UpdateStatus($"Valid input. {unusedDice.Count} left.");
            }
        }

        private static int ValidateInput(string input, List<int> unusedDice)
        {
            foreach (int dice in unusedDice)
            {
                for (int face = 0; face < 6; face++)
                {
                    if (DICE_FACES[dice, face] == input)
                    {
                        return dice;
                    }
                }
            }

            return -1;
        }

        private static Board GenerateBoard(string[] blockers = null)
        {
            Cell[,] grid = new Cell[6, 6];

            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 6; x++)
                {
                    grid[y, x] = new Cell(y, x);
                }
            }

            if (blockers != null)
            {
                foreach (string blocker in blockers)
                {
                    Cell cell = Tools.StrToCell(blocker, true);
                    int y = cell.Y; int x = cell.X;

                    grid[y, x] = cell;
                }
            }

            return new Board(grid);
        }
    }
}