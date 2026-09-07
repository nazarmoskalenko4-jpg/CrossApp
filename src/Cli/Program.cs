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
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"OC (OSDescription) : {info.OSDescription}");
    Console.WriteLine($"OC (Environment)   : {info.EnvironmentOS}");
    Console.WriteLine($"Архітектура процесу: {info.Architecture}");
    Console.WriteLine($"Версія .NET (CLR)  : {info.DotNetVersion}");
    Console.WriteLine($"Runtime            : {info.Runtime}");
    Console.WriteLine($"Каталог застосунку : {info.BaseDirectory}");
    Console.WriteLine($"Поточний каталог   : {info.CurrentDirectory}");
    Console.WriteLine(new string('-', 52));
    Console.WriteLine($"Предметна область: {info.Domain}");
}