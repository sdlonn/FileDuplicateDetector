using FileDuplicateDetector.Config;

namespace FileDuplicateDetector.Services;

public class ParameterService : IParameterService
{
    private readonly string[] _args;
    private readonly Dictionary<string, string> _parameters = new();
    private readonly string[] _requiredParams = { Constants.Paths };
    private readonly List<string> _paths;

    public ParameterService(string[] args)
    {
        _args = args;
        Parse();
        ValidateParameters();
        _paths = SetPaths();
    }

    public void Parse()
    {
        for (var i = 0; i < _args.Length; i++)
        {
            var argument = _args[i];
            if (argument.Trim().StartsWith(Constants.ParameterSuffix))
            {
                var p = _args[i].Split(Constants.ParameterKeyValueSeparator);
                if (p.Length == 2)
                {
                    var key = p[0];
                    var value = p[1];

                    if (key.StartsWith(Constants.ParameterSuffix))
                    {
                        var paramName = key[2..];
                        _parameters.Add(paramName, value);
                    }
                }
            }
        }
    }

    public string? GetParameter(string name)
    {
        _parameters.TryGetValue(name, out var value);
        return value;
    }

    public bool HasParameter(string name)
    {
        return _parameters.ContainsKey(name);
    }

    private void ValidateParameters()
    {
        var missingParams = false;
        foreach (var param in _requiredParams)
        {
            if (!_parameters.ContainsKey(param))
            {
                missingParams = true;
                Console.WriteLine($"Missing parameter '{param}'.");
            }
        }

        if (missingParams)
        {
            throw new ArgumentException();
        }
    }

    private List<string> SetPaths()
    {
        return _parameters[Constants.Paths]
            .Split(Constants.PathSeparator)
            .Select(path => path.EndsWith(Constants.WindowsDirectorySeparator)
                ? path
                : path + Constants.WindowsDirectorySeparator)
            .ToList();
    }

    public List<string> GetPaths()
    {
        return _paths;
    }
}