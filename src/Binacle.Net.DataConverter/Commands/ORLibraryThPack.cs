using Cocona;
using Newtonsoft.Json;

namespace Binacle.Net.DataConverter.Commands;

internal class ORLibraryThPackCommand
{
    internal const string Name = "orlib-thpack";

    public static void Command(
        [Option('i')] string input,
        [Option('o')] string output
    )
    {
        using var reader = new System.IO.StreamReader(input);

        var hasNextLine = true;

        // Get the filename without extension as the name prefix for the test cases
        var name = input.Split('.', StringSplitOptions.RemoveEmptyEntries)[0].TrimStart('/');
        var scenarios = new List<Models.Scenario>();
        var linesRead = 0;

        var firstLine = reader.ReadLine();
        linesRead++;

        var noOfProblemsInFile = Convert.ToInt32(firstLine);

        while (hasNextLine)
        {
            var problemFirstLine = reader.ReadLine();
            if (problemFirstLine is null)
            {
                hasNextLine = false;
                break;
            }

            linesRead++;

            // Problem format is as follows
            // 1 2502505
            // 587 233 220
            // 3
            // 1 108 0 76 0 30 1 40
            // 2 110 0 43 1 25 1 33
            // 3 92 1 81 1 55 1 39

            // First line in problem is
            // The problem number and a seed number used in a paper, see documentation.
            var firstLineParts = problemFirstLine.Split(' ');

            // Second line is
            // Container length, width and height
            string[] containerParts = reader.ReadLine().Split(' ');

            // Third line indicates the number of box types
            int boxTypeCount = Convert.ToInt32(reader.ReadLine());

            var items = new List<string>();

            for (int i = 0; i < boxTypeCount; i++)
            {
                // Each line for a box type is
                // [0] Box type number,
                // [1] length, [2] 0/1 indicator
                // [3] width, [4] 0/1 indicator
                // [5] height, [6] 0/1 indicator
                // [7] quantity

                // 0/1 indicator is used to indicate wether placement in the vertical orientation is permissible (=1) or not (=0)
                // Binacle.Net does not use this information

                string[] itemArray = reader.ReadLine().Split(' ');
                var length = itemArray[1];
                var width = itemArray[3];
                var height = itemArray[5];
                var quantity = itemArray[7];
                items.Add($"{length}x{width}x{height}-{quantity}");
            }

            var scenario = new Models.Scenario
            {
                Name = $"OrLibrary_{name}_{firstLineParts[0]}",
                Bin = $"Raw::{containerParts[0]}x{containerParts[1]}x{containerParts[2]}",
                Result = $"PackingEfficiency",
                Items = items.ToArray()
            };

            scenarios.Add(scenario);
        }
        var json = JsonConvert.SerializeObject(scenarios, Formatting.Indented);

        // write output file
        using var writer = new System.IO.StreamWriter(output);

        writer.Write(json);
    }
}
