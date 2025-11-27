using DevMarathon.Application.Common;
using DevMarathon.Application.Contract.Services;
using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Application.ExceptionHandler;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Utility;
using MediatR;

namespace DevMarathon.Application.Features.Chat.Queries;

public class GetChatRoomMessagesQueryHandler:IRequestHandler<GetChatRoomMessagesQuery,GetChatRoomMessagesQueryVM>
{
    ApiResponseException _apiResponseException;
    IUserRepository _userRepository;
    IChatRoomRepository _chatRoomRepository;
    IChatMessageRepository _chatMessageRepository;
    UserContext _userContext;
    ISocketService _socketService;
    ResponseGenerator _responseGenerator;

    public GetChatRoomMessagesQueryHandler(ApiResponseException apiResponseException, IUserRepository userRepository, IChatRoomRepository chatRoomRepository, IChatMessageRepository chatMessageRepository, UserContext userContext, ISocketService socketService, ResponseGenerator responseGenerator)
    {
        _apiResponseException = apiResponseException;
        _userRepository = userRepository;
        _chatRoomRepository = chatRoomRepository;
        _chatMessageRepository = chatMessageRepository;
        _userContext = userContext;
        _socketService = socketService;
        _responseGenerator = responseGenerator;
    }

    public async Task<GetChatRoomMessagesQueryVM> Handle(GetChatRoomMessagesQuery request, CancellationToken cancellationToken)
    {
        var chatRoom = _chatRoomRepository.GetByUserId(Guid.Parse(_userContext.UserId));
        if (chatRoom is null)
        {
            return new GetChatRoomMessagesQueryVM
            {
                Messages = new List<GetChatRoomMessagesQueryVM_Messages>()
            };
        }
        var messages=_chatMessageRepository.GetByRoom(chatRoom.Id);
        var resultList=new List<GetChatRoomMessagesQueryVM_Messages>();
        foreach (var message in messages)
        {
            resultList.Add(new GetChatRoomMessagesQueryVM_Messages
            {
                Message = message.Message,
                FromSystem = message.FromSystem,
                Date = message.CreatedAt.ToFa("yyyy-MM-dd HH:mm:ss"),
            });
        }

        return new GetChatRoomMessagesQueryVM()
        {
            Messages = resultList,
        };
    }
}
public class GetChatRoomMessagesQuery:IRequest<GetChatRoomMessagesQueryVM>
{
    
}
public class GetChatRoomMessagesQueryVM
{
    public List<GetChatRoomMessagesQueryVM_Messages> Messages { get; set; } = new();
}
public class GetChatRoomMessagesQueryVM_Messages
{
    public string Message { get; set; }
    public bool FromSystem { get; set; }
    public string Date{set;get;}
}