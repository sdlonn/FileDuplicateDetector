using FileDuplicateDetector.Printers;
using FileDuplicateDetector.Services;

namespace FileDuplicateDetector.Application;

internal class FileDuplicateScannerApplication : IFileDuplicateScannerApplication
{
    private readonly IFileScanner _fileScanner;
    private readonly IConsolePrinter _consolePrinter;
    private readonly IFileWriter _fileWriter;

    public FileDuplicateScannerApplication(IParameterService parameterService, IFileScanner fileScanner,
        IConsolePrinter consolePrinter, IFileWriter fileWriter)
    {
        _fileScanner = fileScanner;
        _consolePrinter = consolePrinter;
        _fileWriter = fileWriter;
    }

    public async Task Run(string[] args)
    {
        var pathDict = _fileScanner.ScanPaths();

        var statusPrinter = Task.Run(() => _consolePrinter.RunStatusPrinter(pathDict));

        var processedFilesResult = _fileScanner.ProcessPaths(pathDict.ProcessorModels);

        await _fileWriter.WriteResult(processedFilesResult);

        await statusPrinter;
    }
}