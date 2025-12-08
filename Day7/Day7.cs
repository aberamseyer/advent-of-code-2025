using System.Diagnostics;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("input.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("input.txt")} in {sw.ElapsedMilliseconds}ms");

return;

long Part1(string fileName)
{
    long totalSplits = 0;
    var grid = File.ReadAllLines(fileName)
        .Select(l => l.ToCharArray()).ToList();
    var beams = new HashSet<int> { grid[0].IndexOf('S') };
    grid.RemoveAt(0);

    foreach (var line in grid)
    {
        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];
            if (ch != '^') continue;
            if (!beams.Contains(i)) continue;

            beams.Remove(i);
            if (i - 1 >= 0) beams.Add(i - 1);
            if (i + 1 < line.Length) beams.Add(i + 1);
            totalSplits++;
        }
    }

    return totalSplits;
}

long Part2(string fileName)
{
    var grid = File.ReadAllLines(fileName)
        .Select(l => l.ToCharArray()).ToList();
    var beams = new Dictionary<int, long>();
    foreach (var i in Enumerable.Range(0, grid[0].Length))
        beams[i] = 0;
    beams[grid[0].IndexOf('S')] = 1;
    grid.RemoveAt(0);

    foreach (var line in grid)
    {
        for (var i = 0; i < line.Length; i++)
        {
            var ch = line[i];
            if (ch != '^') continue;
            var timelines = beams[i];
            beams[i] = 0;

            if (i - 1 >= 0)
                beams[i - 1] += timelines;

            if (i + 1 < line.Length)
                beams[i + 1] += timelines;
        }
    }

    return beams.Values.Sum();
}
