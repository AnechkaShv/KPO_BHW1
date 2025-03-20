using Interface;
using Microsoft.Extensions.DependencyInjection;

namespace BHW1;

class Program
{
    public static void Main(string[] args)
    {
        var financeFacade = Configuration.Configure();
        MainInterface menu = new MainInterface();
        menu.ShowMenu(financeFacade);
    }
}

