using Binacle.Net.DataConverter.Commands;
using Cocona;

namespace Binacle.Net.DataConverter;

internal class Program
{
    static void Main(string[] args)
    {
        var app = CoconaLiteApp.Create(); // is a shorthand for `CoconaApp.CreateBuilder().Build()`

        app.AddCommand(ORLibraryThPackCommand.Name, ORLibraryThPackCommand.Command);

        app.Run();
    }
}
