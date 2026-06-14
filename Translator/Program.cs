using System;

namespace Translator
{
    internal static class Program
    {
        private static void Main() // následující syntax mám na doporučení od copilota
        {
            var ui = new Interface();

            while (true)
            {
                ui.Run();
                Console.WriteLine();
                Console.WriteLine("Press any key to return to the main menu or press Q to leave the program.");
                var next = (Console.ReadLine() ?? string.Empty).Trim();
                if (string.Equals(next, "Q", StringComparison.OrdinalIgnoreCase))
                    break;
                Console.Clear();
            }
        }
    }
}