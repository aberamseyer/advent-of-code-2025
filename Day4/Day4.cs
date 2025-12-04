using System.Diagnostics;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("input.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("input.txt")} in {sw.ElapsedMilliseconds}ms");

return;

long Part1(string fileName)
{
    long accessible = 0;
    var map = File.ReadAllLines(fileName)
        .Select(mapStr => mapStr.ToCharArray().ToList())
        .ToList();
    for (var i = 0; i < map.Count; i++)
        for (var j = 0; j < map[i].Count; j++)
            if (map[i][j] == '@' && Accessible(map, i, j))
                accessible++;

    return accessible;
}

long Part2(string fileName)
{
    var map = File.ReadAllLines(fileName)
        .Select(mapStr => mapStr.ToCharArray().ToList())
        .ToList();
    var totalRemoved = 0;
    var accessible = new List<Tuple<int, int>>();
    do {
        foreach (var (i, j) in accessible)
            map[i][j] = '.';
        accessible.Clear();

        for (var i = 0; i < map.Count; i++)
            for (var j = 0; j < map[i].Count; j++)
                if (map[i][j] == '@' && Accessible(map, i, j))
                    accessible.Add(Tuple.Create(i, j));

        totalRemoved += accessible.Count;
    } while (accessible.Count > 0) ;

    return totalRemoved;
}

bool Accessible(List<List<char>> map, int x, int y)
{
    var total = 0;
    for (var i = -1; i <= 1; i++)
    {
        for (var j = -1; j <= 1; j++)
        {
            if (i == 0 && j == 0)
                continue;
            var xi = x + i;
            var yj = y + j;
            if (0 <= xi && xi < map.Count && 0 <= yj && yj < map[xi].Count &&
                map[xi][yj] == '@')
                total++;
            if (total >= 4)
                return false;
        }
    }
    return true;
}