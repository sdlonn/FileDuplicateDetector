using FileDuplicateDetector.Models;

namespace FileDuplicateDetector.Printers;

internal interface IConsolePrinter
{
    Task RunStatusPrinter(ScanResult scanResults);
}