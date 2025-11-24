using System.Reflection;
using AutoMapper;
using DevMarathon.Application.Behaviours;
using DevMarathon.Application.Common;
using DevMarathon.Application.ExceptionHandler;
using DevMarathon.Application.Features.Sample.SampleService;
using DevMarathon.Application.Mapping;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DevMarathon.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(Logging));
        services.AddValidatorsFromAssemblyContaining<SampleServiceValidator>();
        services.AddScoped<ApiResponseException>();
        services.AddScoped(provider =>
          new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); }).CreateMapper());
        return services;
    }
}