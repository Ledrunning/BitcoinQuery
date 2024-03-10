using BitcoinQuery.DesktopClient.Extensions;

namespace BitcoinQuery.UnitTests;

[TestFixture]
public class TimeStampParserTests
{
    [Test]
    public void UnixTimeStampToDateTime_ValidTimeStamp_ReturnsCorrectDateTime()
    {
        // Arrange
        long timeStamp = 1645699200; 
        var expectedDateTime = new DateTime(2022, 2, 25, 0, 0, 0, DateTimeKind.Local);

        // Act
        var actualDateTime = timeStamp.UnixTimeStampToDateTime();

        // Assert
        Assert.That(actualDateTime, Is.EqualTo(expectedDateTime));
    }
}