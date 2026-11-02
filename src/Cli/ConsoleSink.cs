using Core.Abstractions;

namespace Cli;

public class ConsoleSink : INotificationSink
{
    public void Notify(string message) => Console.WriteLine($"[Sink-Console] {message}");
}