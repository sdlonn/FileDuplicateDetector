using FileDuplicateDetector.Models.ConsoleStatus;
using System.Text.RegularExpressions;
using FileDuplicateDetector.Config;
using FileDuplicateDetector.Models;

namespace FileDuplicateDetector.Services;

internal class FileScanner : IFileScanner
{
    private readonly IParameterService _parameterService;
    private readonly IMd5Service _md5Service;
    private readonly int _maxDegreeOfParallelism;

    public FileScanner(IParameterService parameterService, IMd5Service md5Service)
    {
        _parameterService = parameterService;
        _md5Service = md5Service;
        _maxDegreeOfParallelism = _parameterService.HasParameter(Constants.Parallel)
            ? int.TryParse(_parameterService.GetParameter(Constants.Parallel), out var x)
                ? x
                : Constants.SingleParallelExecution
            : Constants.SingleParallelExecution;
    }

    public ScanResult ScanPaths()
    {
        var result = new ScanResult(_parameterService.GetPaths().ToDictionary(x => x, x => new PathProcessorModel()));
        var pattern = _parameterService.HasParameter(Constants.Pattern)
            ? _parameterService.GetParameter(Constants.Pattern)!
            : Constants.AllFiles;

        var regex = new Regex(pattern, RegexOptions.Compiled);


        Parallel.ForEach(result.ProcessorModels, new ParallelOptions()
        {
            MaxDegreeOfParallelism = _maxDegreeOfParallelism
        }, (path) =>
        {
            try
            {
                // TODO Modify later to do this in parallel with the MD5 Calculation.
                path.Value.Paths.AddRange(SafeRecursiveDirectoryIterator(path.Key, result.ErrorList, regex));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during traversal: {ex.Message}");
            }
        });

        return result;
    }

    public ProcessedResult ProcessPaths(Dictionary<string, PathProcessorModel> pathDict)
    {
        var processedResult = new ProcessedResult();

        Parallel.ForEach(pathDict, new ParallelOptions()
        {
            MaxDegreeOfParallelism = _maxDegreeOfParallelism
        }, (path) =>
        {
            try
            {
                foreach (var p in path.Value.Paths)
                {
                    var md5 = _md5Service.CalculateMd5Hash(p);

                    if (processedResult.Result.ContainsKey(md5))
                    {
                        processedResult.Result[md5].Add(p);
                    }
                    else
                    {
                        processedResult.Result.TryAdd(md5, new List<string>()
                        {
                            p
                        });
                    }

                    path.Value.ProcessedFiles++;
                }
            }
            catch (Exception ex)
            {
                processedResult.ErrorList.Add(ex.Message);
            }
        });
        return processedResult;
    }

    private IEnumerable<string> SafeRecursiveDirectoryIterator(string path, List<string> errorList, Regex regex)
    {
        var directories = new Stack<string>();
        directories.Push(path);

        while (directories.Count > 0)
        {
            var currentDirectory = directories.Pop();
            IEnumerable<string> files = null;

            try
            {
                files = Directory.EnumerateFiles(currentDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                errorList.Add(currentDirectory);
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (regex.IsMatch(Path.GetFileName(file)))
                    {
                        yield return file;
                    }
                }
            }

            IEnumerable<string> subDirectories = null;

            try
            {
                subDirectories = Directory.EnumerateDirectories(currentDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                errorList.Add(currentDirectory);
            }

            if (subDirectories != null)
            {
                foreach (var subDir in subDirectories)
                {
                    directories.Push(subDir);
                }
            }
        }
    }
}