using DevMarathon.Domain.Entities.Mongo;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Domain.Enums;

namespace DevMarathon.Application.Contract.MongoDB;

public interface IMongoSampleEntityRepository:IBaseRepository<MongoSampleEntity>
{

}