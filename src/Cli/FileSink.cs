using Core.Abstractions;

namespace Cli;

public class FileSink(string filePath) : INotificationSink
{
    public void Notify(string message) => File.AppendAllText(filePath, $"[Sink-File] {message}{Environment.NewLine}");
}