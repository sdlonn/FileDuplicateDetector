﻿namespace FileDuplicateDetector.Services;

internal interface ICancellationService
{
    bool IsCancellationRequested();
    void RequestCancellation();
}