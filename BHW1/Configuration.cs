using Domain.Abstractions;
using Domain.Patterns.Facades;
using Domain.Patterns.Observer;
using Domain.Repository;
using Microsoft.Extensions.DependencyInjection;


namespace BHW1;

public class Configuration
{
    public static FinanceFacade Configure()
    {
        ServiceCollection services = new ServiceCollection();
        ServiceProvider _serviceProvider;
        FinanceFacade _financeFacade;
        services.AddSingleton<IAccountObserver, Balance>();
        services.AddSingleton<IFinanceManager, FinanceManager>();
        services.AddSingleton<FinanceFacade>();
        _serviceProvider = services.BuildServiceProvider();
        _financeFacade = _serviceProvider.GetService<FinanceFacade>();

        return _financeFacade;
    }
}