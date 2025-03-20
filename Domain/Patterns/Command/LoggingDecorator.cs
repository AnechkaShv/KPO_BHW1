using System.Diagnostics;

namespace Domain.Patterns.Command;

/// <summary>
/// Represents a decorator for logging the execution time of a command.
/// Implements the Decorator pattern to add logging functionality to any <see cref="ICommand"/>.
/// </summary>
public class LoggingDecorator : ICommand
{
    private readonly ICommand _command; // The command to be decorated

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingDecorator"/> class.
    /// </summary>
    /// <param name="command">The command to be decorated with logging functionality.</param>
    public LoggingDecorator(ICommand command)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
    }

    /// <summary>
    /// Executes the decorated command and logs the execution time.
    /// </summary>
    public void Execute()
    {
        // Start a stopwatch to measure execution time
        var stopWatch = Stopwatch.StartNew();

        // Execute the decorated command
        _command.Execute();

        // Stop the stopwatch and log the elapsed time
        stopWatch.Stop();
        Console.WriteLine($"{_command.GetType().Name} executed in {stopWatch.ElapsedMilliseconds}ms");
    }
}