using System.Runtime.InteropServices;
using System.Text.Json;

var info = new
{
    Student = "Москаленко Назарій, група ФЕІ-32",
    OSDescription = RuntimeInformation.OSDescription,
    EnvironmentOS = Environment.OSVersion.ToString(),
    Architecture = RuntimeInformation.ProcessArchitecture.ToString(),
    DotNetVersion = Environment.Version.ToString(),
    Runtime = RuntimeInformation.FrameworkDescription,
    BaseDirectory = AppContext.BaseDirectory,
    CurrentDirectory = Environment.CurrentDirectory,
    Domain = "Замовлення (клієнт, товар, замовлення, рядок замовлення)"
};

if (args.Contains("--json"))
{
    var options = new JsonSerializerOptions { 
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
    };
    Console.WriteLine(JsonSerializer.Serialize(info, options));
}
else
{
    Console.WriteLine("CrossApp - практикум з крос-платформного програмування");
    Console.WriteLine($"Студент: {info.Student}");
    
    // Форматування у вигляді таблиці
    Console.WriteLine(new string('-', 95));
    Console.WriteLine($"| {"Характеристика",-25} | {"Значення",-63} |");
    Console.WriteLine(new string('-', 95));
    Console.WriteLine($"| {"OC (OSDescription)",-25} | {info.OSDescription,-63} |");
    Console.WriteLine($"| {"OC (Environment)",-25} | {info.EnvironmentOS,-63} |");
    Console.WriteLine($"| {"Архітектура процесу",-25} | {info.Architecture,-63} |");
    Console.WriteLine($"| {"Версія .NET (CLR)",-25} | {info.DotNetVersion,-63} |");
    Console.WriteLine($"| {"Runtime",-25} | {info.Runtime,-63} |");
    Console.WriteLine($"| {"Каталог застосунку",-25} | {info.BaseDirectory,-63} |");
    Console.WriteLine($"| {"Поточний каталог",-25} | {info.CurrentDirectory,-63} |");
    Console.WriteLine(new string('-', 95));
    
    Console.WriteLine($"Предметна область: {info.Domain}");
}