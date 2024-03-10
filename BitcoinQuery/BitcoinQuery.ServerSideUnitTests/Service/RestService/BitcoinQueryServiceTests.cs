using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Service.RestService;
using Moq;
using NLog;

namespace BitcoinQuery.ServerSideUnitTests.Service.RestService;

/// <summary>
///     Passed only with working server gateway BitcoinQuery.WebGateway
/// </summary>
[TestFixture]
public class BitcoinQueryServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger>();
        _mockBitcoinDataMapper = new Mock<IBitcoinDataMapper>();
        _mockDataCachingService = new Mock<IDataCachingService>();

        _service = new BitcoinQueryService(
            _mockLogger.Object,
            _mockBitcoinDataMapper.Object,
            _mockDataCachingService.Object,
            "https://cex.io/api/",
            -1,
            "BTC",
            "USD");
    }

    private MockRepository _mockRepository;

    private Mock<ILogger> _mockLogger;
    private Mock<IBitcoinDataMapper> _mockBitcoinDataMapper;
    private Mock<IDataCachingService> _mockDataCachingService;
    private BitcoinQueryService _service;

    [Test]
    public async Task GetLastPriceAsync_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var token = default(CancellationToken);
        var expectedBitcoinData = new BitcoinPriceData
        {
            LastPrice = "69432.9",
            FirstCurrency = "BTC",
            SecondCurrency = "USD"
        };

        // Act
        var result = await _service.GetLastPriceAsync(
            token);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(expectedBitcoinData.LastPrice, Is.EqualTo(result.LastPrice));
        Assert.That(expectedBitcoinData.FirstCurrency, Is.EqualTo(result.FirstCurrency));
        Assert.That(expectedBitcoinData.SecondCurrency, Is.EqualTo(result.SecondCurrency));
    }

    [Test]
    public async Task GetDailyDataAsyncAsync_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var token = default(CancellationToken);

        // Act
        var result = await _service.GetDailyDataAsync("20240308", token);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.DataPerMinute);
        Assert.IsNotNull(result.DataPerHour);
        Assert.IsNotNull(result.DataPerDay);
        if (result.DataPerHour != null)
        {
            Assert.IsNotEmpty(result.DataPerHour);
        }

        if (result.DataPerMinute != null)
        {
            Assert.IsNotEmpty(result.DataPerMinute);
        }

        if (result.DataPerDay != null)
        {
            Assert.IsNotEmpty(result.DataPerDay);
        }
    }
}