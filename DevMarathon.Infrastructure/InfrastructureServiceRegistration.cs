using AutoMapper;
using DevMarathon.Application;
using DevMarathon.Application.Common;
using DevMarathon.Application.Contract.Services;
using DevMarathon.Application.Contract.SQLDB;
using DevMarathon.Domain;
using DevMarathon.Domain.Enums;
using DevMarathon.Infrastructure.Persistence;
using DevMarathon.Infrastructure.Service;
using DevMarathon.Infrastructure.ServiceImpls.MessagingImpl;
using DevMarathon.Infrastructure.SQLRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace DevMarathon.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        ConfigurationManager configurationManager,Configs configs)
    {
        //PostgreSql
        services.AddEntityFrameworkNpgsql().AddDbContext<CleanContext>(opt =>
            opt.UseNpgsql(configurationManager.GetConnectionString("CleanPostgresDB")));

        //Sql Server
        /*
        services.AddDbContext<CleanContext>(options => options.UseSqlServer(configurationManager.GetConnectionString("CleanSqlServer")));
        */

        services.AddScoped(typeof(IEmailService), typeof(EmailService));
        services.AddScoped(typeof(ISmsService), typeof(SmsService));
        services.AddScoped(typeof(IReportService), typeof(ReportService));
        services.AddScoped(typeof(ILogService), typeof(LogService));
        services.AddScoped(typeof(ICachingService), typeof(CachingService));
        services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        services.AddTransient<IFileService>(s => new FileService(configs));
        services.AddTransient<IImageService>(s => new ImageService(configs));
        services.AddScoped(typeof(UserContext));
        services.AddScoped(typeof(DapperContext));
        services.AddScoped(typeof(ResponseGenerator));

        services.AddScoped(typeof(IMessageService), typeof(MessageService));
        services.AddScoped(typeof(En_MessagesImpl));
        services.AddScoped(typeof(Fa_MessagesImpl));


        return services;
    }
}