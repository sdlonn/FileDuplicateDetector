using FileDuplicateDetector.Models;
using FileDuplicateDetector.Models.ConsoleStatus;

namespace FileDuplicateDetector.Services;

internal interface IFileScanner
{
    ScanResult ScanPaths();
    ProcessedResult ProcessPaths(Dictionary<string, PathProcessorModel> pathDict);
}