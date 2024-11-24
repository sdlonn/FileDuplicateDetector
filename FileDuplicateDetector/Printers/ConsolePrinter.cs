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

    public async Task RunStatusPrinter(ScanResult scanResults)
    {
        List<string> paths = scanResults.ProcessorModels.Keys.ToList();
        var totalPaths = paths.Count;


        // Print initial empty lines for each path
        for (var i = 0; i < totalPaths; i++)
        {
            Console.WriteLine();
        }


        while (!_cancellationService.IsCancellationRequested())
        {
            // Simulate progress for each path-
            for (var i = 0; i < totalPaths; i++)
            {
                var path = paths[i];
                ShowProgress(dict[path].Paths.Count, dict[path].ProcessedFiles, path, i, totalPaths);
            }

            // ShowLoadingBar();
            Thread.Sleep(50); // Simulate work with a delay (this can be replaced with actual work)
        }


        // Move to the next line after the progress bars are done
        Console.WriteLine();
        Console.WriteLine("All tasks complete!");
        //return Task.CompletedTask;
    }

    private void ShowProgress(int currentTotalAmountOfFiles, long filesProcessed, string path, int pathIndex,
        int totalPaths)
    {
        // Prevent zero or negative values for total files
        if (currentTotalAmountOfFiles <= 0)
        {
            currentTotalAmountOfFiles = 1;
        }

        // Ensure filesProcessed doesn't exceed total files
        if (filesProcessed > currentTotalAmountOfFiles)
        {
            filesProcessed = currentTotalAmountOfFiles;
        }

        // Calculate the progress as a ratio
        var progress = (double)filesProcessed / currentTotalAmountOfFiles;

        // Ensure the progress is between 0 and 1 (no overflow)
        progress = Math.Min(1.0, Math.Max(0.0, progress));

        // Number of characters in the progress bar
        var barWidth = 50;

        // Calculate the number of "#" characters to represent progress
        var progressChars = (int)(progress * barWidth); // Number of characters to represent progress

        // Set the cursor position so that each path progress bar appears on a separate line
        Console.SetCursorPosition(0, pathIndex);

        // Print the path name and progress
        Console.Write($"{path.PadRight(20)}"); // Ensure path name is aligned (pad with spaces)
        Console.Write($"({filesProcessed}/{currentTotalAmountOfFiles}) ");

        // Print the progress bar for this specific path
        Console.Write("[");
        Console.Write(new string('#', progressChars)); // Filled part of the bar
        Console.Write(new string(' ', barWidth - progressChars)); // Unfilled part of the bar
        Console.Write("]");

        // Print the percentage of completion for this path
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
        // Move the cursor to a new line after all paths are printed
        Console.Write("[");
        Console.Write(new string(' ', _loadingBarIndex));
        Console.Write(new string('#', 3)); // Filled part of the bar
        Console.Write(new string(' ', loadingBarWidth - _loadingBarIndex + 3)); // Unfilled part of the bar
        Console.Write("]");

        // Update the position: Move right or left depending on the direction
        if (_movingRight)
        {
            _loadingBarIndex++;
            if (_loadingBarIndex + loadingChars.Length > loadingBarWidth) // If the string reaches the end
            {
                _movingRight = false; // Change direction to left
            }
        }
        else
        {
            _loadingBarIndex--;
            if (_loadingBarIndex <= 3) // If the string reaches the start
            {
                _movingRight = true; // Change direction to right
            }
        }
    }
}