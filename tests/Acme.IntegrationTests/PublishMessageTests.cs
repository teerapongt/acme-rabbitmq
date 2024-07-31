using System.Net.Http.Json;
using Acme.ConsumerApi.Consumers;
using Acme.Contracts;
using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Acme.IntegrationTests;

public class PublishMessageTests
{
    [Test]
    public async Task PublishMessage_ShouldBeConsumedByRecentMessageConsumer()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureServices(services => services.AddMassTransitTestHarness(
                x => { x.AddConsumer<RecentMessageConsumer>(); }
            )));

        var testHarness = application.Services.GetTestHarness();
        using var client = application.CreateClient();
        const string content = "Hello";

        // Act
        await client.PostAsync("/messages", JsonContent.Create(new Message { Content = content }));
        var consumerTestHarness = testHarness.GetConsumerHarness<RecentMessageConsumer>();

        // Assert
        Assert.That(await consumerTestHarness.Consumed.Any<Message>(x => x.Context.Message.Content == content), Is.True);
    }
}
