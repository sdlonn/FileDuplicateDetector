namespace FileDuplicateDetector.Config;

internal class Constants
{
    public const string Pattern = "Pattern";
    public const string Paths = "Paths";
    public const char PathSeparator = ';';
    public const string Parallel = "Parallel";
    public const string ReportFile = "ReportFile";
    public const string DefaultReportFileName = "ReportFile.txt";
    public const string ErrorSuffix = ".errors";
    public const string ParameterSuffix = "--";
    public const string ParameterKeyValueSeparator = "=";
    public const string WindowsDirectorySeparator = @"\";

    public const string AllFiles = ".*";
    public const int SingleParallelExecution = 1;
}