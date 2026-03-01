namespace IdentityService.UnitTests;

public class NoOpMediator : IMediator
{
    // public async Task<IAsyncEnumerable<TResponse>> CreateStream<TResponse>(IStreamQuery<TResponse> query,
    //     CancellationToken cancellationToken = default)
    // {
    //     await Task.Delay(1);
    //     return AsyncEnumerable.Empty<TResponse>();
    /// <summary>
    /// Creates an empty asynchronous stream for the specified stream request.
    /// </summary>
    /// <returns>An empty IAsyncEnumerable&lt;TResponse&gt;.</returns>

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        return AsyncEnumerable.Empty<TResponse>();
    }

    /// <summary>
    /// Provide an empty asynchronous stream for the specified stream command.
    /// </summary>
    /// <returns>An <see cref="IAsyncEnumerable{TResponse}"/> that yields no elements.</returns>
    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamCommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        return AsyncEnumerable.Empty<TResponse>();
    }

    /// <summary>
    /// Produce an empty asynchronous stream for the given message.
    /// </summary>
    /// <param name="message">The message for which an empty stream is returned.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the stream enumeration to complete.</param>
    /// <returns>An empty <see cref="IAsyncEnumerable{T}"/> of <see cref="object"/> (yields no elements).</returns>
    public IAsyncEnumerable<object?> CreateStream(object message, CancellationToken cancellationToken = default)
    {
        return AsyncEnumerable.Empty<object?>();
    }

    /// <summary>
    /// Publishes the provided notification but performs no operation (no-op).
    /// </summary>
    /// <typeparam name="TNotification">The type of notification being published.</typeparam>
    /// <param name="notification">The notification to publish; this implementation ignores it.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete; this implementation does not observe it.</param>
    /// <returns>A completed <see cref="ValueTask"/> representing immediate completion.</returns>
    public ValueTask Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Publishes the given notification using a no-op implementation.
    /// </summary>
    /// <param name="notification">The notification message to publish.</param>
    /// <param name="cancellationToken">Token to cancel the publish operation.</param>
    /// <returns>A completed <see cref="ValueTask"/>.</returns>
    public ValueTask Publish(object notification, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Returns the default value for the specified request type without executing any handler logic.
    /// </summary>
    /// <param name="request">The request to send; ignored by this no-op implementation.</param>
    /// <param name="cancellationToken">Cancellation token; ignored by this implementation.</param>
    /// <returns>The default value of <typeparamref name="TResponse"/>.</returns>
    public ValueTask<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(default(TResponse)!);
    }

    /// <summary>
    /// Processes the provided command and returns the default value for the response type.
    /// </summary>
    /// <param name="command">The command to process; its contents are ignored by this no-op implementation.</param>
    /// <returns>`default(TResponse)` — the default value for the response type.</returns>
    public ValueTask<TResponse> Send<TResponse>(ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(default(TResponse)!);
    }

    /// <summary>
    /// Provide a no-op response for the given query.
    /// </summary>
    /// <returns>`default(TResponse)`.</returns>
    public ValueTask<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult(default(TResponse)!);
    }

    /// <summary>
    /// Sends a message and yields a null result in this no-op mediator.
    /// </summary>
    /// <param name="message">The message to send; ignored by this no-op implementation.</param>
    /// <returns>Always null.</returns>
    public ValueTask<object?> Send(object message, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult<object?>(null);
    }

    /// <summary>
    /// Produces an empty asynchronous sequence for the provided stream query.
    /// </summary>
    /// <param name="query">The stream query (not used by this implementation).</param>
    /// <param name="cancellationToken">A cancellation token (not observed by this implementation).</param>
    /// <returns>An empty IAsyncEnumerable&lt;TResponse&gt; that yields no items.</returns>
    IAsyncEnumerable<TResponse> ISender.CreateStream<TResponse>(IStreamQuery<TResponse> query,
        CancellationToken cancellationToken)
    {
        return AsyncEnumerable.Empty<TResponse>();
    }
}
