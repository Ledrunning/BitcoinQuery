namespace BitcoinQuery.Service.Models;

public class BitcoinData
{
    public long Timestamp { get; set; }
    public List<DataPoint>? MinuteData { get; set; }
    public List<DataPoint>? HourData { get; set; }
    public List<DataPoint>? DayData { get; set; }
}