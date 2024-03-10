using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BitcoinQuery.DesktopClient.Contracts;
using BitcoinQuery.DesktopClient.Model;
using BitcoinQuery.DesktopClient.Service;
using BitcoinQuery.DesktopClient.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BitcoinQuery.DesktopClientUnitTests.ViewModel
{
    [TestClass]
    public class BitcoinDataTableViewModelTests
    {
        private Mock<IBitcoinRestClientService> mockBitcoinRestClientService;
        private Mock<INLogLogger> mockNLogLogger;
        private Mock<ISignalRService> signalRservice;
        private CancellationToken cancellationToken;

        [TestInitialize]
        public void TestInitialize()
        {
            mockBitcoinRestClientService = new Mock<IBitcoinRestClientService>();
            mockNLogLogger = new Mock<INLogLogger>();
            signalRservice = new Mock<ISignalRService>();
            cancellationToken = CancellationToken.None;
        }

        [TestMethod]
        public async Task UpdateBitcoinData_PopulatesAllBitcoinData()
        {
            // Arrange
            var viewModel = new BitcoinDataTableViewModel(
                mockBitcoinRestClientService.Object,
                signalRservice.Object,
                mockNLogLogger.Object,
                cancellationToken);

            var fakeData = new List<DataPoint>
            {
                new DataPoint { Timestamp = 1645699200, LastPrice = "10000" },
                new DataPoint { Timestamp = 1645785600, LastPrice = "10500" },
                new DataPoint { Timestamp = 1645785700, LastPrice = "10550" },
                new DataPoint { Timestamp = 1645785800, LastPrice = "10555" },
                new DataPoint { Timestamp = 1645785900, LastPrice = "10560" },
                new DataPoint { Timestamp = 1645786600, LastPrice = "10580" }
            };

            mockBitcoinRestClientService.Setup(x => x.GetDataFromRangeAsync(cancellationToken))
                .ReturnsAsync(fakeData);

            // Act
            await viewModel.UpdateBitcoinData();

            // Assert
            Assert.AreEqual(fakeData.Count, viewModel.AllBitcoinData.Count);
        }
    }
}