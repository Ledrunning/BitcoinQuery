using BitcoinQuery.WebGateway.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BitcoinQuery.WebGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VersionController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok($"Version - {ProgramRuntime.ProgramVersion}");
    }
}