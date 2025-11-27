namespace DevMarathon.Domain.Entities.SQL;

public class ChatMessageEntity : EntityBase
{
    public string Message { get; set; }
    public Guid RoomId { get; set; }
    public bool FromSystem { get; set; }
}