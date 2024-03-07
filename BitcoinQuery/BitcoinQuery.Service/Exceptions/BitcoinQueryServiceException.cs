namespace BitcoinQuery.Service.Exceptions;

public class BitcoinQueryServiceException : Exception
{
    private readonly string? _errorCode;
    private readonly string? _errorMessage;

    public BitcoinQueryServiceException(string message) : base(message)
    {
    }

    public BitcoinQueryServiceException(string message, string? errorCode, string? errorMessage) : base(message)
    {
        _errorCode = errorCode;
        _errorMessage = errorMessage;
    }
}