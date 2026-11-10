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

            string[] blockers = new string[7];

            switch (input)
            {
                case "1": RollDice(ref blockers); break;
                case "2": InputDice(ref blockers); break;
                case "3": break;
                default: Console.WriteLine("Invalid input."); break;
            }
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
            List<int> unusedDice = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

            while (unusedDice.Count > 0)
            {
                Console.Write("Enter a dice (eg. B4): ");
                string input = Console.ReadLine().Trim().ToUpper();

                int diceIndex = ValidateInput(input, unusedDice);

                if (diceIndex != -1)
                {
                    unusedDice.Remove(diceIndex);
                    blockers[diceIndex] = input;
                    Console.WriteLine($"Valid input. {unusedDice.Count} left.");
                }
                else
                {
                    Console.WriteLine($"Invalid input ({input}) {unusedDice.Count} left.");
                }
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
    }
}