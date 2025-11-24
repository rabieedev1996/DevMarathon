using DevMarathon.Application.Contract.Services;
using DevMarathon.Domain;
using DevMarathon.Infrastructure.ServiceImpls.SmsImpl;
using DevMarathon.Infrastructure.ServiceImpls.SMSImpl.FarazSMS;

namespace DevMarathon.Infrastructure.Service;

public class SmsService : ISmsService
{
    private ISmsImpl _provider;

    public SmsService(Configs configs)
    {
        _provider = new FarazSmsService(configs);
    }
    public async Task Send(string dest, string text)
    {
        await _provider.Send(new List<string> { dest }, text);
    }

    public async Task SendCode(string dest, string code)
    {
        await _provider.SendCode(dest, code);
    }
}