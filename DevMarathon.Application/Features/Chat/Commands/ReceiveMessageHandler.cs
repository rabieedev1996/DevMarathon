using DevMarathon.Api.SignalR;
using DevMarathon.Application.Common;
using DevMarathon.Application.Contract.Services;
using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Application.ExceptionHandler;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Domain.Enums;
using DevMarathon.Utility;
using MediatR;

namespace DevMarathon.Application.Features.Chat.Commands;

public class ReceiveMessageCommandHandler : IRequestHandler<ReceiveMessageCommand, ReceiveMessageCommandVM>
{
    ApiResponseException _apiResponseException;
    IUserRepository _userRepository;
    IChatRoomRepository _chatRoomRepository;
    IChatMessageRepository _chatMessageRepository;
    UserContext _userContext;
    ISocketService _socketService;
    ResponseGenerator _responseGenerator;

    public ReceiveMessageCommandHandler(ApiResponseException apiResponseException, IUserRepository userRepository,
        UserContext userContext, IChatRoomRepository chatRoomRepository, IChatMessageRepository chatMessageRepository,
        ISocketService socketService, ResponseGenerator responseGenerator)
    {
        _apiResponseException = apiResponseException;
        _userRepository = userRepository;
        _userContext = userContext;
        _chatRoomRepository = chatRoomRepository;
        _chatMessageRepository = chatMessageRepository;
        _socketService = socketService;
        _responseGenerator = responseGenerator;
    }

    public async Task<ReceiveMessageCommandVM> Handle(ReceiveMessageCommand request,
        CancellationToken cancellationToken)
    {
        var chatRoom = _chatRoomRepository.GetByUserId(Guid.Parse(_userContext.UserId));
        if (chatRoom == null)
        {
            chatRoom = new ChatRoomEntity();
            chatRoom.UserId = Guid.Parse(_userContext.UserId);
            await _chatRoomRepository.AddAsync(chatRoom);
        }

        var connections = SignalRHub._Connections.FirstOrDefault(a => a.UserId == _userContext.UserId
                                                                      && a.DeviceId == _userContext.DeviceId);
        var chatMessage = new ChatMessageEntity()
        {
            RoomId = chatRoom.Id,
            Message = request.Message,
        };
        await _chatMessageRepository.AddAsync(chatMessage);
        var socketModel = new ReceiveMessageCommand_SocketModel
        {
            Date = DateTime.Now.ToFa("yyyy-MM-dd HH:mm:ss"),
            FromSystem = false,
            Message = request.Message,
        };
        await _socketService.PushToClient(connections.ConnectionId, "NEW_MESSAGE", socketModel);

          
        
        var answerMessage = new ChatMessageEntity()
        {
            RoomId = chatRoom.Id,
            Message = "پیام پاسخ در ساعت" + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        };
        await _chatMessageRepository.AddAsync(answerMessage);
        var answerSocketModel = new ReceiveMessageCommand_SocketModel
        {
            Date = DateTime.Now.ToFa("yyyy-MM-dd HH:mm:ss"),
            FromSystem = true,
            Message = answerMessage.Message,
        };
        await _socketService.PushToClient(connections.ConnectionId, "NEW_MESSAGE", answerSocketModel);

        return new ReceiveMessageCommandVM();
    }
}

public class ReceiveMessageCommand : IRequest<ReceiveMessageCommandVM>
{
    public string Message { set; get; }
}

public class ReceiveMessageCommandVM
{
}

public class ReceiveMessageCommand_SocketModel
{
    public string Message { get; set; }
    public bool FromSystem { get; set; }
    public string Date { set; get; }
}