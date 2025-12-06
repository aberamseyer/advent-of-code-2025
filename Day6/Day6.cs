using System.Diagnostics;
using System.Text.RegularExpressions;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("input.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("input.txt")} in {sw.ElapsedMilliseconds}ms");

return;

long Part1(string fileName)
{
    long grandTotal = 0;
    var numbers = new List<List<long>>();
    var operators = new List<char>();
    foreach (var line in File.ReadLines(fileName))
    {
        if (line[0] == '*' || line[0] == '+')
        {
            operators.AddRange(WhiteSpaceRegex().Split(line.Trim()).Select(x => x[0]));
            for (var i = 0; i < operators.Count; i++)
            {
                var op = operators[i];
                long total = op == '*' ? 1 : 0;
                foreach (var numberList in numbers)
                {
                    if (op == '*')
                        total *= numberList[i];
                    else if (op == '+')
                        total += numberList[i];
                }
                grandTotal += total;
            }
        }
        else
            numbers.Add(WhiteSpaceRegex().Split(line.Trim()).Select(long.Parse).ToList());
    }

    return grandTotal;
}

long Part2(string fileName)
{
    var allChars = TransformFileToArray(fileName);

    allChars = RotateLeft90(allChars);

    long grandTotal = 0;
    var numbers = new List<long>();
    var cols = allChars.GetLength(0);
    for (var i = cols-1; i >= 0; i--)
    {
        var line = new string(
            Enumerable.Range(0, allChars.GetLength(1)).Select(r => allChars[i, r]).ToArray()
        );
        if (line.Trim().Length == 0)
        {
            numbers.Clear();
            continue;
        }
        var number = long.Parse(line.Trim('*', '+', ' '));
        if (!line.Contains('+') && !line.Contains('*'))
            numbers.Add(number);
        else
        {
            numbers.Add(number);
            // operator line
            var op = line[^1];
            if (op == '*')
                grandTotal += numbers.Aggregate<long, long>(1, (acc, curr) => curr * acc);
            else if (op == '+')
                grandTotal += numbers.Sum();
        }
    }

    return grandTotal;
}

char[,] RotateLeft90(char[,] src)
{
    var rows = src.GetLength(0);
    var cols = src.GetLength(1);

    var dst = new char[cols, rows]; // swap dimensions

    for (var r = 0; r < rows; ++r)
    {
        for (var c = 0; c < cols; ++c)
        {
            // element at (r,c) moves to (c, rows-1-r)
            dst[c, rows - 1 - r] = src[r, c];
        }
    }

    return dst;
}

char[,] TransformFileToArray(string fileName)
{
    var lines = File.ReadAllLines(fileName);

    var height = lines.Length;
    var width = lines.Max(line => line.Length);

    var result = new char[height, width];

    for (var row = 0; row < height; row++)
    {
        var currentLine = lines[height - 1 - row];
        for (var col = 0; col < currentLine.Length; col++)
            result[row, col] = currentLine[col];
    }

    return result;
}


internal partial class Program
{
    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpaceRegex();
}