using FileDuplicateDetector.Models;

namespace FileDuplicateDetector.Printers;

internal interface IConsolePrinter
{
    void RunStatusPrinter(ScanResult scanResults);
}