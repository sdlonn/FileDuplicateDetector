namespace FileDuplicateDetector.Services;

internal class CancellationService : ICancellationService
{
    private readonly CancellationToken _cancellationToken;

    public CancellationService(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public bool IsCancellationRequested()
    {
        return _cancellationToken.IsCancellationRequested;
    }
}