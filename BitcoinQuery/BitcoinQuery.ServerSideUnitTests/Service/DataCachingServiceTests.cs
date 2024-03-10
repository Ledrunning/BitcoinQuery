using BitcoinQuery.Service.Models;
using BitcoinQuery.Service.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace BitcoinQuery.ServerSideUnitTests.Service;

[TestFixture]
public class DataCachingServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _mockMemoryCache = new Mock<IMemoryCache>();
        _mockLogger = new Mock<ILogger<DataCachingService>>();
    }

    private Mock<IMemoryCache> _mockMemoryCache;
    private Mock<ILogger<DataCachingService>> _mockLogger;

    [Test]
    public void SaveDataToCache_NullData_ThrowsArgumentNullException()
    {
        // Arrange
        var service = new DataCachingService(_mockMemoryCache.Object, _mockLogger.Object);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => service.SaveDataToCache(null));
    }

    [Test]
    public void GetLatestDataFromCache_CacheContainsData_ReturnsData()
    {
        // Arrange
        var expectedData = new List<DataPoint>(); // Create some sample data
        _mockMemoryCache.Setup(mc => mc.TryGetValue("key", out expectedData)).Returns(true);
        var service = new DataCachingService(_mockMemoryCache.Object, _mockLogger.Object);

        // Act
        var result = service.GetLatestDataFromCache();

        // Assert
        Assert.That(result, Is.EqualTo(expectedData));
    }

    [Test]
    public void GetLatestDataFromCache_CacheDoesNotContainData_ReturnsNull()
    {
        // Arrange
        List<DataPoint> data = null;
        _mockMemoryCache.Setup(mc => mc.TryGetValue("key", out data)).Returns(false);
        var service = new DataCachingService(_mockMemoryCache.Object, _mockLogger.Object);

        // Act
        var result = service.GetLatestDataFromCache();

        // Assert
        Assert.IsNull(result);
    }
}