using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine($"{Part1("test.txt")} in {sw.ElapsedMilliseconds}ms");
sw.Restart();
Console.WriteLine($"{Part2("test.txt")} in {sw.ElapsedMilliseconds}ms");

return;

long Part1(string fileName)
{
    return File.ReadAllLines(fileName)
        .Select(l => Regex.Match(l, @"\[([\.\#]+)\] (\([\d,]+\) ?)+ \{([\d,]+)\}"))
        .Select(m => new Machine(
            m.Groups[1].Value.Trim().Select(c => c == '#').ToArray(),
            m.Groups[2].Captures.Select(c => c.Value.Trim('(',')',' ').Split(',').Select(int.Parse).ToList()).ToList(),
            m.Groups[3].Value.Split(',').Select(int.Parse).ToList()
        )).Sum(machine =>
        {
            var presses = new int[machine.Buttons.Count];
            while (!CheckMachine(machine, ref presses))
            {
                // this doesn't correctly do permutations, so it doesn't work
                for (var i = 0; i < presses.Length; i++)
                {
                    presses[i] += 1;
                    if (CheckMachine(machine, ref presses)) break;
                    presses[i] -= 1;
                }

                for (var i = 0; i < presses.Length; i++)
                {
                    presses[i]++;
                }
            }
            return presses.Sum();
        });
}

long Part2(string fileName)
{
    return 0;
}

bool CheckMachine(Machine m, ref int[] presses)
{
    var lightState = new bool[m.Lights.Length];
    for (var pressIndex = 0; pressIndex < presses.Length; pressIndex++)
    {
        foreach (var _ in Enumerable.Range(0, presses[pressIndex]))
        {
            foreach (var b in m.Buttons[pressIndex])
            {
                lightState[b] = !lightState[b];
            }
        }
    }

    for (var i = 0; i < m.Lights.Length; i++)
    {
        if (m.Lights[i] != lightState[i])
            return false;
    }

    return true;
}
record Machine(bool[] Lights, List<List<int>> Buttons, List<int> Joltages);