using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DevMarathon.Infrastructure.SQLRepositories;

public class ChatRoomRepository : BaseRepository<ChatRoomEntity>, IChatRoomRepository
{
    public ChatRoomRepository(CleanContext dbContext, DapperContext dapperContext) : base(dbContext, dapperContext)
    {
    }

    public ChatRoomEntity GetByUserId(Guid userId)
    {
        var chatRoom = GetDBSetQuery().Where(x => x.UserId == userId).FirstOrDefault();
        return chatRoom;
    }
}