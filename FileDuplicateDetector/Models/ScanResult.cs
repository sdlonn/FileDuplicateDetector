using FileDuplicateDetector.Models.ConsoleStatus;

namespace FileDuplicateDetector.Models;

public class ScanResult
{
    public Dictionary<string, PathProcessorModel> ProcessorModels { get; internal set; }
    public List<string> ErrorList { get; } = new();

    public ScanResult(Dictionary<string, PathProcessorModel> processorModels)
    {
        ProcessorModels = processorModels;
    }
}