using System;

while (true)
{
    PrintMenu();
    char operace = ReadOperation();
    if (operace == '\0')
    {
        Console.WriteLine("Konec programu");
        break;
    }

    Console.WriteLine("Zadej první celé číslo:");
    double čislo1 = ReadDouble();

    Console.WriteLine("Zadej druhé celé číslo:");
    double čislo2 = ReadDouble(nonZero: operace == '/');

    try
    {
        double výsledek = Compute(operace, čislo1, čislo2);
        PrintResult(operace, čislo1, čislo2, výsledek);
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Chyba: Dělení nulou není povoleno.");
    }
}

void PrintMenu()
{
    Console.WriteLine("Vyber operaci jednu z operací: +, -, *, /, konec");
}

char ReadOperation()
{
    while (true)
    {
        string? input = Console.ReadLine();
        if (input == null) continue;

        input = input.Trim();
        if (input.Equals("konec", StringComparison.OrdinalIgnoreCase))
            return '\0';

        if (input.Length == 1)
        {
            char c = input[0];
            if (c == '+' || c == '-' || c == '*' || c == '/')
                return c;
        }

        Console.WriteLine("Neplatná operace. Zkuste to znovu.");
        PrintMenu();
    }
}

double ReadDouble(bool nonZero = false)
{
    while (true)
    {
        string? input = Console.ReadLine();
        if (!double.TryParse(input, out double value))
        {
            Console.WriteLine("Neplatný vstup. Zkuste to znovu.");
            continue;
        }

        if (nonZero && value == 0)
        {
            Console.WriteLine("Neplatný vstup. Číslo nesmí být 0. Zkuste to znovu.");
            continue;
        }

        return value;
    }
}

double Compute(char operation, double operand1, double operand2)
{
    return operation switch
    {
        '+' => operand1 + operand2,
        '-' => operand1 - operand2,
        '*' => operand1 * operand2,
        '/' => operand2 == 0 ? throw new DivideByZeroException() : operand1 / operand2,
        _ => throw new InvalidOperationException("Neznámá operace")
    };
}

void PrintResult(char operation, double operand1, double operand2, double result)
{
    Console.WriteLine($"Výsledek: {result}");
}