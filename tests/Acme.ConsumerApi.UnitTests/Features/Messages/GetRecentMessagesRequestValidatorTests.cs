using Acme.ConsumerApi.Features.Messages.GetRecentMessages;

namespace Acme.ConsumerApi.UnitTests.Features.Messages;

[TestFixture]
public class GetRecentMessagesRequestValidatorTests
{
    private GetRecentMessagesRequestValidator _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new GetRecentMessagesRequestValidator();
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void Validate_InvalidCount_ShouldHaveValidationError(int invalidCount)
    {
        // Arrange
        var model = new GetRecentMessagesRequest { Count = invalidCount };

        // Act
        var result = _sut.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Count);
    }

    [TestCase(5)]
    public void Validate_ValidCount_ShouldNotHaveValidationError(int count)
    {
        // Arrange
        var model = new GetRecentMessagesRequest { Count = count };

        // Act
        var result = _sut.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Count);
    }
}
