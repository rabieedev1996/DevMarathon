using DevMarathon.Application;
using DevMarathon.Application.Features.Chat;
using DevMarathon.Application.Features.Chat.Commands;
using DevMarathon.Application.Features.Chat.Queries;
using DevMarathon.Application.Models;
using DevMarathon.Domain;
using DevMarathon.Identity;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace DevMarathon.Api.SignalR;

public class SignalRHub : Hub
{
    Configs _config;
    UserContext _userContext;
    public static List<SignalConnectionDTO> _Connections=new();
    IMediator _Mediator;

    public SignalRHub(Configs configs, UserContext userContext, IMediator mediator)
    {
        _config = configs;
        _userContext = userContext;
        _Mediator = mediator;
    }

  

    public async Task SendMessage(ReceiveMessageCommand query)
    {
        var connection = _Connections.FirstOrDefault(a => a.ConnectionId == Context.ConnectionId);
        try
        {
            _userContext.DeviceId = connection.DeviceId;
            _userContext.UserId = connection.UserId;
            var resultData = await _Mediator.Send(query);
        }
        catch (Exception ex) {
        }
    }
    public async Task GetMessages(string callBackAction)
    {
        var connection = _Connections.FirstOrDefault(a => a.ConnectionId == Context.ConnectionId);
        try
        {
            _userContext.DeviceId = connection.DeviceId;
            _userContext.UserId = connection.UserId;
            var resultData = await _Mediator.Send(new GetChatRoomMessagesQuery());
            
            await Clients.Client(connection.ConnectionId).SendAsync(callBackAction,resultData); 
        }
        catch (Exception ex) {
        }
    }
    
    
    public override Task OnConnectedAsync()
    {
        try
        {
            var token = Context.GetHttpContext().Request.Query["authorization"].ToString();
            var deviceId = Context.GetHttpContext().Request.Query["clientId"].ToString();

            var principals = IdentityUtility.GetPrincipals(_config.TokenConfigs.Key, _config.TokenConfigs.Audience,
                _config.TokenConfigs.Issuer, token);
            string userId = principals.Claims.FirstOrDefault(claim => claim.Type == "UserId")?.Value;
            _Connections.Add(new SignalConnectionDTO()
            {
                ConnectionId = Context.ConnectionId,
                UserId = userId,
                DeviceId = deviceId,
            });
            _userContext.DeviceId = deviceId;
            _userContext.UserId = userId;
        }
        catch (Exception ex)
        {
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var connection = _Connections.FirstOrDefault(a => a.ConnectionId == Context.ConnectionId);
            _Connections.Remove(connection);
        }
        catch (Exception ex)
        {
        }

        return base.OnDisconnectedAsync(exception);
    }
}