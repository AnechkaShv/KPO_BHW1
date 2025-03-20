using System.Diagnostics;

namespace Domain.Patterns.Command;

public class LoggingDecorator : ICommand
{
    private readonly ICommand _command;

    public LoggingDecorator(ICommand command)
    {
        _command = command;
    }
    public void Execute()
    {
        var stopWatch = Stopwatch.StartNew();
        _command.Execute();
        stopWatch.Stop();
        Console.WriteLine($"{_command.GetType().Name} executed in {stopWatch.ElapsedMilliseconds}ms");
    }
}