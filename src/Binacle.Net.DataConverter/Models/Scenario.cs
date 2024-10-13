namespace Binacle.Net.DataConverter.Models;

internal sealed class Scenario
{
    public required string Name { get; set; }
    public required string Bin { get; set; }
    public required string Result { get; set; }
    public required string[] Items { get; set; }
}
