using System.Threading;

namespace FileDuplicateDetector.Services;

internal class CancellationService : ICancellationService
{
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public CancellationService(CancellationTokenSource cancellationTokenSource)
    {
        _cancellationTokenSource = cancellationTokenSource;
        _cancellationToken = cancellationTokenSource.Token;
    }

    public bool IsCancellationRequested()
    {
        return _cancellationToken.IsCancellationRequested;
    }

    public void RequestCancellation()
    {
        _cancellationTokenSource.Cancel();
    }
}