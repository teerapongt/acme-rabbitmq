using System.Collections.Concurrent;
using Acme.Contracts;
using MassTransit;

namespace Acme.ConsumerApi.Consumers;

public class RecentMessageConsumer : IConsumer<Message>
{
    private static readonly ConcurrentStack<Message> _messages = new();

    public async Task Consume(ConsumeContext<Message> context)
    {
        _messages.Push(context.Message);
        await Task.CompletedTask;
    }

    public static IEnumerable<Message> GetRecentMessages(int count)
    {
        return _messages.Take(count);
    }
}
