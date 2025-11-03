namespace SmartGymTracker.Api.Models;

public sealed class ApiMessage
{
    public ApiMessage(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
