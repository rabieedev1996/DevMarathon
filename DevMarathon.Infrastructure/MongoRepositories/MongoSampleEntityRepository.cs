using DevMarathon.Application.Contract.MongoDB;
using DevMarathon.Domain;
using DevMarathon.Domain.Entities.Mongo;
using DevMarathon.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DevMarathon.Infrastructure.MongoRepositories;

public class MongoSampleEntityRepository : BaseRepository<MongoSampleEntity>, IMongoSampleEntityRepository
{
    public MongoSampleEntityRepository(Configs configuration) : base(configuration)
    {
    }
  
}