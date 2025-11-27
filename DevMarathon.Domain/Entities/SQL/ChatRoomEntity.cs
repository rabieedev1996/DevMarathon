namespace DevMarathon.Domain.Entities.SQL;

public class ChatRoomEntity : EntityBase
{
    public string Title { set; get; }
    public Guid UserId { set; get; }
}