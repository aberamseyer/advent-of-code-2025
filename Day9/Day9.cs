using System.Diagnostics;
using System.Numerics;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("input.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("input.txt")} in {sw.ElapsedMilliseconds}ms");

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
            var segments = new List<List<Point>>
            {
                new (){ topLeft, coordinates[j] },
                new (){ coordinates[j], bottomRight },
                new (){ bottomRight, coordinates[i] },
                new (){ coordinates[i], topLeft },
            };
            if (segments.All(s =>
                {
                    var prev = 0;
                    for (var k = 1; k < coordinates.Count; k++, prev++)
                    {
                        if (ProperIntersection(s[0], s[1], coordinates[prev], coordinates[k]))
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

bool ProperIntersection(Point p1, Point q1,
    Point p2, Point q2)
{
    // Vector cross product (p,q,r) = (q-p) × (r-p)
    double Cross(Point a, Point b, Point c) =>
        (b.X - a.X) * (c.Y - a.Y) -
        (b.Y - a.Y) * (c.X - a.X);

    // Orientation of triplet (a,b,c):
    //  >0 : counter‑clockwise
    //  <0 : clockwise
    //   0 : collinear
    var o1 = Cross(p1, q1, p2);
    var o2 = Cross(p1, q1, q2);
    var o3 = Cross(p2, q2, p1);
    var o4 = Cross(p2, q2, q1);

    // Proper intersection iff orientations differ
    var proper = (o1 > 0 && o2 < 0 || o1 < 0 && o2 > 0) &&
                 (o3 > 0 && o4 < 0 || o3 < 0 && o4 > 0);

    return proper;
}

record Point(int X, int Y);