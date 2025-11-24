namespace DevMarathon.Application.Contract.Services;

public interface ICachingService
{
    Task Add<TModel>(string key, TModel data, TimeSpan expire);
    Task Remove(string key);
    Task<TModel> Get<TModel>(string key);
}