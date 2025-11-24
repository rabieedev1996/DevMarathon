using DevMarathon.Domain;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Identity;
using DevMarathon.Application.Contract.Services;
using DevMarathon.Application.Contract.SQLDB;
using MediatR;

namespace DevMarathon.Application.Features.Account.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterVm>
{
    private IUserRepository _userRepository;
    private Configs _configs;
    ICachingService _cachingService;

    public RegisterCommandHandler(IUserRepository userRepository, Configs configs, ICachingService cachingService)
    {
        _userRepository = userRepository;
        _configs = configs;
        _cachingService = cachingService;
    }

    public async Task<RegisterVm> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetByPhoneNumber(request.PhoneNumber);
        if (user == null)
        {
            user = new UserEntity()
            {
                PhoneNumber = request.PhoneNumber,
            };
            await _userRepository.AddAsync(user);
        }

        var token = IdentityUtility.GenerateToken(new TokenParams
        {
            UserId = Guid.NewGuid().ToString(),
            TokenId = Guid.NewGuid().ToString(),
            ExpireTime = TimeSpan.FromMinutes(2),
            Roles = new List<string> { "TEMP_REGISTER_TOKEN" },
            OtherClaims = new List<KeyValuePair<string, string>>
            {
            },
            Reason = "ON_REGISTER"
        }, _configs.TokenConfigs);

        var activationCode = new Random().Next(100000, 999999).ToString();
        await _cachingService.Add($"ActivationCode_{user.Id}", activationCode, TimeSpan.FromMinutes(2));

        return new RegisterVm()
        {
            Token = token
        };
    }
}

public class RegisterCommand : IRequest<RegisterVm>
{
    public string PhoneNumber { set; get; }
}

public class RegisterVm
{
    public string Token { set; get; }
}