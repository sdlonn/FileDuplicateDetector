using System.Collections.Concurrent;

namespace FileDuplicateDetector.Models;

public class ProcessedResult
{
    public ConcurrentDictionary<string, List<string>> Result { get; } = new();
    public List<string> ErrorList { get; } = new();
}