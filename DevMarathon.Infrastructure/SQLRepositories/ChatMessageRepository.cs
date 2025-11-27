using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevMarathon.Infrastructure.SQLRepositories;

public class ChatMessageRepository : BaseRepository<ChatMessageEntity>, IChatMessageRepository
{
    public ChatMessageRepository(CleanContext dbContext, DapperContext dapperContext) : base(dbContext, dapperContext)
    {
    }

    public List<ChatMessageEntity> GetByRoom(Guid roomId)
    {
        return GetDBSetQuery().Where(a=>a.RoomId == roomId).OrderByDescending(a=>a.CreatedAt).ToList();
    }
}