using BitcoinQuery.Service.Contracts;
using BitcoinQuery.Service.Dto;
using BitcoinQuery.Service.Models;
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
        return await _bitcoinQueryService.GetLastPriceAsync(token);
    }

    [HttpGet]
    [Route(nameof(GetDailyData))]
    public async Task<BitcoinDailyData> GetDailyData(string date, CancellationToken token)
    {
        return await _bitcoinQueryService.GetDailyDataAsync(date, token);
    }

    [HttpGet]
    [Route(nameof(GetDataFromRange))]
    public async Task<List<DataPoint>?> GetDataFromRange(CancellationToken token)
    {
        return await _bitcoinQueryService.GetDataFromRangeAsync(token);
    }
}