using System.Diagnostics;
using System.Numerics;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("test.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("test.txt")} in {sw.ElapsedMilliseconds}ms");

return;

long Part1(string fileName)
{
    var boxes = File.ReadAllLines(fileName)
        .Select(l => l.Split(','))
        .Select(s => new Vector3(long.Parse(s[0]), long.Parse(s[1]), long.Parse(s[2])))
        .ToList();
    var distances = boxes
        .ToDictionary(v => v, v => (from: v, to: Vector3.Zero, distance: float.MaxValue));
    var circuits = new List<HashSet<Vector3>>();
    foreach (var box in boxes)
    {
        foreach (var otherBox in boxes)
        {
            if (otherBox == box) continue;
            var d = Vector3.Distance(box, otherBox);
            if (d < distances[box].distance)
            {
                distances[box] = (box, otherBox, d);
            }
        }
    }

    var orderedDistances = distances.Values
        .OrderBy(d => d.distance).ToList();
    var connectionsMade = 0;
    foreach (var (from, to, _) in orderedDistances)
    {
        if (!distances.ContainsKey(from) && !distances.ContainsKey(to)) continue;
        var c = circuits.FirstOrDefault(c => c.Contains(from) || c.Contains(to));
        if (c != null)
        {
            c.Add(from);
            c.Add(to);
            connectionsMade++;
        }
        else
        {
            circuits.Add([ from, to ]);
            connectionsMade += 1;
        }

        distances.Remove(from);
        distances.Remove(to);
        if (connectionsMade == 10)
            break;
    }

    return circuits.Select(c => c.Count).OrderDescending()
        .Take(3).Aggregate(1, (a, b) => a * b);
}

long Part2(string fileName)
{
    var boxes = File.ReadAllLines(fileName)
        .Select(l => l.Split(','))
        .Select(s => new Vector3(long.Parse(s[0]), long.Parse(s[1]), long.Parse(s[2])))
        .ToHashSet();

    return 0;
}