using FileDuplicateDetector.Models;

namespace FileDuplicateDetector.Services;

internal interface IFileWriter
{
    Task WriteResult(ProcessedResult processedResult);
}