using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            switch (input)
            {
                case "1": StartSingleSolve(cursorTop); break;
                case "2": StartMultiSolve(cursorTop); break;
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
            bool findAll = false;

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
            Console.Write("\nDo you want to find all solutions for each puzzle (y/N): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                findAll = true;
            }

            Console.Clear();

            Solver solver = new Solver(findAll, false);
            HandleSolve(blockers, solver);
        }

        private static int HandleSolve (string[] blockers, Solver solver, int seed = 0)
        {
            Board board = GenerateBoard(blockers);

            if (!solver.IsMulti)
            {
                Console.WriteLine("Blank board:");
                board.DisplayBoard();

                //Check if user is happy to proceed:
                Console.Write("Proceed to solve (Y/n)? ");
                string toProceed = Console.ReadLine();
                Console.Clear();

                if (toProceed.ToLower() != "n")
                {
                    if (seed == 0)
                        seed = BlockersToSeed(blockers);
                    
                    Console.WriteLine($"Seed {seed}:");
                    solver.ManageSolve(board);

                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                Console.WriteLine($"Seed {seed}:");
                
                solver.ManageSolve(board);

                return solver.Solutions.Count;
            }
        }

        private static void StartMultiSolve(int cursorTop)
        {
            string[] blockers = new string[7];
            List<(int seed, int count)> results = new List<(int seed, int count)>();

            //Reset to sub-menu display:
            Console.SetCursorPosition(0, cursorTop);
            for (int _ = 0; _ < 5; _++)
            {
                Console.Write(string.Empty.PadRight(Console.WindowWidth - 1));
            }
            Console.SetCursorPosition(0, cursorTop);

            Console.WriteLine("" +
                "\n 1: Input solve settings" +
                "\n 2: Coming soon..." +
                "\n 3: Return to menu");

            string input = Console.ReadLine();

            int start, end;
            bool findAll = false;

            switch (input)
            {
                case "1": InputSolveBounds(out start, out end, ref findAll); break;
                case "2": /*Use a config file for solve*/
                case "3": return;
                default:
                    Console.Write("Invalid input. (Enter to continue)");
                    Console.ReadLine(); return;
            }

            Console.Clear();

            int tempCount;
            Stopwatch timer = Stopwatch.StartNew();

            //Setup solver:
            Solver solver = new Solver(findAll, true);

            while (start <= end)
            {
                int[] diceIndexes = SeedToDice(start);

                for (int i = 0; i < 7; i++)
                {
                    blockers[i] = DICE_FACES[i][diceIndexes[i]];
                }

                tempCount = HandleSolve(blockers, solver, start);
                results.Add((start, tempCount));

                start++;
            }

            timer.Stop();

            //Summary:
            int minCount = int.MaxValue, maxCount = 0, total = 0;
            int minSeed = 0, maxSeed = 0;

            foreach (var (seed, count) in results)
            {
                total += count;

                if (count < minCount)
                {
                    minCount = count;
                    minSeed = seed;
                }

                if (count > maxCount)
                {
                    maxCount = count;
                    maxSeed = seed;
                } 
            }

            Console.WriteLine($"\n--- Summary: {results.Count} puzzles ---");
            Console.WriteLine($"Time: {timer.Elapsed.TotalSeconds:N2}s at {results.Count / timer.Elapsed.TotalSeconds:N1} seeds/sec\n");
            
            if (findAll)
            {
                Console.WriteLine($"Min: {minCount:N0} solutions (seed {minSeed})");
                Console.WriteLine($"Max: {maxCount:N0} solutions (seed {maxSeed})");
                Console.WriteLine($"Avg: {(double)total / results.Count:N1} solutions\n");
            }
        }

        private static void InputSolveBounds(out int start, out int end, ref bool findAll)
        {
            Console.Clear();

            while (true)
            {
                Console.Write("Enter first puzzle to solve (>= 1 and <= 62208): ");
                if (!int.TryParse(Console.ReadLine(), out start) || start < 1 || start > 62208)
                {
                    Console.WriteLine("First puzzle must be a whole number between 1 and 62208.\n");
                    continue;
                }

                Console.Write("Enter last puzzle to solve (>= first number and <= 62208): ");
                if (!int.TryParse(Console.ReadLine(), out end) || end < 1 || end > 62208)
                {
                    Console.WriteLine($"Last puzzle must be a whole number between {start} and 62208.\n");
                    continue;
                }

                if (end < start)
                {
                    Console.WriteLine($"Last puzzle ({end}) must be greater than or equal to the first ({start}).\n");
                    continue;
                }

                break;
            }

            Console.Write("\nDo you want to find all solutions for each puzzle (y/N): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                findAll = true;
            }

            Console.Clear();
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

        private static int DiceToSeed(int[] diceFaces)
        {
            int n = 0;

            for (int i = 0; i < 7; i++)
            {
                n = n * DICE_RANGES[i] + diceFaces[i];
            }

            return n + 1;   //As seeds are 1-based
        }

        private static int BlockersToSeed(string[] blockers)
        {
            int[] diceFaces = new int[7];

            for (int die = 0; die < 7; die++)
            {
                int found = -1;

                foreach (string blocker in blockers)
                {
                    int index = Array.IndexOf(DICE_FACES[die], blocker.Trim().ToUpper());
                    if (index != -1)
                    {
                        if (found != -1)
                            throw new ArgumentException("Two blockers belong to die " + die);
                        found = index;
                    }
                }

                if (found == -1)
                    throw new ArgumentException("No blocker belongs to die " + die);

                diceFaces[die] = found;
            }

            return DiceToSeed(diceFaces);
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