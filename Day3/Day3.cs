Console.WriteLine(Part1("input.txt"));
Console.WriteLine(Part2("input.txt"));

return;

long Part1(string fileName)
{
    long totalJoltage = 0;
    foreach (var batteryBank in File.ReadLines(fileName))
    {
        var batteries = batteryBank.Select(c => c - '0').ToArray();
        var firstBatteryIndex = 0;
        for (var i = 0; i < batteries.Length - 1; i++)
        {
            if (batteries[i] > batteries[firstBatteryIndex])
                firstBatteryIndex = i;
        }

        var secondBatteryIndex = firstBatteryIndex + 1;
        for (var i = firstBatteryIndex + 1; i < batteries.Length; i++)
        {
            if (batteries[i] > batteries[secondBatteryIndex])
                secondBatteryIndex = i;
        }

        totalJoltage += batteries[firstBatteryIndex] * 10 + batteries[secondBatteryIndex];
    }

    return totalJoltage;
}

long Part2(string fileName)
{
    long totalJoltage = 0;
    foreach (var batteryBank in File.ReadLines(fileName))
    {
        var batteries = batteryBank.Select(c => c - '0').ToArray();
        var reverseDigits = new int[12];
        long currentJoltage = 0;
        for (var digit = reverseDigits.Length - 1; digit >= 0; digit--)
        {
            var checkFrom = digit == reverseDigits.Length - 1 ? 0 : reverseDigits[digit + 1] + 1;
            reverseDigits[digit] = checkFrom;
            for (var cellToCheck = checkFrom; cellToCheck < batteries.Length - digit; cellToCheck++)
            {
                if (batteries[cellToCheck] > batteries[reverseDigits[digit]])
                    reverseDigits[digit] = cellToCheck;
            }

            var cellJoltage = batteries[reverseDigits[digit]] *
                              (long)Math.Pow(10, digit);
            currentJoltage += cellJoltage;
        }

        totalJoltage += currentJoltage;
    }

    return totalJoltage;
}