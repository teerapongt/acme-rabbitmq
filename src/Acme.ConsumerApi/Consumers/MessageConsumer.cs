using System.Collections.Concurrent;
using Acme.Contracts;
using MassTransit;

namespace Acme.ConsumerApi.Consumers;

public class MessageConsumer : IConsumer<Message>
{
    private static readonly ConcurrentQueue<Message> _messages = new();

    public async Task Consume(ConsumeContext<Message> context)
    {
        _messages.Enqueue(context.Message);
        await Task.CompletedTask;
    }

    public static IEnumerable<Message> GetRecentMessages(int count)
    {
        return _messages.Reverse().Take(count);
    }
}
