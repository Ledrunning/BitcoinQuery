using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinQuery.WebGateway.Controllers;

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

    [HttpGet]
    [Route(nameof(GetDailyData))]
    public async Task<BitcoinDailyData> GetDailyData(string date, CancellationToken token)
    {
        return await _bitcoinQueryService.GetDailyData(date, token);
    }

    [HttpGet]
    [Route(nameof(GetDataFromRange))]
    public async Task<List<double[][]>> GetDataFromRange(CancellationToken token)
    {
        return await _bitcoinQueryService.GetDataFromRange(token);
    }
}