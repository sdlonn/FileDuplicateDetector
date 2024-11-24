using System.Security.Cryptography;
using System.Text;

namespace FileDuplicateDetector.Services;

public class Md5Service : IMd5Service
{
    public string CalculateMd5Hash(string filePath)
    {
        using var md5 = MD5.Create();
        using var fileStream = File.OpenRead(filePath);
        var hashBytes = md5.ComputeHash(fileStream);
        var sb = new StringBuilder();
        foreach (var byteValue in hashBytes)
        {
            sb.Append(byteValue.ToString("x2"));
        }

        return sb.ToString();
    }
}