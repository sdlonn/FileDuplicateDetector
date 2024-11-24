using System.Diagnostics;
using FileDuplicateDetector.Models;
using FileDuplicateDetector.Services;

namespace FileDuplicateDetector.Printers;

internal class ConsolePrinter : IConsolePrinter
{
    private int _loadingBarIndex = 0;
    private bool _movingRight = true;
    private readonly ICancellationService _cancellationService;

    public ConsolePrinter(ICancellationService cancellationService)
    {
        _cancellationService = cancellationService;
    }

    // TODO Remake all
    public void RunStatusPrinter(ScanResult scanResults)
    {
        List<string> paths = scanResults.ProcessorModels.Keys.ToList();
        var totalPaths = paths.Count;

        for (var i = 0; i < totalPaths; i++)
        {
            Console.WriteLine();
        }

        var stopPrinting = false;
        var cancellationRequested = false;
        var stopwatch = new Stopwatch();

        while (!stopPrinting)
        {
            for (var i = 0; i < totalPaths; i++)
            {
                var path = paths[i];
                ShowProgress(scanResults.ProcessorModels[path].Paths.Count,
                    scanResults.ProcessorModels[path].ProcessedFiles, path, i, totalPaths);
            }

            if (!cancellationRequested && _cancellationService.IsCancellationRequested())
            {
                cancellationRequested = true;
                stopwatch.Start();
            }

            if (cancellationRequested && stopwatch.ElapsedMilliseconds > 3000)
            {
                stopPrinting = true;
                // TODO Add some more to console
            }

            Thread.Sleep(50);
        }

        Console.WriteLine();
        Console.WriteLine("All tasks complete!");
    }

    private void ShowProgress(int currentTotalAmountOfFiles, long filesProcessed, string path, int pathIndex,
        int totalPaths)
    {
        if (currentTotalAmountOfFiles <= 0)
        {
            currentTotalAmountOfFiles = 1;
        }


        if (filesProcessed > currentTotalAmountOfFiles)
        {
            filesProcessed = currentTotalAmountOfFiles;
        }


        var progress = (double)filesProcessed / currentTotalAmountOfFiles;


        progress = Math.Min(1.0, Math.Max(0.0, progress));


        var barWidth = 50;


        var progressChars = (int)(progress * barWidth);


        Console.SetCursorPosition(0, pathIndex);


        Console.Write($"{path.PadRight(20)}");
        Console.Write($"({filesProcessed}/{currentTotalAmountOfFiles}) ");


        Console.Write("[");
        Console.Write(new string('#', progressChars));
        Console.Write(new string(' ', barWidth - progressChars));
        Console.Write("]");

        Console.Write($" {progress * 100:0.0}%");

        if (pathIndex == totalPaths - 1)
        {
            ShowLoadingBar(totalPaths);
        }
    }

    private void ShowLoadingBar(int pathIndex)
    {
        const int loadingBarWidth = 50;
        var loadingChars = "###";

        Console.SetCursorPosition(0, pathIndex);

        Console.Write("[");
        Console.Write(new string(' ', _loadingBarIndex));
        Console.Write(new string('#', 3));
        Console.Write(new string(' ', loadingBarWidth - _loadingBarIndex + 3));
        Console.Write("]");


        if (_movingRight)
        {
            _loadingBarIndex++;
            if (_loadingBarIndex + loadingChars.Length > loadingBarWidth)
            {
                _movingRight = false;
            }
        }
        else
        {
            _loadingBarIndex--;
            if (_loadingBarIndex <= 3)
            {
                _movingRight = true;
            }
        }
    }
}