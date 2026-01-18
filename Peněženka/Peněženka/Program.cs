using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

const int MAX_ITEMS = 1000;
string dataFile = Path.Combine(AppContext.BaseDirectory, "penazenka.txt");

var entries = new List<Entry>();

LoadFromFile();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== Peněženka =====");
    Console.WriteLine("1) Načíst ze souboru");
    Console.WriteLine("2) Uložit do souboru");
    Console.WriteLine("3) Vypsat všechny položky");
    Console.WriteLine("4) Vypsat s filtrem");
    Console.WriteLine("5) Přidat položku");
    Console.WriteLine("6) Upravit položku");
    Console.WriteLine("7) Smazat položku");
    Console.WriteLine("8) Statistiky");
    Console.WriteLine("9) Řadit a vypsat (seřazeno bez změny pořadí)");
    Console.WriteLine("0) Konec (uloží a ukončí)");
    Console.Write("Volba: ");
    var opt = Console.ReadLine()?.Trim();

    Console.WriteLine();

    switch (opt)
    {
        case "1":
            LoadFromFile();
            break;
        case "2":
            SaveToFile();
            break;
        case "3":
            PrintEntries(entries, showTotals: true);
            break;
        case "4":
            FilteredList();
            break;
        case "5":
            AddEntry();
            break;
        case "6":
            EditEntry();
            break;
        case "7":
            DeleteEntry();
            break;
        case "8":
            PrintStatistics(entries);
            break;
        case "9":
            SortAndPrint();
            break;
        case "0":
            SaveToFile();
            return;
        default:
            Console.WriteLine("Neplatná volba.");
            break;
    }
}

void LoadFromFile()
{
    try
    {
        if (!File.Exists(dataFile))
        {
            Console.WriteLine("Soubor neexistuje. Začínám s prázdným seznamem.");
            return;
        }

        var lines = File.ReadAllLines(dataFile);
        var loaded = new List<Entry>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var parts = line.Split('\t');
            if (parts.Length < 2) continue;

            if (!int.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var val))
                continue;

            var name = parts[1];
            var category = parts.Length >= 3 ? parts[2] : string.Empty;

            loaded.Add(new Entry { Amount = val, Name = name, Category = category });
            if (loaded.Count >= MAX_ITEMS) break;
        }

        entries = loaded;
        Console.WriteLine($"Načteno {entries.Count} položek ze souboru '{dataFile}'.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Chyba při načítání: {ex.Message}");
    }
}

void SaveToFile()
{
    try
    {
        using var sw = new StreamWriter(dataFile, false);
        foreach (var e in entries)
        {
            sw.WriteLine($"{e.Amount}\t{e.Name}\t{e.Category}");
        }
        Console.WriteLine($"Uloženo {entries.Count} položek do '{dataFile}'.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Chyba při ukládání: {ex.Message}");
    }
}

void PrintEntries(IEnumerable<Entry> listToPrint, bool showTotals)
{
    var list = listToPrint.ToArray();

    Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0,8}  {1,-30}  {2,-15}", "Částka", "Popis", "Kategorie"));
    Console.WriteLine(new string('-', 60));

    for (int i = 0; i < list.Length; i++)
    {
        var e = list[i];
        Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0,8}  {1,-30}  {2,-15}", e.Amount, Truncate(e.Name, 30), Truncate(e.Category, 15)));
    }

    if (showTotals)
    {
        Console.WriteLine(new string('-', 60));
        PrintStatistics(entries);
    }
}

void FilteredList()
{
    Console.Write("Zadejte hledaný řetězec (case-insensitive): ");
    var q = Console.ReadLine() ?? string.Empty;
    var filtered = entries.Where(e => e.Name.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0 || e.Category.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0);
    PrintEntries(filtered, showTotals: true);
}

void AddEntry()
{
    if (entries.Count >= MAX_ITEMS)
    {
        Console.WriteLine($"Nelze přidat položku: dosaženo MAX_ITEMS = {MAX_ITEMS}.");
        return;
    }

    Console.Write("Zadejte hodnotu (int, kladná příjem, záporná výdaj): ");
    var sVal = Console.ReadLine();
    if (!int.TryParse(sVal, NumberStyles.Integer, CultureInfo.InvariantCulture, out var val))
    {
        Console.WriteLine("Neplatné číslo.");
        return;
    }

    Console.Write("Zadejte popis: ");
    var name = (Console.ReadLine() ?? string.Empty).Trim();

    Console.Write("Zadejte kategorii (volitelně): ");
    var category = (Console.ReadLine() ?? string.Empty).Trim();

    entries.Add(new Entry { Amount = val, Name = name, Category = category });
    Console.WriteLine("Položka přidána.");
}

