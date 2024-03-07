namespace BitcoinQuery.WebGateway.Configuration;

public class CexConfigSection
{
    public const string SectionName = "CexApi";
    public string? BaseUrl { get; set; }
    public int Timeout { get; set; }
    public string? FirstCurrency { get; set; }
    public string? SecondCurrency { get; set; }
}