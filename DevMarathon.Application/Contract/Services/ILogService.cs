namespace DevMarathon.Application.Contract.Services;

public interface ILogService
{
    Task InsertLog<TData>(string category, TData body);
}