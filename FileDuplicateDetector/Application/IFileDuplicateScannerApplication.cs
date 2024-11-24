namespace FileDuplicateDetector.Application;

internal interface IFileDuplicateScannerApplication
{
    Task Run(string[] args);
}