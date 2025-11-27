namespace DevMarathon.Application.Contract.Services;

public interface ISocketService
{
    Task PushToClient<T>(string connectionId, string title, T data);
    Task Broadcast<T>(string title, T data);
    Task PushToClient<T>(List<string> connectionIds, string title, T data);
}