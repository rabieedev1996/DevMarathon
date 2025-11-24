using DevMarathon.Application.Contract.Database.SQLDB;
using DevMarathon.Domain.Entities.Mongo;
using DevMarathon.Domain.Entities.SQL;

namespace DevMarathon.Application.Contract.SQLDB;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    UserEntity GetByPhoneNumber(string phoneNumber);
}