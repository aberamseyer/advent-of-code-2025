Console.WriteLine(Part1("input.txt"));
Console.WriteLine(Part2("input.txt"));
return;

long Part2(string fileName)
{
    long dial = 50;
    long password = 0;

    foreach(var line in File.ReadLines(fileName))
    {
        var turn = line.AsSpan()[1..].AsLong();
        password += turn / 100;
        turn %= 100;

        if (line[0] == 'L')
        {
            dial -= turn;
            if (dial < 0)
            {
                if (Math.Abs(dial) != turn)
                    password++;
                dial = 100 + dial;
            }
        }
        else
        {
            dial += turn;
            if (dial > 99 && dial % 100 != 0)
                password++;
            dial %= 100;
        }

        if (dial == 0) password++;
    }

    return password;
}

long Part1(string fileName)
{
    long dial = 50;
    long password = 0;

    foreach(var line in File.ReadLines(fileName))
    {
        var turn = line.AsSpan()[1..].AsLong();
        if (line[0] == 'L')
        {
            dial -= turn;
            if (dial < 0) dial = 100 + dial;
        }
        else dial = (dial + turn) % 100;

        if (dial == 0) password++;
    }

    return password;
}

internal static class Extensions
{
    internal static long AsLong(this ReadOnlySpan<char> span) => long.Parse(span.ToString());
}
