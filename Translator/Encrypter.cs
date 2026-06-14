using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Translator
{
    public class Encrypter
    {
        private static readonly Dictionary<char, string> MorseMap = new()
        {
            ['A'] = ".-",
            ['B'] = "-...",
            ['C'] = "-.-.",
            ['D'] = "-..",
            ['E'] = ".",
            ['F'] = "..-.",
            ['G'] = "--.",
            ['H'] = "....",
            ['I'] = "..",
            ['J'] = ".---",
            ['K'] = "-.-",
            ['L'] = ".-..",
            ['M'] = "--",
            ['N'] = "-.",
            ['O'] = "---",
            ['P'] = ".--.",
            ['Q'] = "--.-",
            ['R'] = ".-.",
            ['S'] = "...",
            ['T'] = "-",
            ['U'] = "..-",
            ['V'] = "...-",
            ['W'] = ".--",
            ['X'] = "-..-",
            ['Y'] = "-.--",
            ['Z'] = "--..",
            ['0'] = "-----",
            ['1'] = ".----",
            ['2'] = "..---",
            ['3'] = "...--",
            ['4'] = "....-",
            ['5'] = ".....",
            ['6'] = "-....",
            ['7'] = "--...",
            ['8'] = "---..",
            ['9'] = "----.",
        };

        public void Encrypt(string message)
        {
            Console.WriteLine("Which cipher do you want to use?");
            Console.WriteLine("Morse Code(M)");
            Console.WriteLine("Caesar Cipher(C)");
            Console.WriteLine("Vigener(V):");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "M" or "m":
                    MorseEncrypt(message);
                    break;
                case "C" or "c":
                    CeasarEncrypt(message);
                    break;
                case "V" or "v":
                    VigenerEncrypt(message);
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        public void MorseEncrypt(string message) 
        {
            // Pokud je prázdný nebo jen mezery, vypíšeme pouze koncové lomítka
            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("///");
                return;
            }

            // Rozdělíme text na slova (více mezer ignorujeme)
            var words = message.ToUpperInvariant()
                               .Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            // Pro každé slovo vytvoříme posloupnost morseových písmen oddělených '/'
            var wordSegments = new List<string>(words.Length);
            foreach (var word in words)
            {
                var letterSegments = new List<string>(word.Length);
                foreach (var ch in word)
                {
                    if (MorseMap.TryGetValue(ch, out var morse))
                        letterSegments.Add(morse);
                    else if (ch == '.')
                        letterSegments.Add("///");
                    else
                        letterSegments.Add("?"); // nepodporovaný znak jako '?'
                }
                wordSegments.Add(string.Join("/", letterSegments));
            }

            // Mezi slovy použijeme '//', na konci věty '///'
            var result = string.Join("//", wordSegments) + "///";
            // Zajistí, že žádná sekvence '/' nepřesahuje 3 — pokud ano, zkrátí na přesně '///'
            result = Regex.Replace(result, "/{4,}", "///");
            Console.WriteLine(result);
        }

        public void CeasarEncrypt(string message)
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
                    char encryptedChar = (char)(((c + shift - offset) % 26) + offset);
                    sb.Append(encryptedChar);
                }
                else
                {
                    sb.Append(c);
                }
            }
            Console.WriteLine(sb.ToString());
        }

        public void VigenerEncrypt(string message)
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
                    char encryptedChar = (char)(((c + 1 + keyShift - offset) % 26) + offset);
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
