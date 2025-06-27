using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Utilities
{
    public static class ConsoleHelper
    {
        public static void WriteHeader(string title)
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine($"   {title.ToUpper()}");
            Console.WriteLine("═══════════════════════════════════════");
            Console.WriteLine();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✔ {message}");
            Console.ResetColor();
        }

        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"✘ {message}");
            Console.ResetColor();
        }

        public static string ReadInput(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine()?.Trim();
        }

        public static void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        internal static void WriteWarning(string v)
        {
            throw new NotImplementedException();
        }
    }
}
