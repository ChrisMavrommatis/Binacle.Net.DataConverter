using Binacle.Net.DataConverter.Commands;
using Cocona;

namespace Binacle.Net.DataConverter;

internal class Program
{
    static void Main(string[] args)
    {
        var app = CoconaLiteApp.Create(); 

        app.AddCommand(ORLibraryThPackCommand.Name, ORLibraryThPackCommand.Command);

        app.Run();
    }
}
