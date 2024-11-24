namespace FileDuplicateDetector.Models.ConsoleStatus;

public class PathProcessorModel
{
    public List<string> Paths { get; set; } = new();
    public long ProcessedFiles { get; set; }
}