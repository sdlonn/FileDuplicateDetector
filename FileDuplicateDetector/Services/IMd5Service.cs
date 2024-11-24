namespace FileDuplicateDetector.Services;

public interface IMd5Service
{
    string CalculateMd5Hash(string filePath);
}