using DevMarathon.Domain;
using DevMarathon.Domain.Entities.SQL;
using DevMarathon.Identity;
using DevMarathon.Application.Contract.Services;
using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Application.Features.Sample.SampleService;
using DevMarathon.Domain.Enums;
using FluentValidation;
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
            UserId = user.Id.ToString(),
            TokenId = Guid.NewGuid().ToString(),
            ExpireTime = TimeSpan.FromMinutes(10),
            Roles = new List<string> { "TEMP_REGISTER_TOKEN" },
            OtherClaims = new List<KeyValuePair<string, string>>
            {
            },
            Reason = "ON_REGISTER"
        }, _configs.TokenConfigs);

        var activationCode = new Random().Next(100000, 999999).ToString();
        await _cachingService.Add($"ActivationCode_{user.Id}", activationCode, TimeSpan.FromMinutes(10));

        return new RegisterVm()
        {
            Code=activationCode,
            Token = token
        };
    }
}
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    IMessageService _messageService;

    public RegisterCommandValidator(IMessageService messageService)
    {
        _messageService = messageService;
        RuleFor(p => p.PhoneNumber)
            .NotNull().WithMessage(_messageService.GetMessage(MessageCodes.MESSAGE_REQUIRED_PARAM, Langs.FA));
        RuleFor(p => p.PhoneNumber)
            .Length(11).WithMessage(_messageService.GetMessage(MessageCodes.MESSAGE_INVALID_PHONE, Langs.FA));
        RuleFor(p => p.PhoneNumber)
            .Must(p => p.StartsWith("09"))
            .WithMessage(_messageService.GetMessage(MessageCodes.MESSAGE_INVALID_PHONE, Langs.FA));
    }
}
public class RegisterCommand : IRequest<RegisterVm>
{
    public string PhoneNumber { set; get; }
}

public class RegisterVm
{
    public string Token { set; get; }
    public string Code { get; set; }
}