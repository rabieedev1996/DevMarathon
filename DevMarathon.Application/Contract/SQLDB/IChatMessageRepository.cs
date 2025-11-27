using DevMarathon.Application.Contract.Database.SQLDB;
using DevMarathon.Domain.Entities.Mongo;
using DevMarathon.Domain.Entities.SQL;

namespace DevMarathon.Application.Contract.SQLDB;

public interface IChatMessageRepository : IBaseRepository<ChatMessageEntity>
{
}