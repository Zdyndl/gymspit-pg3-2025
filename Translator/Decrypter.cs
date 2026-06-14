using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    public class Decrypter
    {
        private static readonly Dictionary<string, char> MorseMap = new()
        {
            [".-"] = 'A',
            ["-..."] = 'B',
            ["-.-."] = 'C',
            ["-.."] = 'D',
            ["."] = 'E',
            ["..-."] = 'F',
            ["--."] = 'G',
            ["...."] = 'H',
            [".."] = 'I',
            [".---"] = 'J',
            ["-.-"] = 'K',
            [".-.."] = 'L',
            ["--"] = 'M',
            ["-."] = 'N',
            ["---"] = 'O',
            [".--."] = 'P',
            ["--.-"] = 'Q',
            [".-."] = 'R',
            ["..."] = 'S',
            ["-"] = 'T',
            ["..-"] = 'U',
            ["...-"] = 'V',
            [".--"] = 'W',
            ["-..-"] = 'X',
            ["-.--"] = 'Y',
            ["--.."] = 'Z',
            ["-----"] = '0',
            [".----"] = '1',
            ["..---"] = '2',
            ["...--"] = '3',
            ["....-"] = '4',
            ["....."] = '5',
            ["-...."] = '6',
            ["--..."] = '7',
            ["---.."] = '8',
            ["----."] = '9',
        };

        public void Decrypt(string message)
        {
            Console.WriteLine("Which cipher are you using?");
            Console.WriteLine("Morse Code(M)");
            Console.WriteLine("Caesar Cipher(C)");
            Console.WriteLine("Vigener(V)");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "M" or "m":
                    MorseDecrypt(message);
                    break;
                case "C" or "c":
                    CeasarDecrypt(message);
                    break;
                case "V" or "v":
                    VigenerDecrypt(message);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        public void MorseDecrypt(string message) // s tímhle mi trochu pomohl copilot
        {
            if (message == null)
            {
                Console.WriteLine();
                return;
            }

            // Odstraníme mezery v původním textu (mezery mezi znaky se ignorují)
            string input = new string(message.Where(c => c != ' ').ToArray());

            var sb = new StringBuilder();
            var token = new StringBuilder();
            int i = 0;
            while (i < input.Length)
            {
                char ch = input[i];
                if (ch == '.' || ch == '-')
                {
                    token.Append(ch);
                    i++;
                }
                else if (ch == '/')
                {
                    // Pokud máme nasbíraný morse token, dekódujeme ho
                    if (token.Length > 0)
                    {
                        string code = token.ToString();
                        if (MorseMap.TryGetValue(code, out char decoded))
                            sb.Append(decoded);
                        else
                            sb.Append("?"); // neznámý kód
                        token.Clear();
                    }

                    // Spočítáme počet po sobě jdoucích '/'
                    int j = i;
                    while (j < input.Length && input[j] == '/') j++;
                    int slashCount = j - i;

                    if (slashCount >= 3)
                    {
                        sb.Append(".");
                    }
                    else if (slashCount == 2)
                    {
                        // // = mezera mezi slovy
                        sb.Append(' ');
                    }
                    // single slash = oddělení písmen -> nic extra nepřidáme

                    i = j;
                }
                else
                {
                    // Ignorujeme libovolné další znaky
                    i++;
                }
            }

            // Na konci zpracujeme případný poslední token bez oddělovače
            if (token.Length > 0)
            {
                string code = token.ToString();
                if (MorseMap.TryGetValue(code, out char decoded))
                    sb.Append(decoded);
                else
                    sb.Append('?');
            }

            Console.WriteLine(sb.ToString());
        }

        public void CeasarDecrypt(string message)
        {
            Console.WriteLine("Enter the shift value (1-25):");
            if (!int.TryParse(Console.ReadLine(), out int shift) || shift < 1 || shift > 25)
            {
                Console.WriteLine("Invalid shift value. Please enter a number between 1 and 25.");
                return;
            }
            var sb = new StringBuilder();
            foreach (char c in message)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    char encryptedChar = (char)(((c - shift - offset) % 26 + 26) % 26 + offset);
                    sb.Append(encryptedChar);
                }
                else
                {
                    sb.Append(c);
                }
            }
            Console.WriteLine(sb.ToString());
        }

        public void VigenerDecrypt(string message)
        {
            Console.WriteLine("Enter the keyword:");
            string keyword = Console.ReadLine();
            if (string.IsNullOrEmpty(keyword))
            {
                Console.WriteLine("Keyword cannot be empty.");
                return;
            }
            if (!keyword.All(char.IsLetter))
            {
                Console.WriteLine("Keyword must contain only letters (A-Z).");
                return;
            }

            var sb = new StringBuilder();
            int keywordIndex = 0;
            foreach (char c in message)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    char keyChar = char.ToUpper(keyword[keywordIndex % keyword.Length]);
                    int keyShift = keyChar - 'A';
                    char encryptedChar = (char)(((c - 1 - keyShift - offset) % 26 + 26) % 26 + offset);
                    sb.Append(encryptedChar);
                    keywordIndex++;
                }
                else
                {
                    sb.Append(c);
                }
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
