using FileDuplicateDetector.Config;
using FileDuplicateDetector.Models;

namespace FileDuplicateDetector.Services;

public class FileWriter : IFileWriter
{
    private readonly string _resultFileName;

    public FileWriter(IParameterService parameterService)
    {
        _resultFileName = parameterService.HasParameter(Constants.ReportFile)
            ? parameterService.GetParameter(Constants.ReportFile)!
            : Constants.DefaultReportFileName;
    }

    public async Task WriteResult(ProcessedResult processedResult)
    {
        await WriteDuplicateResult(processedResult);
        await WriteErrors(processedResult);
    }

    private async Task WriteDuplicateResult(ProcessedResult processedResult)
    {
        await using var writer = new StreamWriter(_resultFileName);
        foreach (KeyValuePair<string, List<string>> keyValuePair in processedResult.Result)
        {
            if (keyValuePair.Value.Count <= 1)
            {
                continue;
            }

            await writer.WriteLineAsync($"{keyValuePair.Key} | {keyValuePair.Value.Count}");
            foreach (var pa in keyValuePair.Value)
            {
                await writer.WriteLineAsync(pa);
            }

            await writer.WriteLineAsync("");
        }
    }

    private async Task WriteErrors(ProcessedResult processedResult)
    {
        if (processedResult.ErrorList.Count > 0)
        {
            await using var writer = new StreamWriter(_resultFileName + Constants.ErrorSuffix);
            foreach (var errorMessage in processedResult.ErrorList)
            {
                await writer.WriteLineAsync(errorMessage);
            }
        }
    }
}