using BitcoinQuery.Service.Conrtacts;
using BitcoinQuery.Service.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinQuery.WebGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitcoinPriceController : ControllerBase
    {
        private readonly IBitcoinQueryService _bitcoinQueryService;
        public BitcoinPriceController(IBitcoinQueryService bitcoinQueryService)
        {
            _bitcoinQueryService = bitcoinQueryService;
        }

        [HttpGet]
        [Route(nameof(GetLastPrice))]
        public async Task<BitcoinPriceData> GetLastPrice(CancellationToken token)
        {
            return await _bitcoinQueryService.GetLastPrice(token);
        }
    }
}
