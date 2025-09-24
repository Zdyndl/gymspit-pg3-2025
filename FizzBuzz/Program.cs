while (true)
{
    Console.Write("Zadejte kladné celé číslo N: ");
    string? input = Console.ReadLine();
    int result;
    bool success = int.TryParse(input, out result);
    if (success && result > 0)
    {
        for (int i = 1; i <= result; i++)
        {
            switch (i % 3, i % 5)
            {
                case (0, 0):
                    Console.WriteLine("FizzBuzz");
                    break;
                case (0, _):
                    Console.WriteLine("Fizz");
                    break;
                case (_, 0):
                    Console.WriteLine("Buzz");
                    break;
                default:
                    Console.WriteLine(i);
                    break;
            }
        }
    }
    else
    {
        Console.WriteLine("Zadaná hodnota není kladné číslo.");
    }
}