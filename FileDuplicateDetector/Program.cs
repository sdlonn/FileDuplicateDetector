using FileDuplicateDetector.Application;
using Microsoft.Extensions.DependencyInjection;
using FileDuplicateDetector.Printers;
using FileDuplicateDetector.Services;

namespace FileDuplicateDetector;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var appService = CreateServices(args).GetRequiredService<IFileDuplicateScannerApplication>();
        await appService.Run(args);
    }

    private static ServiceProvider CreateServices(string[] args)
    {
        var cancellationTokenSource = new CancellationTokenSource();

        return new ServiceCollection()
            .AddSingleton<ICancellationService>(new CancellationService(cancellationTokenSource))
            .AddSingleton<IParameterService>(new ParameterService(args))
            .AddTransient<IMd5Service, Md5Service>()
            .AddSingleton<IFileScanner, FileScanner>()
            .AddSingleton<IConsolePrinter, ConsolePrinter>()
            .AddSingleton<IFileWriter, FileWriter>()
            .AddSingleton<IFileDuplicateScannerApplication, FileDuplicateScannerApplication>()
            .BuildServiceProvider();
    }
}