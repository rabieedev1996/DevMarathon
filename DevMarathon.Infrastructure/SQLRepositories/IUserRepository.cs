using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevMarathon.Infrastructure.SQLRepositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(CleanContext dbContext, DapperContext dapperContext) : base(dbContext, dapperContext)
    {
    }

    public UserEntity GetByPhoneNumber(string phoneNumber)
    {
        var user=GetDBSetQuery().FirstOrDefault(a=>a.PhoneNumber==phoneNumber); 
        return user;    
    }
}