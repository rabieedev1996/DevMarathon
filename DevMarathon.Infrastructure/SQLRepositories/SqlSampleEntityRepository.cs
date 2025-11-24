using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevMarathon.Infrastructure.SQLRepositories;

public class SqlSampleEntityRepository : BaseRepository<SQLSampleEntity>, ISqlSampleEntityRepository
{
    public SqlSampleEntityRepository(CleanContext dbContext, DapperContext dapperContext) : base(dbContext, dapperContext)
    {
    }

}