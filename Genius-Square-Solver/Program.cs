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

            switch (input)
            {
                case "1": break;
                case "2": break;
                case "3": break;
                default: Console.WriteLine("Invalid input."); break;
            }
        }
    }
}
