using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace Lit_Uno.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new Lit_Uno.App(), args);
            host.Run();
        }
    }
}