void EditEntry()
{
    if (entries.Count == 0)
    {
        Console.WriteLine("Žádné položky k úpravě.");
        return;
    }

    PrintEntries(entries, showTotals: false);
    Console.Write("Zadejte číslo řádku k úpravě (1..{0}): ", entries.Count);
    var sIdx = Console.ReadLine();
    if (!int.TryParse(sIdx, NumberStyles.Integer, CultureInfo.InvariantCulture, out var idx) || idx < 1 || idx > entries.Count)
    {
        Console.WriteLine("Neplatné číslo řádku.");
        return;
    }
    var e = entries[idx - 1];

    Console.Write($"Nová hodnota (aktuálně {e.Amount}): ");
    var sVal = Console.ReadLine();
    if (!int.TryParse(sVal, NumberStyles.Integer, CultureInfo.InvariantCulture, out var newVal))
    {
        Console.WriteLine("Neplatné číslo.");
        return;
    }

    Console.Write($"Nový popis (aktuálně '{e.Name}'): ");
    var newName = (Console.ReadLine() ?? string.Empty).Trim();

    Console.Write($"Nová kategorie (aktuálně '{e.Category}'): ");
    var newCat = (Console.ReadLine() ?? string.Empty).Trim();

    e.Amount = newVal;
    e.Name = newName;
    e.Category = newCat;
    Console.WriteLine("Položka upravena.");
}

void DeleteEntry()
{
    if (entries.Count == 0)
    {
        Console.WriteLine("Žádné položky ke smazání.");
        return;
    }

    PrintEntries(entries, showTotals: false);
    Console.Write("Zadejte číslo řádku k smazání (1..{0}): ", entries.Count);
    var sIdx = Console.ReadLine();
    if (!int.TryParse(sIdx, NumberStyles.Integer, CultureInfo.InvariantCulture, out var idx) || idx < 1 || idx > entries.Count)
    {
        Console.WriteLine("Neplatné číslo řádku.");
        return;
    }

    entries.RemoveAt(idx - 1);
    Console.WriteLine("Položka smazána.");
}

void PrintStatistics(IEnumerable<Entry> list)
{
    var arr = list.ToArray();
    var total = arr.Sum(e => e.Amount);
    var incomes = arr.Where(e => e.Amount > 0).Select(e => e.Amount).ToArray();
    var expenses = arr.Where(e => e.Amount < 0).Select(e => e.Amount).ToArray();

    Console.WriteLine($"Celkem položek: {arr.Length}, Součet: {total}");
    Console.WriteLine($"Příjmy: počet {incomes.Length}, součet {incomes.Sum()}, min { (incomes.Length>0 ? incomes.Min().ToString() : "-") }, max { (incomes.Length>0 ? incomes.Max().ToString() : "-") }");
    Console.WriteLine($"Výdaje: počet {expenses.Length}, součet {expenses.Sum()}, min { (expenses.Length>0 ? expenses.Min().ToString() : "-") }, max { (expenses.Length>0 ? expenses.Max().ToString() : "-") }");

    // součty podle kategorií
    var catSums = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
    foreach (var e in arr)
    {
        var cat = string.IsNullOrWhiteSpace(e.Category) ? "(bez kategorie)" : e.Category;
        if (!catSums.ContainsKey(cat)) catSums[cat] = 0;
        catSums[cat] += e.Amount;
    }

    if (catSums.Count > 0)
    {
        Console.WriteLine("Součty podle kategorií:");
        foreach (var kv in catSums.OrderBy(k => k.Key))
        {
            Console.WriteLine($"  {kv.Key}: {kv.Value}");
        }
    }
}

void SortAndPrint()
{
    Console.WriteLine("Řazení: 1) podle hodnoty  2) podle popisku");
    Console.Write("Volba: ");
    var k = Console.ReadLine()?.Trim();
    Console.WriteLine("Směr: 1) vzestupně  2) sestupně");
    Console.Write("Volba směru: ");
    var s = Console.ReadLine()?.Trim();

    bool descending = s == "2";
    Entry[] working = entries.ToArray();

    if (k == "1")
    {
        Array.Sort(working, (a, b) => a.Amount.CompareTo(b.Amount));
    }
    else
    {
        Array.Sort(working, (a, b) => string.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase));
    }

    if (descending) Array.Reverse(working);

    PrintEntries(working, showTotals: false);
}

static string Truncate(string s, int max)
{
    if (s == null) return string.Empty;
    return s.Length <= max ? s : s.Substring(0, max - 3) + "...";
}

class Entry
{
    public int Amount { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
} 
