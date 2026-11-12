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
        static readonly string[][] DICE_FACES =
        {
            new[] { "A1", "C1", "D1", "D2", "E2", "F3" },
            new[] { "A2", "B2", "C2", "A3", "B1", "B3" },
            new[] { "C3", "D3", "E3", "B4", "C4", "D4" },
            new[] { "E1", "F2", "B6", "A5" },
            new[] { "A4", "B5", "C6", "C5", "D6", "F6" },
            new[] { "E4", "F4", "E5", "F5", "D5", "E6" },
            new[] { "F1", "A6" }
        };

        static readonly int[] DICE_RANGES = { 6, 6, 6, 4, 6, 6, 2 };


        static void Main()
        {
            while (true)
            {
                Menu();
            }
        }

        private static void Menu()
        {
            Console.Clear();

            //ANSI art:
            Console.WriteLine("\n ██████╗ ███████╗███╗   ██╗██╗██╗   ██╗███████╗   \r\n██╔════╝ ██╔════╝████╗  ██║██║██║   ██║██╔════╝   \r\n██║  ███╗█████╗  ██╔██╗ ██║██║██║   ██║███████╗   \r\n██║   ██║██╔══╝  ██║╚██╗██║██║██║   ██║╚════██║   \r\n╚██████╔╝███████╗██║ ╚████║██║╚██████╔╝███████║   \r\n ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝ ╚═════╝ ╚══════╝   \r\n                                                  \r\n███████╗ ██████╗ ██╗   ██╗ █████╗ ██████╗ ███████╗\r\n██╔════╝██╔═══██╗██║   ██║██╔══██╗██╔══██╗██╔════╝\r\n███████╗██║   ██║██║   ██║███████║██████╔╝█████╗  \r\n╚════██║██║▄▄ ██║██║   ██║██╔══██║██╔══██╗██╔══╝  \r\n███████║╚██████╔╝╚██████╔╝██║  ██║██║  ██║███████╗\r\n╚══════╝ ╚══▀▀═╝  ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝\r\n                                                  \r\n███████╗ ██████╗ ██╗    ██╗   ██╗███████╗██████╗  \r\n██╔════╝██╔═══██╗██║    ██║   ██║██╔════╝██╔══██╗ \r\n███████╗██║   ██║██║    ██║   ██║█████╗  ██████╔╝ \r\n╚════██║██║   ██║██║    ╚██╗ ██╔╝██╔══╝  ██╔══██╗ \r\n███████║╚██████╔╝███████╗╚████╔╝ ███████╗██║  ██║ \r\n╚══════╝ ╚═════╝ ╚══════╝ ╚═══╝  ╚══════╝╚═╝  ╚═╝ \r\n                                                  ");

            int cursorTop = Console.CursorTop;

            Console.WriteLine("" +
                "\n 1: Single solver" +
                "\n 2: Multi solver" +
                "\n 3: Exit");

            string input = Console.ReadLine();

            string[] blockers = new string[7];       

            switch (input)
            {
                case "1": StartSingleSolve(cursorTop); break;
                case "2": MultiSolve(GetMultiSolveSettings(/*cursorTop*/)); break;
                case "3": Environment.Exit(0); break;
                default:
                    Console.Write("Invalid input. (Enter to continue)");
                    Console.ReadLine(); return;
            }

            Console.Write("(Press enter to return to menu)");
            Console.ReadLine();
        }

        private static void StartSingleSolve(int cursorTop)
        {
            string[] blockers = new string[7];

            //Reset to sub-menu display:
            Console.SetCursorPosition(0, cursorTop);
            for (int _ = 0; _ < 4; _++)
            {
                Console.Write(string.Empty.PadRight(Console.WindowWidth - 1));
            }
            Console.SetCursorPosition(0, cursorTop);

            Console.WriteLine("" +
                "\n 1: Roll dice" +
                "\n 2: Input dice" +
                "\n 3: Input seed" +
                "\n 4: Return to menu");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1": RollDice(ref blockers); break;
                case "2": InputDice(ref blockers); break;
                case "3": InputSeed(ref blockers); break;
                case "4": return;
                default:
                    Console.Write("Invalid input. (Enter to continue)");
                    Console.ReadLine(); return;
            }

            Console.Clear();
            HandleSolve(blockers);
        }

        private static void HandleSolve(string[] blockers, bool isMulti = false)
        {
            Console.WriteLine("Blank board:");
            Board board = GenerateBoard(blockers);
            board.DisplayBoard();

            if (!isMulti)
            {
                //Check if user is happy to proceed:
                Console.Write("Proceed to solve (Y/n)? ");
                string toProceed = Console.ReadLine();
                Console.Clear();

                if (toProceed.ToLower() != "n")
                {
                    new Solver(board);
                }
                else
                {
                    return;
                }
            }
            else
            {
                Console.WriteLine("Solution:");
                new Solver(board);
            }
        }

        private static void MultiSolve(int solCount)
        {
            //For now, just solve random solutions
            string[] blockers = new string[7];

            for (int i = 0; i < solCount; i++)
            {
                RollDice(ref blockers);
                HandleSolve(blockers, true);
            }
        }

        private static int GetMultiSolveSettings()
        {
            Console.Write("Enter a number of solutions to find: ");
            int solCount = 0;

            try
            {
                solCount = Convert.ToInt16(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }


            Console.Clear();
            return solCount;
        }

        private static void RollDice(ref string[] blockers)
        {
            Random rnd = new Random();

            for (int i = 0; i < 7; i++)
            {
                blockers[i] = DICE_FACES[i][rnd.Next(0, DICE_RANGES[i])];
            }
        }

        private static void InputDice(ref string[] blockers)
        {
            Console.Clear();
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
                for (int face = 0; face < DICE_RANGES[dice]; face++)
                {
                    if (DICE_FACES[dice][face] == input)
                    {
                        return dice;
                    }
                }
            }

            return -1;
        }

        private static void InputSeed(ref string[] blockers)
        {
            Console.Clear();

            int input = -1;
            do
            {
                Console.Write("Enter a seed (eg. 37): ");

                try
                {
                    input = int.Parse(Console.ReadLine());

                    if (input < 1 || input > 62208)
                    {
                        throw new Exception("Invalid seed.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (input < 1 || input > 62208);

            int[] diceIndexes = SeedToDice(input);

            for (int i = 0; i < 7; i++)
            {
                blockers[i] = DICE_FACES[i][diceIndexes[i]];
            }
        }

        private static int[] SeedToDice(int input)
        {
            int n = input - 1;

            int[] result = new int[7];

            for (int i = 6; i >= 0; i--)
            {
                result[i] = n % DICE_RANGES[i];
                n /= DICE_RANGES[i];
            }

            return result;
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