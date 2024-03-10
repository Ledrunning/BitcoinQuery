using Newtonsoft.Json;

namespace BitcoinQuery.Service.Attributes;

/// <summary>
///     This converter is using for deserealise arrays of string into arrays of double
/// </summary>
public class DoubleArrayConverter : JsonConverter<double[][]>
{
    public override double[][]? ReadJson(JsonReader reader, Type objectType, double[][] existingValue,
        bool hasExistingValue, JsonSerializer serializer)
    {
        var data = (string)reader.Value;
        var array = JsonConvert.DeserializeObject<double[][]?>(data);
        return array;
    }

    public override void WriteJson(JsonWriter writer, double[][] value, JsonSerializer serializer)
    {
        throw new JsonSerializationException(
            $"{nameof(DoubleArrayConverter)}: Serialization of double[][] is not supported.");
    }
}