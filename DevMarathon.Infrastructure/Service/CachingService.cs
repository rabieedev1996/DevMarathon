using DevMarathon.Application.Contract.Services;
using DevMarathon.Domain;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DevMarathon.Infrastructure.Service;

public class CachingService : ICachingService,IDisposable
{
    IDatabase _redisDB;
    ConnectionMultiplexer _redis;
    public CachingService(Configs config)
    {
        // Connect to the Redis server
           
        _redis = ConnectionMultiplexer.Connect(config.RedisConfigs.Endpoint);
           
        // Get a reference to the Redis database
        _redisDB = _redis.GetDatabase();
        // ... Your Redis operations go here ...
        //// Disconnect from Redis
        //redis.Close();
    }
    public async Task Add<TModel>(string key, TModel data, TimeSpan expire)
    {
        _redisDB.StringSet(key, JsonConvert.SerializeObject(data), expire);
    }

      

    public async Task<TModel> Get<TModel>(string key)
    {
        return JsonConvert.DeserializeObject<TModel>(_redisDB.StringGet(key).ToString());
    }

    public async Task Remove(string key)
    {
        _redisDB.KeyDelete(key);
    }

    public void Dispose()
    {
        try
        {
            _redis.Close();
        }
        catch (Exception ex) { }
    }
}