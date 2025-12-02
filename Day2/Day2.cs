using System.Collections.Immutable;

Console.WriteLine(Part1("input.txt"));
Console.WriteLine(Part2("input.txt"));

return;

long Part1(string fileName)
{
    long total = 0;
    foreach(var rangeStr in File.ReadAllText(fileName).Split(','))
    {
        var range = rangeStr.Split('-').Select(long.Parse).ToImmutableArray();
        var begin = range[0];
        var end = range[1];

        for (var i = begin; i <= end; i++)
        {
            var number = i.ToString();
            if (number[..(number.Length / 2)] == number[(number.Length / 2)..])
                total += i;
        }
    }

    return total;
}
long Part2(string fileName)
{
    long total = 0;
    foreach(var rangeStr in File.ReadAllText(fileName).Split(','))
    {
        var range = rangeStr.Split('-').Select(long.Parse).ToImmutableArray();
        var begin = range[0];
        var end = range[1];

        for (var currentNumber = begin; currentNumber <= end; currentNumber++)
        {
            var number = currentNumber.ToString();
            for (var windowSize = number.Length / 2; windowSize >= 1; windowSize--)
            {
                var firstChunk = number[..windowSize];
                if (firstChunk[0] == '0')
                    break;
                if (!number.Chunk(windowSize).All(chunk => chunk.SequenceEqual(firstChunk)))
                    continue;
                total += currentNumber;
                break;
            }
        }
    }

    return total;
}