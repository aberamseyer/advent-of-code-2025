using System.Diagnostics;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("input.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("input.txt")} in {sw.ElapsedMilliseconds}ms");

return;

long Part1(string fileName)
{
    ParseFile(fileName, out var freshRanges, out var ingredientsToCheck);

    return ingredientsToCheck.LongCount(ingredient =>
        freshRanges.Any(freshRange =>
            freshRange.Start <= ingredient && ingredient <= freshRange.End));
}

long Part2(string fileName)
{
    ParseFile(fileName, out var freshRanges, out _);

    MergeRanges(ref freshRanges);

    return freshRanges.Sum(r => r.End - r.Start + 1);
}

void MergeRanges(ref List<(long Start, long End)> ranges)
{
    ranges = ranges.OrderBy(r => r.Start).ToList();

    var merged = new List<(long, long)>();
    var current = ranges[0];
    foreach (var r in ranges.Skip(1))
    {
        if (r.Start <= current.End)
        {
            current = (current.Start, Math.Max(current.End, r.End));
        }
        else
        {
            merged.Add(current);
            current = r;
        }
    }

    merged.Add(current);
    ranges = merged;
}

void ParseFile(string s, out List<(long Start, long End)> freshRanges, out List<long> ingredientsToCheck)
{
    freshRanges = [];
    ingredientsToCheck = [];
    foreach (var line in File.ReadLines(s))
    {
        if (line.Length == 0) continue;
        if (line.Contains('-'))
        {
            var range = line.Split('-');
            freshRanges.Add((long.Parse(range[0]), long.Parse(range[1])));
        }
        else ingredientsToCheck.Add(long.Parse(line));
    }
}