using Domain.Patterns.DataExporter;
using Domain.Patterns.DataImporter;
using Domain.Patterns.Facades;
using Domain.Patterns.Observer;
using Domain.Repository;
using Interface;
using Microsoft.Extensions.DependencyInjection;

namespace BHW1;

class Program
{
    private static readonly ServiceCollection services = new ServiceCollection();
    private static ServiceProvider serviceProvider;
    private static FinanceFacade financeFacade;
        
    private static JsonExporter jsonExporter;
    private static CsvExporter csvExporter;
        
    private static JsonImporter jsonImporter;
    private static CsvImporter csvImporter;
    public static void Configure()
    {
        services.AddSingleton<IAccountObserver, Balance>();
        services.AddSingleton<IFinanceManager, FinanceManager>();
        services.AddSingleton<FinanceFacade>();
        services.AddTransient<JsonExporter>();
        services.AddTransient<CsvExporter>();
        services.AddTransient<JsonImporter>();
        services.AddTransient<CsvImporter>();
        serviceProvider = services.BuildServiceProvider();
        financeFacade = serviceProvider.GetService<FinanceFacade>();
        jsonExporter = serviceProvider.GetService<JsonExporter>();
        csvExporter = serviceProvider.GetService<CsvExporter>();
        jsonImporter = serviceProvider.GetService<JsonImporter>();
        csvImporter = serviceProvider.GetService<CsvImporter>();
    }
    public static void Main(string[] args)
    {
        Configure();
        MainInterface menu = new MainInterface();
        menu.ShowMenu(financeFacade, jsonExporter, csvExporter, csvImporter, jsonImporter);
    }
}

