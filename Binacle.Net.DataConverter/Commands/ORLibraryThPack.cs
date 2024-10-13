using Cocona;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        var counter = 1;
        var hasNextLine = true;

        // Get the filename without extension as the name prefix for the test cases
        var name = input.Split('.', StringSplitOptions.RemoveEmptyEntries)[0].TrimStart('/');
        var scenarios = new List<Scenario>();
        while (hasNextLine)
        {
            var testCaseLine = reader.ReadLine();
            if (testCaseLine is null)
            {
                hasNextLine = false;
                break;
            }

            // Test Case
            // 1 2502505
            // 1 112 104 88.47 89.52 1
            // 587 233 220
            // 3
            // 1 108 0 76 0 30 1 40
            // 2 110 0 43 1 25 1 33
            // 3 92 1 81 1 55 1 39

            // First line in each test case is an ID.
            var testCaseIds = testCaseLine.Split(' ');

            // Second line states the results of the test, as reported in the EB-AFIT master's thesis, appendix E.
            string[] testResults = reader.ReadLine().Split(' ');
            var totalItems = Convert.ToDecimal(testResults[1]);
            var totalPackedItems = Convert.ToDecimal(testResults[2]);
            var containerVolumePacked = Convert.ToDecimal(testResults[3]);
            var itemsVolumePacked = Convert.ToDecimal(testResults[4]);

            // Third line defines the container dimensions.
            string[] containerDims = reader.ReadLine().Split(' ');

            // Fourth line states how many distinct item types we are packing.
            int itemTypeCount = Convert.ToInt32(reader.ReadLine());

            // compactFormat   
            // q-LxWxH
            var items = new List<string>();

            for (int i = 0; i < itemTypeCount; i++)
            {
                string[] itemArray = reader.ReadLine().Split(' ');
                var length = itemArray[1];
                var width = itemArray[3];
                var height = itemArray[5];
                var quantity = itemArray[7];
                items.Add($"{length}x{width}x{height}-{quantity}");
            }


            var expectedResult = totalItems == totalPackedItems ? "Fit" : "None";
            var scenario = new Scenario
            {
                Name = $"{name}_{testCaseIds[1]}",
                Bin = $"Raw::{containerDims[0]}x{containerDims[1]}x{containerDims[2]}",
                Result = $"PackingEfficiency::{containerVolumePacked},{itemsVolumePacked}_{totalPackedItems}/{totalItems}",
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
