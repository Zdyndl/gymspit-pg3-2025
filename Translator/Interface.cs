using System;
using static System.Net.Mime.MediaTypeNames;

namespace Translator
{
    internal class Interface
    {
        public void Run()
        {
            Console.Title = "Translator";
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=== Translator ===");
                Console.ResetColor();
                Console.WriteLine("Choose action:");
                Console.WriteLine("[1] Encrypt message");
                Console.WriteLine("[2] Decrypt message");
                Console.WriteLine("[3] Information about program");
                Console.WriteLine("[4] End");
                Console.Write("Your Choice: ");

                var key = Console.ReadKey(true);
                Console.WriteLine();
                switch (char.ToUpperInvariant(key.KeyChar))
                {
                    case '1':
                        {
                            string message = PromptMessage("encrypt");
                            if (message is null) break;
                            var encrypter = new Encrypter();
                            encrypter.Encrypt(message);
                            Pause();
                            break;
                        }
                    case '2':
                        {
                            string message = PromptMessage("decrypt");
                            if (message is null) break;
                            var decrypter = new Decrypter();
                            decrypter.Decrypt(message);
                            Pause();
                            break;
                        }
                    case '3':
                        ShowHelp();
                        Pause();
                        break;
                    case '4':
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Invalid choice, try again.");
                        Console.ResetColor();
                        Pause();
                        break;
                }
            }
        }

        private static string? PromptMessage(string action)
        {
            Console.Clear();
            Console.WriteLine($"Enter the message you want to {action} (leave blank to cancel):");
            Console.Write("> ");
            string? message = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Action cancelled.");
                return null;
            }
            return message;
        }

        private static void ShowHelp()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Help:");
            Console.ResetColor();
            Console.WriteLine(" - The program supports three ciphers: Morse, Caesar, Vigenère.");
            Console.WriteLine(" - When encrypting/decrypting, you will be prompted to choose a specific cipher and, if applicable, enter a key..");
            Console.WriteLine(" - Morse: separate letters with '/' and words with '//'. Output ends with '///'.");
            Console.WriteLine(" - Caesar: enter the shift value 1-25.");
            Console.WriteLine(" - Vigenère: enter a keyword using only letters (A-Z).");
            Console.WriteLine();
            Console.WriteLine("Tip: After performing an action, press any key to return to the main menu.");
        }

        private static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the menu....");
            Console.ReadKey(true);
        }
    }
}
