using System.Diagnostics;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("input.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("test.txt")} in {sw.ElapsedMilliseconds}ms");

return;

long Part1(string fileName)
{
    var coordinates = File.ReadAllLines(fileName)
        .Select(l => l.Split(','))
        .Select(s => new Point(int.Parse(s[0]), int.Parse(s[1])))
        .ToList();
    var maxArea = long.MinValue;
    for (var i = 0; i < coordinates.Count; i++)
    {
        for (var j = 0; j < coordinates.Count; j++)
        {
            if (i == j) continue;
            var area = Area(coordinates[i], coordinates[j]);
            if (area < maxArea) continue;
            maxArea = area;
        }
    }
    return maxArea;
}

float Part2(string fileName)
{
    var coordinates = File.ReadAllLines(fileName)
        .Select(l => l.Split(','))
        .Select(s => new Point(int.Parse(s[0]), int.Parse(s[1])))
        .ToList();
    var maxArea = long.MinValue;
    for (var i = 0; i < coordinates.Count; i++)
    {
        for (var j = 0; j < coordinates.Count; j++)
        {
            if (i == j) continue;
            var area = Area(coordinates[i], coordinates[j]);
            if (area < maxArea) continue;

            var topLeft = new Point(coordinates[i].X, coordinates[j].Y);
            var bottomRight = new Point(coordinates[j].X, coordinates[i].Y);
            var vertices = new List<Point>
                { topLeft, coordinates[j], bottomRight, coordinates[i] };
            if (vertices.All(v =>
                {
                    for (int prev = 0, k = 1; k < coordinates.Count; prev=k++)
                    {
                        if (Left(coordinates[prev], coordinates[k], v))
                             return false;
                    }

                    return true;
                }))
            {
                maxArea = area;
            }
        }
    }
    return maxArea;
}

long Area(Point a, Point b) => Math.BigMul(
    Math.Abs(a.X - b.X)+1, Math.Abs(a.Y - b.Y)+1
);

int AreaSign(Point a, Point b, Point c)
{
    var area2 = (b.X - a.X) * (double)(c.Y - a.Y) -
                (c.X - a.X) * (double)(b.Y - a.Y);

    if (area2 > 0.5) return 1;
    if (area2 < -0.5) return -1;
    return 0;
}

bool Left(Point a, Point b, Point c)
{
    return AreaSign(a, b, c) < 0;
}

record Point(int X, int Y);