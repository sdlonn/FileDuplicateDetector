namespace FileDuplicateDetector.Services;

public interface IParameterService
{
    string? GetParameter(string name);
    bool HasParameter(string name);
    void Parse();
    List<string> GetPaths();
}