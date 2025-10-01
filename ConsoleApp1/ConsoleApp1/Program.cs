while (true)
{
    Console.WriteLine("Vyber operaci jednu z operací: +, -, *, /, konec");
    string operace = Console.ReadLine();
    if (operace == "konec")
    {
        Console.WriteLine("Konec programu");
        break;
    }
    if (operace != "+" && operace != "-" && operace != "*" && operace != "/")
    {
        Console.WriteLine("Neplatná operace. Zkuste to znovu.");
        continue;
    }
    Console.WriteLine("Zadej první celé číslo:");
    string? input1 = Console.ReadLine();
    int čislo1;
    bool success1 = int.TryParse(input1, out čislo1);
    if (!success1)
    {
        Console.WriteLine("Neplatný vstup. Zkuste to znovu.");
        continue;
    }

    Console.WriteLine("Zadej druhé celé číslo:");
    string? input2 = Console.ReadLine();
    int čislo2;
    bool success2 = int.TryParse(input2, out čislo2);
    if (!success2)
    {
        Console.WriteLine("Neplatný vstup. Zkuste to znovu.");
        continue;
    }
    double výsledek = 0;
    switch (operace)
    {
        case "+":
            výsledek = čislo1 + čislo2;
            Console.WriteLine($"Výsledek: {výsledek}");
            break;
        case "-":
            výsledek = čislo1 - čislo2;
            Console.WriteLine($"Výsledek: {výsledek}");
            break;
        case "*":
            výsledek = čislo1 * čislo2;
            Console.WriteLine($"Výsledek: {výsledek}");
            break;
        case "/":
            if (čislo2 == 0)
            {
                Console.WriteLine("Chyba: Dělení nulou není povoleno.");
            }
            else
            {
                výsledek = (double)čislo1 / čislo2;
                Console.WriteLine($"Výsledek: {výsledek}");
            }
            break;
    }

}