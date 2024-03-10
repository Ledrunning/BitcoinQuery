using System.Threading;
using System.Threading.Tasks;
using BitcoinQuery.DesktopClient.Configuration;
using BitcoinQuery.DesktopClient.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BitcoinQuery.DesktopClientUnitTests.Rest
{
    [TestClass]
    public class BitcoinRestClientServiceTests
    {
        private Mock<AppConfig> _mockAppConfig;
        private BitcoinRestClientService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockAppConfig = new Mock<AppConfig>();
            _service = new BitcoinRestClientService(_mockAppConfig.Object);
        }

        [TestMethod]
        public async Task GetDataFromRangeAsync_ValidData_ReturnsExpectedResult()
        {
            // Arrange
            var expectedApiUrl = "https://localhost:7186/api/BitcoinPrice";
            var timeout = -1;
            _mockAppConfig.Setup(x => x.LoadConfiguration().BaseServerUri).Returns(expectedApiUrl);
            _mockAppConfig.Setup(x => x.LoadConfiguration().Timeout).Returns(timeout);

            // Act
            var result = await _service.GetDataFromRangeAsync(CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Count > 0);
        }
    }
}