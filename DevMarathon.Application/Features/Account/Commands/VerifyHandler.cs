using DevMarathon.Domain;
using DevMarathon.Domain.Enums;
using DevMarathon.Identity;
using DevMarathon.Application.Contract.Services;
using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Application.ExceptionHandler;
using MediatR;

namespace DevMarathon.Application.Features.Account.Commands;

public class VerifyCommandHandler : IRequestHandler<VerifyCommand, VerifyVm>
{
    IUserRepository _userRepository;
    UserContext _userContext;
    ICachingService _cachingService;
    ApiResponseException _apiResponseException;
    Configs _configs;

    public VerifyCommandHandler(IUserRepository userRepository, UserContext userContext, ICachingService cachingService,
        ApiResponseException apiResponseException, Configs configs)
    {
        _userRepository = userRepository;
        _userContext = userContext;
        _cachingService = cachingService;
        _apiResponseException = apiResponseException;
        _configs = configs;
    }


    public async Task<VerifyVm> Handle(VerifyCommand request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetById(Guid.Parse(_userContext.UserId));
        var activeCode = await _cachingService.Get<string>($"ActivationCode_{user.Id}");
        if (string.IsNullOrEmpty(activeCode))
        {
            _apiResponseException.SetDetail(ResponseCodes.INVALID_ACTIVATION_CODE);
            throw _apiResponseException;
        }

        if (activeCode != request.ActivationCode)
        {
            _apiResponseException.SetDetail(ResponseCodes.INVALID_ACTIVATION_CODE);
            throw _apiResponseException;
        }

        user.Verified = true;
        await _userRepository.UpdateAsync(user);
        await _cachingService.Remove($"ActivationCode_{user.Id}");
        var token = IdentityUtility.GenerateToken(new TokenParams
        {
            UserId = Guid.NewGuid().ToString(),
            TokenId = Guid.NewGuid().ToString(),
            ExpireTime = TimeSpan.FromDays(30),
            Roles = new List<string> { "GENERAL_TOKEN" },
            OtherClaims = new List<KeyValuePair<string, string>>(),
            Reason = "ON_REGISTER"
        }, _configs.TokenConfigs);
        return new VerifyVm()
        {
            Token = token
        };
    }
}

public class VerifyCommand : IRequest<VerifyVm>
{
    public string ActivationCode { set; get; }
}

public class VerifyVm
{
    public string Token { set; get; }
}